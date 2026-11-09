using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private bool key = false;
    [SerializeField] private bool door;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player" && key == true && door == true)
        {
            animator.SetBool("Open", true);
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Player" && door == false && Input.GetAxis("Vertical") > 0.5f)
        {
            key = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player" && door == true)
        {
            animator.SetBool("Open", false);
        }
    }
}
