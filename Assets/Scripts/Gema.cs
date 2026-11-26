using System;
using UnityEngine;

public class Gema : MonoBehaviour
{
    [SerializeField] private string id;
    public string message;
    [SerializeField] private ParticleSystem particles;
    private LevelManager lm;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        switch (id)
        {
            case "DobleJump":
                if (GameManager.instance.gameData.MaxJumps > 1) { Destroy(gameObject); }
                break;
            case "Dash":

                break;
            case "MagicAtack":
                
                break;
            case "AugMaxLife":
                if (GameManager.instance.gameData.MaxLife > 50) { Destroy(gameObject); }
                break;
            case "AugMaxMana":
                if (GameManager.instance.gameData.MaxMana > 30) { Destroy(gameObject); }
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
                case "AugMaxLife":
                    GameManager.instance.gameData.MaxLife = 100;
                    particles.Play();
                    Time.timeScale = 0;
                    GameManager.instance.gameData.Life = GameManager.instance.gameData.MaxLife;
                    lm.UpdateLifeBar();
                    break;
                case "AugMaxMana":
                    GameManager.instance.gameData.MaxMana = 50;
                    particles.Play();
                    Time.timeScale = 0;
                    GameManager.instance.gameData.Mana = GameManager.instance.gameData.MaxMana;
                    lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
                    lm.UpdateManaBar();
                    break;
                default:
                    
                    break;
            }
            
        }
    }
}
