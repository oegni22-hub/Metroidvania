using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveStoneController : MonoBehaviour
{
    [SerializeField] private GameObject arrow;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private SpriteRenderer brilloPiedra;
    [SerializeField] private float animSpeed;
    private LevelManager lm;

    private void Start()
    {
        StartCoroutine(BrilloPiedra());
        lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }
    
    private void Update()
    {
        if (arrow.activeInHierarchy == true)
        {
            if (Input.GetAxis("Vertical") > 0.5f)
            {
                //GUARDAR
                GameManager.instance.gameData.Scene = SceneManager.GetActiveScene().buildIndex;
                GameManager.instance.gameData.SavePosition = transform.position;
                //curar
                GameManager.instance.gameData.Life = GameManager.instance.gameData.MaxLife;
                lm.UpdateLifeBar();
                //recargar mana
                GameManager.instance.gameData.Mana = GameManager.instance.gameData.MaxMana;
                lm.UpdateManaBar();
                
                GameManager.instance.SaveData();
                particles.Play();
                arrow.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            arrow.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            arrow.SetActive(false);
        }
    }

    IEnumerator BrilloPiedra()
    {
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * animSpeed;
            float alpha = Mathf.Lerp(0, 1, t);
            Color tempColor = brilloPiedra.color;
            tempColor.a = alpha;
            brilloPiedra.color = tempColor;
            yield return null;
        }
        while (t > 0)
        {
            t -= Time.deltaTime * animSpeed;
            float alpha = Mathf.Lerp(0, 1, t);
            Color tempColor = brilloPiedra.color;
            tempColor.a = alpha;
            brilloPiedra.color = tempColor;
            yield return null;
        }
        
        StartCoroutine(BrilloPiedra());
    }
}
