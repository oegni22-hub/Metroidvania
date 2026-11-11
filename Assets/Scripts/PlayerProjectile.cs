using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] private float duration;
    private Animator animator;
    [SerializeField] private float explosionDuration;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * speed;
        animator = GetComponent<Animator>();
        Destroy(gameObject, duration);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log(col.gameObject.name);
        if (col.gameObject.tag == "Enemy")
        {
            //auch
            col.gameObject.GetComponent<EnemyController>().TakeDamage(damage);
            animator.SetTrigger("Collision");
            rb.linearVelocity = Vector3.zero;
            Destroy(gameObject, explosionDuration);
        }
        else
        {
            animator.SetTrigger("Collision");
            rb.linearVelocity = Vector3.zero;
            Destroy(gameObject, explosionDuration);
        }
    }
}
