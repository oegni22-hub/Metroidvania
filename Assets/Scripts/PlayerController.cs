using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    private int JumpCount;

    private Rigidbody2D rb;
    
    private Animator animator;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator =transform.GetChild(0).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        rb.linearVelocityX = speed * inputX;
        //Rotation
        if (inputX > 0)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if (inputX < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        
        
        //Animation
        if (inputX != 0)
        {
            animator.SetBool("Run", true);
        }
        else
        {
            animator.SetBool("Run", false);
        }
        
        //Jump
        Collider2D [] temp = Physics2D.OverlapCircleAll(transform.position, 0.01f);
        bool isGrounded = false;
        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i].tag == "Ground")
            {
                isGrounded = true;
            }
        }

        if (isGrounded)
        {
            animator.SetBool("Jump", false);
            animator.SetBool("Falling", false);
            JumpCount = 0;
        }
        else
        {
            if (JumpCount == 0)
            {
                JumpCount += 1;
            }
            animator.SetBool("Falling", true);
        }
        
        if (Input.GetButtonDown("Jump") && JumpCount < GameManager.instance.gameData.MaxJumps)
        {
            rb.linearVelocity = new Vector2 (rb.linearVelocityX, jumpForce);
            animator.SetBool("Jump", true);
            JumpCount++;
        }
        
    }
}
