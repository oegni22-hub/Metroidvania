using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameData gameData;
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
    }

    public void LoadData()
    {
        string data = PlayerPrefs.GetString(saveSlot);
        gameData = JsonUtility.FromJson<GameData>(data);
    }
    

}
