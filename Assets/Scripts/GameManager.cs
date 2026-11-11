using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameData gameData;
    public ScenarioData scenarioData;
    public int doorToGo;
    public string saveSlot;
    public bool isFromLoad;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        { 
            Destroy(gameObject);
        }
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SaveData()
    {
        //JSON
        string data = JsonUtility.ToJson(gameData);
        PlayerPrefs.SetString(saveSlot, data);
        
        string data2 = JsonUtility.ToJson(scenarioData);
        PlayerPrefs.SetString(saveSlot + "scenario", data2);
    }

    public void LoadData()
    {
        string data = PlayerPrefs.GetString(saveSlot);
        gameData = JsonUtility.FromJson<GameData>(data);
        
        string data2 = PlayerPrefs.GetString(saveSlot + "scenario");
        scenarioData = JsonUtility.FromJson<ScenarioData>(data2);
    }
    

}
