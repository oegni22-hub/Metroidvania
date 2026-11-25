using System;
using System.Xml.Schema;
using UnityEngine;

public class StartBoss : MonoBehaviour
{
    [SerializeField] private GameObject[] bossDoors;
    //Action = Evento / static para acceder desde cualquiera
    public static Action OnCombatStart;

    void Start()
    {
        if (GameManager.instance.scenarioData.Boss1Defeated == true)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            // ?.Invoke para assegurar-se de que no tiene error
            OnCombatStart?.Invoke();
            //Cierra puertas
            //Activa el boss
            Destroy(gameObject);
        }
    }
    
}
