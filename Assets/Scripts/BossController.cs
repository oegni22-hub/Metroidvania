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
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base.Start();
        
        player = GameObject.Find("Prota").transform;
        
        // TEST //////////////////////////
        ChangeState(BossStates.Moving);
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
        yield return new WaitForSeconds(1);
        ChangeState(BossStates.Waiting);
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
            rb.velocity = transform.right * speed;

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

    IEnumerator SpikeAttack()
    {
        animator.SetBool("Spike", true);
        yield return null;
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
