/*
Class(es): GlobalGameSettings
Short description: Class contains JSON handling of all "game relevant user settings", like Camera Border Width and other settings. (for example achivements)

Initial author: Christian Kessner <SpiegelEiXXL>
Initial creation date: 20th AUG 2018
Initial name: GlobalGameSettings.cs
Written for: Unity Free 2 Play RTS project
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class GlobalGameSettings
{
    public Dictionary<string, string> userSettings;
    public static GlobalGameSettings gameSettingsInstance
    {
        get
        {
            if (_gameSettingsInstance == null)
            {
                _gameSettingsInstance = gameSettingsFactory();
            }
            return _gameSettingsInstance;
        }
    }
    private static GlobalGameSettings _gameSettingsInstance;
    private static GlobalGameSettings gameSettingsFactory()
    {
        Debug.Log("Instanciated GlobalGameSettings class. Directory: "+System.IO.Directory.GetCurrentDirectory());
        return new GlobalGameSettings();

    }
    public const string gameSettingsSaveFile = "userSettings.json";

    private GlobalGameSettings()
    {
        userSettings = new Dictionary<string, string>();
        
        
        if (System.IO.File.Exists(gameSettingsSaveFile))
        {
            string jsonContent = System.IO.File.ReadAllText(gameSettingsSaveFile);
            userSettings = JsonConvert.DeserializeObject<Dictionary<string,string>>(jsonContent);
            //userSettings = UnityEngine.JsonUtility.FromJson<Dictionary<string, string>>(jsonContent);
        }
    }

    public void saveJSON(bool overwrite = false)
    {
        if (System.IO.File.Exists(gameSettingsSaveFile) && !overwrite)
        {
            Debug.Log("not overwriting file. Exiting.");
            return;
        }
        else
        {
            Debug.Log("Deleted old file.");
            System.IO.File.Delete(gameSettingsSaveFile);
        }

        System.IO.File.WriteAllText(gameSettingsSaveFile, JsonConvert.SerializeObject(userSettings));

        Debug.Log("Wrote JSON to disk. File: " + gameSettingsSaveFile);

    }
    public string getValue(string key)
    {
        if (userSettings.ContainsKey(key))
            return userSettings[key];
        else
            return "";
    }
    public void setValue(string key, string value)
    {
        userSettings[key] = value;
        saveJSON(true);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
