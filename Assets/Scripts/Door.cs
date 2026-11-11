using UnityEngine;

public class Door : MonoBehaviour
{
    //[SerializeField] private int id;
    private Animator anim;
    [SerializeField] private Animator loQueAbra;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        
        /*for (int i = 0; i< GameManager.instance.scenarioData.DoorOpen[id] == true)
         {
            if (GameManager.instance.scenarioData.DoorOpen == true)
            {
                OpenDoor();
            }
         }*/
        
        if (GameManager.instance.scenarioData.DoorOpen == true)
        {
            OpenDoor();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            OpenDoor();
        }
    }

    void OpenDoor()
    {
        loQueAbra.SetBool("Open", true);
        anim.SetTrigger("Open");
        GameManager.instance.scenarioData.DoorOpen = true;
    }
}
