using System.Collections;
using Mono.Cecil;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float inmunityTime;
    [SerializeField] private AnimationCurve inmunityAnim;
    
    [Header("Progectile")]
    [SerializeField] private GameObject fire;
    [SerializeField] private float fireVelocity;
    [SerializeField] private float fireExpence;
    
    private int jumpCount;
    private int comboCount;
    private bool canMove = true;

    private Rigidbody2D rb;
    private Animator animator;

    private LevelManager lm;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator =transform.GetChild(0).GetComponent<Animator>();
        lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove)
        {
            return;
        }
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
        
        //Hard Attack
        if (Input.GetButtonDown("HardAttack") && isGrounded)
        {
            animator.SetTrigger("HardAttack");
            canMove = false;
            rb.linearVelocityX = 0;
        }
        
        //Magic Attack
        if (Input.GetButtonDown("Magic") && isGrounded && GameManager.instance.gameData.Mana >= fireExpence)
        {
            animator.SetTrigger("Magic");
            Instantiate(fire, new Vector3(transform.position.x, transform.position.y + 0.7f, transform.position.z), transform.rotation);
            GameManager.instance.gameData.Mana -= fireExpence;
            lm.UpdateManaBar();
            canMove = false;
            rb.linearVelocityX = 0;
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
        canMove = true;
    }
    
    public void FinishHardAttack()
    {
        canMove = true;
    }

    public void KnockBack(Vector2 direction, float knockBackForce)
    {
        StartCoroutine(KnockBackRoutine(direction, knockBackForce));
    }

    IEnumerator KnockBackRoutine(Vector2 direction, float knockBackForce)
    {
        canMove = false;
        rb.AddForce(direction * knockBackForce);
        yield return new WaitForSeconds(0.5f);
        canMove = true;
    }

    IEnumerator TemporalInmunity()
    {
        SpriteRenderer spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        gameObject.layer = 10;
        //yield return new WaitForSeconds(inmunityTime);
        float t = 0;
        Color tempC = spriteRenderer.color;
        while (t < inmunityTime)
        {
            t += Time.deltaTime;
            float value = inmunityAnim.Evaluate(t);
            tempC.a = value;
            spriteRenderer.color = tempC;
            yield return null;
        }
        gameObject.layer = 9;
        tempC.a = 1;
        spriteRenderer.color = tempC;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //Damage Enemy
        if (col.tag == "Enemy")
        {
            col.gameObject.GetComponent<EnemyController>().TakeDamage(GameManager.instance.gameData.Damage);
        }
    }

    public void TakeDamage(float _damage)
    {
        GameManager.instance.gameData.Life -= _damage;
        lm.UpdateLifeBar();
        if (GameManager.instance.gameData.Life <= 0 && canMove)
        {
            //DEATH
            animator.SetTrigger("Death");
            canMove = false;
            rb.bodyType = RigidbodyType2D.Kinematic;
            GetComponent<Collider2D>().enabled = false;
            rb.linearVelocity  = Vector2.zero;
        }
        else if (GameManager.instance.gameData.Life > 0)
        {
            //HIT
            animator.SetTrigger("Hit");
            comboCount = 0;
            canMove = true;
            animator.SetInteger("Combo", comboCount);
            StartCoroutine(TemporalInmunity());
        }
    }
    
}
