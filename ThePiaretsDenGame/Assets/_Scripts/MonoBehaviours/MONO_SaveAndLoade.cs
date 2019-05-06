using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class MONO_SaveAndLoade : MonoBehaviour {

    public string filename = "/SecretOfTheOldShip.dat";

    public int sparedLevel;


    /// <summary>
    /// save data to a file 
    /// </summary>
    public void Save(int lastScene)
    {
        sparedLevel         = lastScene;
        BinaryFormatter bf  = new BinaryFormatter();
        FileStream file     = File.Open(Application.persistentDataPath + filename, FileMode.OpenOrCreate);

        SaveDate data   = new SaveDate();
        data.level      = lastScene;

        bf.Serialize(file, data);
        file.Close();

    }

    public int LoadSave()
    {
        if (File.Exists(Application.persistentDataPath + filename))
        {
            BinaryFormatter bf  = new BinaryFormatter();
            FileStream file     = File.Open(Application.persistentDataPath + filename, FileMode.Open);
            SaveDate data       = (SaveDate)bf.Deserialize(file);
            file.Close();

            return data.level;
        }
        Debug.LogError("sa#%an babian r%¤öv");
        return 1;
    }


    [System.Serializable]
    class SaveDate
    {
        public int level;

    }
}
