using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class MONO_SaveAndLoad : MonoBehaviour
{

    public string filename = "/SecretOfTheOldShip.dat";

    private void Awake()
    {
        
    }


    /// <summary>
    /// save data to a file 
    /// </summary>
    public void Save(int lastScene)
    {
  

           SaveDate2 data = new SaveDate2();
        GameObject test = GameObject.Find("VariableFlowchartAct1");
        GameObject tes2t = GameObject.Find("VariableFlowchartAct2");
        Fungus.Flowchart[] tats = new Fungus.Flowchart[2];//FindObjectsOfType(typeof(Fungus.Flowchart)) as Fungus.Flowchart[];

        tats[0] = test.GetComponent<Fungus.Flowchart>();
        tats[1] = tes2t.GetComponent<Fungus.Flowchart>();
        data.flowshart = new string[tats.Length];
        data.data      = new SaveDate[tats.Length];

        for (int i = 0; i < tats.Length; i++)
        {
            data.flowshart[i] = tats[i].GetName();
            data.data[i] = new SaveDate();
            if (tats[i].GetVariableNames() == null)
            {
                data.data[i].keys = new string[1];
                data.noVariables = true;
            }
            else
            {
                
                data.data[i].keys = tats[i].GetVariableNames();
         
            }
            
      
            int length = tats[i].GetVariableNames().Length;
            data.data[i].values = new SaveDate3[length];
            data.data[i].typeS = new Type[length];
            for (int v = 0; v < length; v++)
            {
                Fungus.Variable temp = tats[i].GetVariable(data.data[i].keys[i]);

                data.data[i].values[v] = new SaveDate3();
                 Type temp2 = temp.GetType();
                data.data[i].typeS[v] = temp2;
                if(temp2 == typeof(int))
                {
                    data.data[i].values[v].innt = tats[i].GetIntegerVariable(data.data[i].keys[i]);
                    data.data[i].values[v].isInt = true;
                }
                else
                {
                    data.data[i].values[v].bol = tats[i].GetBooleanVariable(data.data[i].keys[i]);
                }
            }
        }
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + filename, FileMode.OpenOrCreate);

        bf.Serialize(file, data);
        file.Close();
        Debug.Log("SAVED");
    }

    public void LoadSave(bool load)
    {
        if (load)
        {

        
        if (File.Exists(Application.persistentDataPath + filename))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + filename, FileMode.Open);
            SaveDate2 data = (SaveDate2)bf.Deserialize(file);
            file.Close();



            for (int i = 0; i < data.flowshart.Length; i++)
            {
                GameObject test = GameObject.Find(data.flowshart[i]);
                if (test != null)
                {
                    Fungus.Flowchart flow = test.GetComponent<Fungus.Flowchart>();
                    if (flow != null)
                    {
                        Debug.Log(flow.GetName());
       

                        for (int v = 0; v < flow.GetVariableNames().Length; v++)
                        {

                            if (data.data[i].values[i].isInt)
                            {
                                flow.SetIntegerVariable(data.data[i].keys[i], data.data[i].values[i].innt);
                            }
                            else
                            {
                                flow.SetBooleanVariable(data.data[i].keys[i], data.data[i].values[i].bol);
                            }


                        }
                    }
                }
            }

        }
        Debug.Log("LOADED");
        }
    }

    [System.Serializable]
    class SaveDate2
    {
        public string[] flowshart = new string[1];
        public SaveDate[] data = new SaveDate[1];
        public bool noVariables = false;
    }
    [System.Serializable]
    class SaveDate
    {

        public string[] keys = new string[1];
        public Type[] typeS = new Type[1];
        public SaveDate3[] values = new SaveDate3[1];

    }
    [System.Serializable]
    class SaveDate3
    {
        public bool isInt = false;

        public int innt = 1;
        public bool bol = false;

    }
}
