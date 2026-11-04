using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveStoneController : MonoBehaviour
{
    [SerializeField] private GameObject arrow;

    private void Update()
    {
        if (arrow.activeInHierarchy == true)
        {
            if (Input.GetAxis("Vertical") > 0.5f)
            {
                //GUARDAR
                Debug.Log("Guardo");
                GameManager.instance.gameData.Scene = SceneManager.GetActiveScene().buildIndex;
                GameManager.instance.gameData.SavePosition = transform.position;
                //curar
                //recargar mana
                GameManager.instance.SaveData();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            Debug.Log("entro en piedra");
            arrow.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            Debug.Log("Salgo de piedra");
            arrow.SetActive(false);
        }
    }
}
