using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class JsonReadWriteSystem : MonoBehaviour
{
    [SerializeField] private string fileName;

    //[SerializeField]
    public PlayerData playerData;

    public static JsonReadWriteSystem INSTANCE;

   

    private void Awake()
    {
        if (INSTANCE)
        {
            Destroy(gameObject);
            return;
        }

        INSTANCE = this;

        //DontDestroyOnLoad(gameObject);
        playerData = new PlayerData();
        Load();

    }


    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.I))
        {
            ResetData();
        }
#endif
    }

    private void WriteToFile(string filename, string jsonData)
    {
        string path = GetFilePath(filename);
        FileStream fileStream = new FileStream(path, FileMode.Create);
        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(jsonData);
        }
    }

    private string ReadFromFile(string filename)
    {
        string path = GetFilePath(filename);

        if (!File.Exists(path))
        {
            Debug.Log("File does not exist!");
            return "";
        }

        using (StreamReader reader = new StreamReader(path))
        {
            return reader.ReadToEnd();
        }
    }

    private string GetFilePath(string fileName)
    {
        return Application.persistentDataPath + "/" + fileName;
    }

    public void Save()
    {
#if UNITY_WEBGL

        // Se estiver no WebGL, use PlayerPrefs para salvar um �nico valor (exemplo: maxScore)
        PlayerPrefs.SetInt("MaxScore", playerData.maxScore);
        PlayerPrefs.Save();

#else
            // Se n�o estiver no WebGL, salve os dados como JSON em um arquivo
            string json = JsonUtility.ToJson(playerData);
            WriteToFile(fileName, json);
#endif


    }

    public void Load()
    {
#if UNITY_WEBGL
        if (PlayerPrefs.HasKey("MaxScore"))
        {
            playerData.maxScore = PlayerPrefs.GetInt("MaxScore");
        }
#else
        Debug.Log("Load done");
        string json = ReadFromFile(fileName);
        Debug.Log(fileName + " path " + GetFilePath(fileName));

        JsonUtility.FromJsonOverwrite(json, playerData);
#endif


    }

    public void ResetData()
    {
        playerData.MaxScore = 0;
        playerData.soundOn = true;
        Save();
    }

    
}