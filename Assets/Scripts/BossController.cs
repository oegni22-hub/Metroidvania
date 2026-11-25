using System.Collections;
using System;
using UnityEngine;

public class BossController : EnemyController
{
    enum BossStates { Waiting, Roar, Roll, SpikeAttack, Jump, Moving, Death }

    [SerializeField] private BossStates bossState;
    [SerializeField] private GameObject hitSprite;
    private CapsuleCollider2D collider;
    [SerializeField] private GameObject gema;
    
    public static Action OnBossDefeated;
    
    [Header("Particles")]
    [SerializeField] private ParticleSystem runingParticles;
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
    [SerializeField] private float knockBackForce;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GameManager.instance.scenarioData.Boss1Defeated == true)
        {
            Destroy(gameObject);
        }
        
        base.Start();

        player = GameObject.Find("Prota").transform;
        collider = GetComponent<CapsuleCollider2D>();

        StartBoss.OnCombatStart += StartCombat;
    }

    void StartCombat()
    {
        ChangeState(BossStates.Roar);
    }

    // Update is called once per frame
    void Update()
    {
        
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
        int azar = UnityEngine.Random.Range(1, 6);
        ChangeState((BossStates)azar);
    }

    IEnumerator Roar()
    {
        animator.SetBool("Roar", true);
        yield return new WaitForSeconds(0.5f);
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
            int rocaAzar = UnityEngine.Random.Range(0, rocasPrefab.Length);
            Vector2 randomPos = new Vector2(UnityEngine.Random.Range(rocaSpawnMin.position.x, rocaSpawnMax.position.x),
                rocaSpawnMin.position.y);
            Quaternion randomRot = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360));
            GameObject rocaClon = Instantiate(rocasPrefab[rocaAzar], randomPos, randomRot);
            rocaClon.GetComponent<ConstantForce2D>().torque = UnityEngine.Random.Range(225, 275);
            timePass += rocasSpawnRate;
            yield return new WaitForSeconds(rocasSpawnRate);
        }
    }

    IEnumerator Roll()
    {
        animator.SetBool("Roll", true);
        yield return new WaitForSeconds(1.292f);
        rolling = true;
        runingParticles.gameObject.SetActive(true);
        runingParticles.Play();
        collider.size = new Vector2(2.5f, 2.5f);
        while (rolling)
        {
            rb.linearVelocity = transform.right * rollSpeed;
            yield return null;
        }
        Camera.main.GetComponent<CameraController>().StartShake(0.5f);
        collider.size = new Vector2(3.8f, 2.5f);
        animator.SetBool("Roll", false);
        rb.linearVelocity = Vector2.zero;
        runingParticles.Pause();
        runingParticles.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        yield return new WaitForSeconds(0.5f);

        ChangeState(BossStates.Waiting);
    }

    IEnumerator Movement()
    {
        LookTo(pointToMove);
        animator.SetBool("Walk", true);

        bool walking = true;
        runingParticles.Play();
        runingParticles.gameObject.SetActive(true);

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
        runingParticles.gameObject.SetActive(false);

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
        Camera.main.GetComponent<CameraController>().StartShake(0.5f);
        animator.SetBool("Jump", false);
        //          Poner particulas aterrizar
        yield return new WaitForSeconds(0.58f);
        ChangeState(BossStates.Waiting);
    }

    IEnumerator SpikeAttack()
    {
        animator.SetBool("Spikes", true);
        yield return new WaitForSeconds(0.5f);
        collider.size = new Vector2(2.5f, 2.5f);
        yield return new WaitForSeconds(2.5f);
        animator.SetBool("Spikes", false);
        yield return new WaitForSeconds(0.83f);
        collider.size = new Vector2(3.8f, 2.5f);
        ChangeState(BossStates.Waiting);
    }

    void Death()
    {
        StopAllCoroutines();
        OnBossDefeated();
        GameManager.instance.scenarioData.Boss1Defeated = true;
        //Soltar loot
        StartCoroutine(SoltarLoot());
    }

    IEnumerator SoltarLoot()
    {
        yield return new WaitForSeconds(0.5f);
        Instantiate(gema, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), transform.rotation);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (isDead == true)
        {
            ChangeState(BossStates.Death);
            rb.linearVelocity = Vector2.zero;
            hitSprite.SetActive(false);
            runingParticles.Pause();
            runingParticles.gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(Hit());
        }
    }

    IEnumerator Hit()
    {
        hitSprite.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        hitSprite.SetActive(false);
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
        if (col.gameObject.tag == "BossDoor")
        {
            rolling = false;
        }
        
        if (col.gameObject.tag == "Player")
        {
            rolling = false;
            col.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
            
            if (bossState == BossStates.Jump)
            {
                Vector3 direccion = col.transform.position - transform.position;
                Vector2 direccionKnockBack = Vector2.zero;
                if (direccion.x > 0)
                {
                    direccionKnockBack = Vector2.left;
                }
                else if (direccion.x <= 0)
                {
                    direccionKnockBack = Vector2.right;
                }
                col.gameObject.GetComponent<PlayerController>().KnockBack(direccionKnockBack, knockBackForce);

            }
        }
    }

    void OnDisable()
    {
        StartBoss.OnCombatStart -= StartCombat;
    }
}
