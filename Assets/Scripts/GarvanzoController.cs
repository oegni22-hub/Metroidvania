using System.Collections;
using UnityEngine;

public class GarvanzoController : EnemyController
{
    [SerializeField] private int comboAmount;
    private int currentCombo;

    [SerializeField] private GameObject escupitajo;
    [SerializeField] private Transform escupitajoSpawn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base.Start();
        currentCombo = comboAmount;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    protected override void Attack()
    {
        if (currentCombo == comboAmount)
        {
            StartCoroutine(MultippleAttack());
        }
    }

    IEnumerator MultippleAttack()
    {
        currentCombo = 0;
        while (currentCombo < comboAmount)
        {
            currentCombo++;
            animator.SetTrigger("Attack");
            yield return new WaitForSeconds(0.2f);
            Instantiate(escupitajo, escupitajoSpawn.position, transform.rotation);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
