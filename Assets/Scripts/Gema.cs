using System;
using UnityEngine;

public class Gema : MonoBehaviour
{
    [SerializeField] private string id;
    public string message;
    [SerializeField] private ParticleSystem particles;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        switch (id)
        {
            case "DobleJump":
                if (GameManager.instance.gameData.MaxJumps >1) { Destroy(gameObject); }
                break;
            case "Dash":

                break;
            case "MagicAtack":
                
                break;
            default:
                
                break;
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
            switch (id)
            {
                case "DobleJump":
                    GameManager.instance.gameData.MaxJumps = 2;
                    particles.Play();
                    Time.timeScale = 0;
                    break;
                case "Dash":
                    Debug.Log("Dash");
                    break;
                case "Magic Atack":
                    Debug.Log("Magic Atack");
                    break;
                default:
                    
                    break;
            }
            
        }
    }
}
