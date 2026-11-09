using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Image lifeBar;
    [SerializeField] private Image manaBar;
    [SerializeField] private TextMeshProUGUI pocionesText;
    
    [SerializeField] private Transform [] spawnPoint;
    [SerializeField] private GameObject gemsPanel, gameOverPanel, winPanel;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateLifeBar();
        UpdateManaBar();
        UpdatePocionesText();
        
        Transform player = GameObject.FindWithTag("Player").transform;

        if (GameManager.instance.isFromLoad)
        {
            player.position = GameManager.instance.gameData.SavePosition;
            GameManager.instance.isFromLoad = false;
        }
        else
        {
            player.position = spawnPoint[GameManager.instance.doorToGo].position;
            player.rotation = spawnPoint[GameManager.instance.doorToGo].rotation;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateLifeBar()
    {
        lifeBar.fillAmount = GameManager.instance.gameData.Life / GameManager.instance.gameData.MaxLife;
    }

    public void UpdateManaBar()
    {
        manaBar.fillAmount = GameManager.instance.gameData.Mana / GameManager.instance.gameData.MaxMana;
    }
    
    public void UpdatePocionesText()
    {
        pocionesText.text = "X" + GameManager.instance.gameData.PotionsCount.ToString();
    }

    public void ShowGemMessage(string message)
    {
        gemsPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = message;
        gemsPanel.transform.GetChild(1).GetComponent<Button>().Select();
        gemsPanel.SetActive(true);
    }

    public void ContinueButton()
    {
        Time.timeScale = 1;
        gemsPanel.SetActive(false);
    }
}
