using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class JsonReadWriteSystem : MonoBehaviour
{
    [SerializeField] private string fileName;

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

        Load();

    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ResetData();
        }

       

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
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
        string json = JsonUtility.ToJson(playerData);

        WriteToFile(fileName, json);

    }

    public void Load()
    {
        Debug.Log("Load done");
        string json = ReadFromFile(fileName);
        Debug.Log(fileName + " path " + GetFilePath(fileName));
        JsonUtility.FromJsonOverwrite(json, playerData);

    }

    public void ResetData()
    {
        playerData.maxScore = 0;
        
        Save();
    }

    
}