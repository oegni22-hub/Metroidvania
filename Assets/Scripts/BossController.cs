using System.Collections;
using UnityEngine;

public class BossController : EnemyController
{
    enum BossStates {Waiting, Roar, Roll, SpikeAttack, Jump, Moving, Death}

    [SerializeField] private BossStates bossState;
    [SerializeField] private float rollSpeed;
    
    private bool rolling = false;
    [SerializeField] private Transform pointToMove;
    [SerializeField] private Transform[] spikesSpawnPoints;
    [SerializeField] private GameObject spikesPrefab;
    [SerializeField] private Transform rocaSpawnMin, rocaSpawnMax;
    [SerializeField] private GameObject[] rocasPrefab;
    [SerializeField] private float rocasSpawnRate;
    [SerializeField] private float jumpForce;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base.Start();
        
        player = GameObject.Find("Prota").transform;
        
        // TEST //////////////////////////
        ChangeState(BossStates.Jump);
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
        ChangeState((BossStates) azar);
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
            Vector2 randomPos = new Vector2(Random.Range(rocaSpawnMin.position.x, rocaSpawnMax.position.x), rocaSpawnMin.position.y);
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
        
        while (rolling)
        {
            rb.linearVelocity = transform.right * rollSpeed;
            yield return null;
        }
        animator.SetBool("Roll", false);
        rb.linearVelocity = Vector2.zero;
        
        yield return new WaitForSeconds(1);
        
        ChangeState(BossStates.Waiting);
    }

    IEnumerator Movement()
    {
        LookTo(pointToMove);
        animator.SetBool("Walk", true);
        
        bool walking = true;

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
        
        ChangeState(BossStates.Waiting);
    }

    IEnumerator Jump()
    {
        animator.SetBool("Jump", true);
        yield return new WaitForSeconds(0.75f);
        rb.linearVelocity = new Vector2(speed, jumpForce);
        //Cuando acabe el salto
        yield return new WaitForSeconds(2);
        animator.SetBool("Jump", false);
        yield return new WaitForSeconds(0.33f);
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
    }
}
