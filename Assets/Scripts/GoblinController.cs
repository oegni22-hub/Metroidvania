using UnityEngine;

public class GoblinController : EnemyController
{
    [SerializeField] private GameObject lanzaPrefab;
    [SerializeField] private Transform lanzaSpawnPoint;
    
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

    public void LanzarLanza()
    {
        Instantiate(lanzaPrefab, lanzaSpawnPoint.position, lanzaSpawnPoint.rotation);
    }
}
