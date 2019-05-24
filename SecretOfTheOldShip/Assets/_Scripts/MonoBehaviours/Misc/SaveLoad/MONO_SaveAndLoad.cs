﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class MONO_SaveAndLoad : MonoBehaviour
{
    public enum fungusVariableData { STRING, INT, BOOL, FLOAT };

    public string filename = "/SecretOfTheOldShip.dat";

    [Tooltip("The name of the flowcharts that holds the variable information, " +
             "everything exept the number that tells withc act its for example:" +
             " act 1 (outsie is named VariableFlowchartAct1). this is to make it" +
             " esayer to lokate them then saving ")]
    public string baseNameOfVariableCharts = "VariableFlowchartAct";
    public int lastLevel;


    [SerializeField]
    private SaveData data = null;
    

    //Only used to Keep track of saved flowcharts;
    // string: name of flowchart, int: index in data varaialbe
    [SerializeField]
    Dictionary<String, int> hasBenSaved = new Dictionary<string, int>();

    public void Start()
    {
        loadData();
        UppdateSavedReckord();
    }

    /// <summary>
    ///  Save 
    /// </summary>
    /// <param name="loadAllredySavedData"> TRUE: loads allready saved data and combinds whit new saved data to prevent lost of data ( normal save)
    ///                                     FALSE: Writes over old data ( then startin a new game )</param>
    public void Save(bool loadAllredySavedData)
    {

        /*Creats data variabel to be saved
         *Gets savaed data if wated 
         */
        SaveData dataToSave = new SaveData();

        Fungus.Flowchart[] flowChartsInScene = FindObjectsOfType(typeof(Fungus.Flowchart)) as Fungus.Flowchart[];
       
        Fungus.Flowchart[] variableFlowCharts = getVariableFlowCharts(flowChartsInScene);

        if (loadAllredySavedData && loadData())
        {
            dataToSave.flowChartVariableData = getAllVariableFlowchartToSave(variableFlowCharts);
        }
        else
        {
            List<variableData> completeDataTosave = new List<variableData>();
            foreach (Fungus.Flowchart variabelChart in variableFlowCharts)
            {
                variableData variabledata = new variableData(variabelChart);
                completeDataTosave.Add(variabledata);
            }
            dataToSave.flowChartVariableData = completeDataTosave.ToArray();
        }

     //===============================
     // saves the data
     //===============================
        BinaryFormatter bf  = new BinaryFormatter();
        FileStream file     = File.Open(Application.persistentDataPath + filename, FileMode.OpenOrCreate);

        bf.Serialize(file, dataToSave);
        file.Close();
        data = dataToSave;
        UppdateSavedReckord();

    }
   
    /// <summary>
    /// Loads data from save file if it exsist
    /// </summary>
    /// <param name="applayLodedData"> TRUE: emidiet applays the loaded infomration 
    ///                                FALSE: only loade the values</param>
    public void handLoad(bool applayLodedData)
    {

        if (loadData())
        {
            UppdateSavedReckord();
            if (applayLodedData)
            {
                UppdateFlowcharts();
            }

        }
    }


    /// <summary>
    /// Uppdates all flowcharts in 
    /// current sceen whit walus form 
    /// loade data
    /// </summary>
    public void UppdateFlowcharts()
    {
        Fungus.Flowchart[] flowChartsInScene = FindObjectsOfType(typeof(Fungus.Flowchart)) as Fungus.Flowchart[];

        foreach(Fungus.Flowchart chart in flowChartsInScene)
        {
            int index;
            if(hasBenSaved.TryGetValue(chart.name, out index))
            {
                foreach(valueData value in data.flowChartVariableData[index].variabelValues)
                {
                    switch (value.variableType)
                    {
                        case fungusVariableData.STRING:
                            chart.SetStringVariable(value.valueKey, value.getStringValue);
                            break;
                        case fungusVariableData.INT:
                            chart.SetIntegerVariable(value.valueKey, value.getIntValue);
                            break;
                        case fungusVariableData.BOOL:
                            chart.SetBooleanVariable(value.valueKey, value.getBoolValue);
                            break;
                        case fungusVariableData.FLOAT:
                            chart.SetFloatVariable(value.valueKey, value.getFlowtValue);
                            break;
                        default:
                            break;
                    }
                }
            }

        }

    }



//==========================================================================================
// HELP FUNKTIONS
//==========================================================================================

    /// <summary>
    /// Uppdates the dictionary that has tarack of
    /// what flowchart that has ben saved
    /// </summary>
    private void UppdateSavedReckord()
    {
        //uppdate the saved dictonary
        hasBenSaved.Clear();
        for (int i = 0; i < data.flowChartVariableData.Length; i++)
        {
            hasBenSaved.Add(data.flowChartVariableData[i].flowChartName, i);
        }
    }
    /// <summary>
    /// Uppdates the dictionary that has tarack of
    /// what flowchart that has ben saved
    /// </summary>
    public void UppdateSavedReckordEditor()
    {

        //uppdate the saved dictonary
        loadData();
        hasBenSaved.Clear();
        for (int i = 0; i < data.flowChartVariableData.Length; i++)
        {
            hasBenSaved.Add(data.flowChartVariableData[i].flowChartName, i);
        }
    }

    //==========================================================================================
    // SAVE HELPER
    //==========================================================================================

    /// <summary>
    ///                                 Gets all the variabel flowcharts in the curent scene
    /// </summary>
    /// <param name="flowChartsInScene"> All the flowcharts in curent scene</param>
    /// <returns>                        variable flowcharts in current scene </returns>
    private Fungus.Flowchart[] getVariableFlowCharts(Fungus.Flowchart[] flowChartsInScene)
    {


        Fungus.Flowchart[] variableFlowCharts;                  //Return variable, will hold all the variable flocharts in current scene
        int index = 0;                // used then sparing to return variable;
        int numberOfVariableFlowCharts = 0;                // to holde the number of variabel flowcharts in the scene
        List<int> indexOfvariableFlowCharts = new List<int>();  // index of variable flowcharts in flowChartsInScene


        // Lokate all variable flowcharts in flowChartsInScene
        for (int i = 0; i < flowChartsInScene.Length; i++)
        {
            string name = flowChartsInScene[i].GetName();
            if (name.Remove(name.Length - 1) == baseNameOfVariableCharts)
            {
                numberOfVariableFlowCharts++;
                indexOfvariableFlowCharts.Add(i);
            }
        }

        variableFlowCharts = new Fungus.Flowchart[numberOfVariableFlowCharts];

        // Gets the variable flowcharts from flowChartsInScene
        foreach (int flowChartIndex in indexOfvariableFlowCharts)
        {
            variableFlowCharts[index] = flowChartsInScene[flowChartIndex];
            index++;
        }



        return variableFlowCharts;
    }


    /// <summary>
    /// Gets a array of variable data that is comprised of
    /// all the variable data from all flowcharts in the current scene
    /// as well as all the variable data from the flowcharts that is not in
    /// the scene but is saved since earlyer.
    /// </summary>
    /// <param name="variableFlowCharts"> the flowcharts that is in
    ///                                   the current scene</param>
    /// <returns> array of all flowcharts to save</returns>
    private variableData[] getAllVariableFlowchartToSave(Fungus.Flowchart[] variableFlowCharts)
    {   
            

         //========================================================
         // Gets all flowcharts that arent in the new scene but 
         // that are in the saved data, to make sure that nothing 
         // is lost/ writen over
         //========================================================            
            Dictionary<String, int> flowchartsFromCurrentScene = new Dictionary<string, int>();
            for (int i = 0; i < variableFlowCharts.Length; i++)
            {
                flowchartsFromCurrentScene.Add(variableFlowCharts[i].GetName(),i );
            }
           
            List<int>              indexToCepFromOld = new List<int>();
            for (int i = 0; i < data.flowChartVariableData.Length; i++)
            {

                int  indexInData;
                string serceName = data.flowChartVariableData[i].flowChartName;
                bool isNotInThisScene = !flowchartsFromCurrentScene.TryGetValue(serceName, out indexInData);
                if (isNotInThisScene)
                {
                    hasBenSaved.TryGetValue(serceName, out indexInData);// gets actually index
                    indexToCepFromOld.Add(indexInData);
                }
            }

        //========================================================
        // ACombinds the new flowcharts whit the old
        //========================================================
        List<variableData> completeDataTosave = new List<variableData>();
            foreach(int index in indexToCepFromOld)
            {
                completeDataTosave.Add(data.flowChartVariableData[index]);
            }
        foreach (Fungus.Flowchart flowchart in variableFlowCharts)
            {
                variableData variabledata = new variableData(flowchart);
                completeDataTosave.Add(variabledata);
            }


        return completeDataTosave.ToArray();
    }

    //==========================================================================================
    // LOAD HELPER
    //==========================================================================================

    /// <summary>
    /// Loades data from the save file
    /// </summary>
    /// <returns>TRUE: it existed saved data and it was loaded
    ///          FALSE: Dident exist anny saved data, nothing was loaded</returns>
    private bool loadData()
    {
        if (File.Exists(Application.persistentDataPath + filename))
        {
           
            BinaryFormatter bf  = new BinaryFormatter();
            FileStream file     = File.Open(Application.persistentDataPath + filename, FileMode.Open);
            data                = (SaveData)bf.Deserialize(file);
            file.Close();
            return true;
        }

        return false;
    }

 
    /// <summary>
    /// Contins the ovar information thant need
    /// to be saved ( i.e like flowcharts and curent scene scene)
    /// </summary>
    [System.Serializable]
    class SaveData
    {
        public variableData[] flowChartVariableData = new variableData[1];
    }
    /// <summary>
    /// Contains a list all the variables
    /// (and the name) of flowchart, variable 
    /// data can be instantiated from a nother
    /// variable data (then it gets cept on
    /// a save) and from a flowchart)
    /// </summary>
    [System.Serializable]
    class variableData
    {
        public string flowChartName;

        public valueData[] variabelValues = new valueData[1];

        public variableData(Fungus.Flowchart flowchartToSpare)
        {
            flowChartName                     = flowchartToSpare.GetName();
            Fungus.Variable[] fungusVariables = flowchartToSpare.Variables.ToArray();

            variabelValues = new valueData[fungusVariables.Length];
            for (int i = 0; i < fungusVariables.Length; i++)
            {
                variabelValues[i] = new valueData();
                variabelValues[i].dataValue = fungusVariables[i];
            }
        }
        public variableData(variableData valuesFrom)
        {
            flowChartName = valuesFrom.flowChartName;
            variabelValues = valuesFrom.variabelValues;
        }


    }
    /// <summary>
    /// Contains variable data from 
    /// a single fungus variable, to 
    /// save it must be broken donw
    /// to base values like strings,
    /// float, int and bool.
    /// </summary>
    [System.Serializable]
    class valueData
    {
        public fungusVariableData variableType;
        public string valueKey;
        private Type valueTyp;

        private int     valueInt;
        private bool    valueBool;
        private float   valueFloat;
        private string  valueString;

        public string getStringValue
        {
            get
            {
                return valueString;
            }
        }
        public int    getIntValue
        {
            get
            {
                return valueInt;
            }
        }
        public bool   getBoolValue
        {
            get
            {
                return valueBool;
            }
        }
        public float  getFlowtValue
        {
            get
            {
                return valueFloat;
            }
        }

        public Fungus.Variable dataValue
        {
            get
            {

                switch (variableType)
                {
                    case fungusVariableData.STRING:
                        Fungus.StringVariable valueS = new Fungus.StringVariable();
                        valueS.Key                   = valueKey;
                        valueS.Value                 = valueString;
                        return valueS;
                    case fungusVariableData.INT:
                        Fungus.IntegerVariable valueI = new Fungus.IntegerVariable();
                        valueI.Key                    = valueKey;
                        valueI.Value                  = valueInt;
                        return valueI;
                    case fungusVariableData.BOOL:
                        Fungus.BooleanVariable valueB = new Fungus.BooleanVariable();
                        valueB.Key                    = valueKey;
                        valueB.Value                  = valueBool;
                        return valueB;
                    case fungusVariableData.FLOAT:
                        Fungus.FloatVariable valueF = new Fungus.FloatVariable();
                        valueF.Key                  = valueKey;
                        valueF.Value                = valueFloat;
                        return valueF;
                    default:
                        Debug.Log("ERROR then geting valueData in MONO_SaveAndLoad: this data has no reqognised value");
                        return new Fungus.BooleanVariable(); ;
                }
            }

            set
            {
                Type newType    = value.GetType();
                string newKey   = value.Key;

                if (newType == typeof(Fungus.IntegerVariable))
                {
                    Fungus.IntegerVariable temp = (Fungus.IntegerVariable)value;
                    valueTyp        = newType;
                    valueKey        = newKey;
                    variableType    = fungusVariableData.INT;
                    valueInt        = temp.Value;
                }
                else if (newType == typeof(Fungus.BooleanVariable))
                {
                    Fungus.BooleanVariable temp = (Fungus.BooleanVariable)value;
                    valueTyp        = newType;
                    valueKey        = newKey;
                    variableType    = fungusVariableData.BOOL;
                    valueBool       = temp.Value;

                }
                else if (valueTyp == typeof(Fungus.StringVariable))
                {
                    Fungus.StringVariable temp = (Fungus.StringVariable)value;
                    valueTyp        = newType;
                    valueKey        = newKey;
                    variableType    = fungusVariableData.STRING;
                    valueString     = temp.Value;

                }
                else if (valueTyp == typeof(Fungus.FloatVariable))
                {
                    Fungus.FloatVariable temp = (Fungus.FloatVariable)value;
                    valueTyp        = newType;
                    valueKey        = newKey;
                    variableType    = fungusVariableData.FLOAT;
                    valueFloat      = temp.Value;

                }
                else
                {
                    Debug.Log("ERROR then seting valueData in MONO_SaveAndLoad: this (" +newKey+") new data has no reqognised type (" + newType.ToString() +")");
                }
            }

        }

    }
}