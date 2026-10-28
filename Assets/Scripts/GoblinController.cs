using UnityEngine;

public class GoblinController : EnemyController
{
    [SerializeField] private GameObject flecha;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        if (isDead)
        {
            Destroy(gameObject, 1);
        }
    }

}
