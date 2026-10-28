using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] protected float distanceToAttack;
    [SerializeField] protected float damage;
    [SerializeField] protected float life;
    [SerializeField] protected float attackRate;

    protected bool isDead;
    protected bool following;
    protected float timePass;
    protected bool attacking;
    
    protected Transform player;
    protected Rigidbody2D rb;
    protected Animator animator;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    // Update is called once per frame
    protected void Update()
    {
        if (isDead)
        {
            return;
        }
        timePass += Time.deltaTime;
        //Moviment
        if (following)
        {
            animator.SetBool("Run", true);
            Vector3 direction = player.position - transform.position;
            if (direction.x < 0) //cap a l'esquerra
            {
                rb.linearVelocityX = -speed;
                transform.eulerAngles = Vector3.zero;
            }
            else //dreta
            {
                rb.linearVelocityX = speed;
                transform.eulerAngles = new Vector3(0, 180, 0);
            }

            if (Mathf.Abs(direction.x) <= distanceToAttack)
            {
                //ataca
                rb.linearVelocityX = 0;
                animator.SetBool("Run", false);
                //Attack();
                attacking = true;
            }
            else
            {
                attacking = false;
            }
        }

        if (attacking)
        {
            if (timePass >= attackRate)
            {
                Attack();
                timePass = 0;
            }
        }
    }

    protected void Attack()
    {
        animator.SetTrigger("Attack");
    }

    public void TakeDamage(float _damage)
    {
        life -= _damage;
        if (life <= 0 && isDead == false)
        {
            animator.SetTrigger("Death");
            rb.bodyType = RigidbodyType2D.Kinematic;
            GetComponent<Collider2D>().enabled = false;
            isDead = true;
            //Destroy(gameObject, 1);
        }
        else
        {
            animator.SetTrigger("Hit");
        }
    }

    public void SetPlayer(Transform _player)
    {
        player = _player;
        following = true;
    }

    public void StopFollowing()
    {
        following = false;
    }

    protected void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
        }
    }
}
