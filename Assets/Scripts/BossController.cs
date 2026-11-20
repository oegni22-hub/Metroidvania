using System.Collections;
using System.Reflection.Metadata;
using UnityEngine;

public class BossController : EnemyController
{
    enum BossStates { Waiting, Roar, Roll, SpikeAttack, Jump, Moving, Death }

    [SerializeField] private BossStates bossState;
    [Header("Particles")]
    [SerializeField] private ParticleSystem runingParticles;
    [SerializeField] private ParticleSystem collisionParticles;

    [Header("Rolling Attack")] [SerializeField]
    private float rollSpeed;

    private bool rolling = false;
    [Header("Movement")] 
    [SerializeField] private Transform pointToMove;

    [Header("Spikes Attack")] [SerializeField]
    private GameObject spikesPrefab;

    [SerializeField] private Transform[] spikesSpawnPoints;
    [Header("Roar")] [SerializeField] private GameObject[] rocasPrefab;
    [SerializeField] private Transform rocaSpawnMin, rocaSpawnMax;
    [SerializeField] private float rocasSpawnRate;
    [Header("Jump")] [SerializeField] private float jumpHeight;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float minX, maxX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base.Start();

        player = GameObject.Find("Prota").transform;

        ChangeState(BossStates.Roar);
    }

    // Update is called once per frame
    void Update()
    {
        /*if (isDead)
        {
            ChangeState(BossStates.Death);
        }*/
    }

    void ChangeState(BossStates newState)
    {
        LookTo(player);

        bossState = newState;
        switch (bossState)
        {
            case BossStates.Waiting:
                StartCoroutine(Waiting());
                break;
            case BossStates.Roar:
                StartCoroutine(Roar());
                break;
            case BossStates.Roll:
                StartCoroutine(Roll());
                break;
            case BossStates.Death:
                Death();
                break;
            case BossStates.Moving:
                StartCoroutine(Movement());
                break;
            case BossStates.Jump:
                StartCoroutine(Jump());
                break;
            case BossStates.SpikeAttack:
                StartCoroutine(SpikeAttack());
                break;
            default:
                break;
        }
    }

    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(1);
        int azar = Random.Range(1, 6);
        ChangeState((BossStates)azar);
    }

    IEnumerator Roar()
    {
        animator.SetBool("Roar", true);
        yield return new WaitForSeconds(0.5f);
        //tiembla la camara
        Camera.main.GetComponent<CameraController>().StartShake(3);
        yield return StartCoroutine(FallingRocks());
        animator.SetBool("Roar", false);
        yield return new WaitForSeconds(0.55f);
        ChangeState(BossStates.Waiting);
    }

    IEnumerator FallingRocks()
    {
        float timePass = 0;
        while (timePass < 3)
        {
            int rocaAzar = Random.Range(0, rocasPrefab.Length);
            Vector2 randomPos = new Vector2(Random.Range(rocaSpawnMin.position.x, rocaSpawnMax.position.x),
                rocaSpawnMin.position.y);
            Quaternion randomRot = Quaternion.Euler(0, 0, Random.Range(0, 360));
            GameObject rocaClon = Instantiate(rocasPrefab[rocaAzar], randomPos, randomRot);
            rocaClon.GetComponent<ConstantForce2D>().torque = Random.Range(225, 275);
            timePass += rocasSpawnRate;
            yield return new WaitForSeconds(rocasSpawnRate);
        }
    }

    IEnumerator Roll()
    {
        animator.SetBool("Roll", true);
        yield return new WaitForSeconds(1.292f);
        rolling = true;
        runingParticles.Play();
        
        while (rolling)
        {
            rb.linearVelocity = transform.right * rollSpeed;
            yield return null;
        }

        animator.SetBool("Roll", false);
        rb.linearVelocity = Vector2.zero;
        runingParticles.Pause();
        collisionParticles.Play();
        yield return new WaitForSeconds(0.5f);
        collisionParticles.Stop();
        yield return new WaitForSeconds(0.5f);

        ChangeState(BossStates.Waiting);
    }

    IEnumerator Movement()
    {
        LookTo(pointToMove);
        animator.SetBool("Walk", true);

        bool walking = true;
        runingParticles.Play();

        while (walking)
        {
            rb.linearVelocity = transform.right * speed;

            if (transform.position.x < pointToMove.position.x + distanceToAttack &&
                transform.position.x > pointToMove.position.x - distanceToAttack)
            {
                rb.linearVelocity = Vector2.zero;
                animator.SetBool("Walk", false);
                walking = false;
            }

            yield return null;
        }
        runingParticles.Pause();

        ChangeState(BossStates.Waiting);
    }

    IEnumerator Jump()
    {
        Vector3 initialPos = transform.position;
        float finishPosX = Mathf.Clamp(player.position.x, minX, maxX);
        float t = 0;
        animator.SetBool("Jump", true);
        yield return new WaitForSeconds(0.7f);

        //Parabola
        while (t < 1)
        {
            t += Time.deltaTime * jumpSpeed;
            float x = Mathf.Lerp(initialPos.x, finishPosX, t);
            float tY = 4f * t * (1f - t);
            float y = Mathf.Lerp(initialPos.y, jumpHeight, tY);
            transform.position = new Vector3(x, y, transform.position.z);
            yield return null;
        }

        animator.SetBool("Jump", false);
        //          Poner particulas aterrizar
        yield return new WaitForSeconds(0.58f);
        ChangeState(BossStates.Waiting);
    }

    IEnumerator SpikeAttack()
    {
        animator.SetBool("Spikes", true);
        yield return new WaitForSeconds(3);
        animator.SetBool("Spikes", false);
        yield return new WaitForSeconds(0.83f);
        ChangeState(BossStates.Waiting);
    }

    void Death()
    {
        StopAllCoroutines();
        //Soltar loot y abrir puerta
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (isDead == true)
        {
            ChangeState(BossStates.Death);
            runingParticles.Pause();
            //         Parar particulas
        }
    }

void LookTo(Transform toLook)
    {
        if (toLook.position.x > transform.position.x)
        {
            //Player a la dreta
            transform.eulerAngles = Vector3.zero;
        }
        else if (toLook.position.x < transform.position.x)
        {
            //Player a l'esquerra
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    public void ThrowSpikes()
    {
        for (int i = 0; i < spikesSpawnPoints.Length; i++)
        {
            GameObject tempSpike = Instantiate(spikesPrefab, spikesSpawnPoints[i].position, spikesSpawnPoints[i].rotation);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        rolling = false;
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
        }
    }
}
