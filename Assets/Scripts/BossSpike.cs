using UnityEngine;

public class BossSpike : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float damage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * -speed;
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion rot = Quaternion.FromToRotation(transform.right * -1, rb.linearVelocity.normalized);
        
        //transform.eulerAngles = rot.eulerAngles;
        
        transform.rotation = transform.rotation * rot;
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
            Destroy(gameObject);
        }
        Destroy(gameObject);
    }
}
