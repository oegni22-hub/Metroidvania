using UnityEngine;

public class DetectionArea : MonoBehaviour
{
    private EnemyController enemy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemy = transform.parent.GetComponent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            enemy.SetPlayer(col.gameObject.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            enemy.StopFollowing();
        }
    }
}
