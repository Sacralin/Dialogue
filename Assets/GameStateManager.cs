using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using File = System.IO.File;
using System;

[Serializable]
public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    
    private Dictionary<string, bool> flags;

    private string flagsFilePath;
    

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            flags = LoadGame();
            DontDestroyOnLoad(gameObject);
            flagsFilePath = Application.persistentDataPath + "/flags.xml";
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetFlag(string key, bool value)
    {
        flags[key] = value;
    }
    public bool GetFlag(string key)
    {
        return flags.ContainsKey(key) && flags[key];
    }

    private Dictionary<string, bool> LoadGame()
    {
        Dictionary<string, bool> dictionary;

        if (File.Exists(flagsFilePath))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Dictionary<string, bool>));
            using (FileStream stream = new FileStream(flagsFilePath, FileMode.Open))
            {
                dictionary = serializer.Deserialize(stream) as Dictionary<string, bool>;
            }
        }
        else
        {
            dictionary = new Dictionary<string, bool>();
        }

        return dictionary;
    }

    public void SaveGame() 
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Dictionary<string, bool>));
        using (FileStream stream = new FileStream(flagsFilePath, FileMode.Create))
        {
            serializer.Serialize(stream, flags);
        }
    }
}
