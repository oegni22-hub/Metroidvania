using UnityEngine;

public class ArbolitoController : EnemyController
{
    [SerializeField] private float distanceToAttack2;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        if (Mathf.Abs(direction.x) <= distanceToAttack2)
        {
            animator.SetBool("Attack2", true);
        }
        else
        {
            animator.SetBool("Attack2", false);
        }
    }
}
