using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject menuInicio, slotMenu;
    [SerializeField] private GameObject [] slotButtons;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IniciarPartida()
    {
        slotMenu.SetActive(true);
        menuInicio.SetActive(false);
        slotMenu.transform.GetChild(2).GetComponent<Button>().Select();

        for (int i = 0; i < slotButtons.Length; i++)
        {
            if (PlayerPrefs.HasKey("GameData" + i.ToString()))
            {
                slotButtons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Partida " + (i+1).ToString();
            }
            slotButtons[i].GetComponent<Button>().onClick.RemoveAllListeners();
            int temp = i;
            slotButtons[i].GetComponent<Button>().onClick.AddListener(delegate { SlotButton(temp); });
        }
    }
    
    public void ButtonBack()
    {
        menuInicio.SetActive(true);
        slotMenu.SetActive(false);
        menuInicio.transform.GetChild(0).GetComponent<Button>().Select();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    
    public void SlotButton(int index)
    {
        GameManager.instance.saveSlot = "GameData" + index.ToString();
        if (PlayerPrefs.HasKey("GameData" + index.ToString()))
        {
            GameManager.instance.LoadData();
            GameManager.instance.isFromLoad = true;
        }
        else
        {
            GameData inicio = new GameData();
            GameManager.instance.gameData = inicio;
        }
        SceneManager.LoadScene(GameManager.instance.gameData.Scene);
    }
}
