using UnityEngine;

public class DemonioController : EnemyController
{
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
            Destroy(gameObject, 1.5f);
        }
    }
}
