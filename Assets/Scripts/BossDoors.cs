using UnityEngine;

public class BossDoors : MonoBehaviour
{
    private Animator animator;
    private Collider2D collider;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
        OpenDoors();
        //subscriure's al event
        StartBoss.OnCombatStart += CloseDoors;
        BossController.OnBossDefeated += OpenDoors;
    }

    void OpenDoors()
    {
        animator.SetBool("Open", true);
        collider.enabled = false;
    }
    
    void CloseDoors()
    {
        animator.SetBool("Open", false);
        collider.enabled = true;
    }
    
    //Dessubscriure's del event
    void OnDisable()
    {
        StartBoss.OnCombatStart -= CloseDoors;
        BossController.OnBossDefeated -= OpenDoors;
    }
}
