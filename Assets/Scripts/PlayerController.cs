using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    private int jumpCount;
    private int comboCount;

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
        //Move
        float inputX = Input.GetAxis("Horizontal");
        if (comboCount > 0)
        {
            inputX = 0;
        }
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
            jumpCount = 0;
        }
        else
        {
            if (jumpCount == 0)
            {
                jumpCount += 1;
            }
            animator.SetBool("Falling", true);
        }
        
        if (Input.GetButtonDown("Jump") && jumpCount < GameManager.instance.gameData.MaxJumps && comboCount == 0)
        {
            rb.linearVelocity = new Vector2 (rb.linearVelocityX, jumpForce);
            animator.SetBool("Jump", true);
            jumpCount++;
        }

        //Attack
        if (Input.GetButtonDown("Attack") && isGrounded)
        {
            comboCount  = Mathf.Clamp(comboCount+1, 0, 2);
            animator.SetInteger("Combo", comboCount);
        }
        
    }

    public void FinishAttack1()
    {
        if (comboCount <= 1)
        {
            comboCount = 0;
            animator.SetInteger("Combo", comboCount);
        }
    }

    public void FinishAttack2()
    {
        comboCount = 0;
        animator.SetInteger("Combo", comboCount);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Enemy")
        {
            //llamar a la funcion enemigo para recivir danyo
        }
    }
}
