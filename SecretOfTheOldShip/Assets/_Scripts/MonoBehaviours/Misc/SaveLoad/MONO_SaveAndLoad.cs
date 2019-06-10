using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MONO_SaveAndLoad : MonoBehaviour
{
    public enum fungusVariableData { STRING, INT, BOOL, FLOAT };
    public enum dictionary { SAVED, TObeSAVED };

    public string filename = "/SecretOfTheOldShip.dat";

    [Tooltip("The name of the flowcharts that holds the variable information, " +
             "everything exept the number that tells withc act its for example:" +
             " act 1 (outsie is named VariableFlowchartAct1). this is to make it" +
             " esayer to lokate them then saving ")]
    public string baseNameOfVariableCharts = "VariableFlowchartAct";

    [Space]
    public MONO_Inventory monoInventory = null;






    public variableData GetAchivmetData
    {
        get
        {
            /* NOTE would be more efficent to have it be founden 
            * in geting flowcharts, but its more flexible (
            * dosent has to be cald after the getVariableFlowcharts funktion
            * and more understadebla then its consentratet att one spot
            */


            /// attemts geting achivmet data from current scene
            Fungus.Flowchart[] flowChartsInScene = FindObjectsOfType(typeof(Fungus.Flowchart)) as Fungus.Flowchart[];
            foreach (Fungus.Flowchart chart in flowChartsInScene)
            {
                string name = chart.GetName();
                int difff = name.Length - baseNameOfVariableCharts.Length;// to allow dubble didget flowcharts
                if (difff > 0)
                {
                    if (name.Remove(name.Length - difff) == baseNameOfVariableCharts)
                    {
                        //spare down the achivemt in a separate variable
                        if (name[baseNameOfVariableCharts.Length] == '0' && !newAhivmentFlowchart)
                        {
                            newAhivmentFlowchart    = true;
                            achivmetsFlowchartData  = chart;
                            achivmetsData           = new variableData(achivmetsFlowchartData);
                            return achivmetsData;
                        }

                    }
                }

            }

            //attemts getting it from loading
            if (loadData())
            {
                if (savedData.hasAvchivmentData)
                {
                    return savedData.getSetAchivment;
                }

            }


            // waset abbel to rekover any achivement flowchart, so returns null
            newAhivmentFlowchart    = false;
            achivmetsFlowchartData  = null;
            achivmetsData           = null;

            return achivmetsData;

        }



    }

    [Space]
    [Space]
    [SerializeField]
    private variableData achivmetsData;
    [SerializeField]
    private Fungus.Flowchart achivmetsFlowchartData;
    [SerializeField]
    private bool newAhivmentFlowchart = false;
    [Space]
    [Space]

    [SerializeField]
    private SaveData dataToSave = null;
    public SaveData GetdataToSave
    {
        get
        {
          
            return (dataToSave == null) ? new SaveData() : dataToSave;
        }
        set
        {
            dataToSave = value;
        }
    }
    public bool hasNotSavedData = false;


    [SerializeField]
    private SaveData savedData = null;
    public SaveData GetData
    {
        get
        {
            if (savedData == null && loadData())
            {
                return savedData;
            }
            return (savedData == null) ? new SaveData() : savedData;
        }
    }


    //Only used to Keep track of saved flowcharts;
    // string: name of flowchart, int: index in data varaialbe
    [SerializeField]
    Dictionary<String, int> hasBenSaved = new Dictionary<string, int>();

    //Only used to Keep track of on get saved flowcharts;
    // string: name of flowchart, int: index in data varaialbe
    [SerializeField]
    Dictionary<String, int> hasNotBenSaved = new Dictionary<string, int>();


    /// <summary>
    /// Only uppdates the to be saved file
    /// </summary>
    /// <param name="nextScene"></param>
    public void SaveToTempStorage(string nextScene)
    {

        /*Creats data variabel to be saved
         *Gets savaed data if wated 
         */


        Fungus.Flowchart[] flowChartsInScene = FindObjectsOfType(typeof(Fungus.Flowchart)) as Fungus.Flowchart[];

        Fungus.Flowchart[] variableFlowCharts = getVariableFlowCharts(flowChartsInScene);

        if (hasNotSavedData)
        {
            /* the last saved data dos allready exist
             * inside the dataTobeSaved, so makes sure 
             * to only get the data from that file.
            */
            dataToSave.flowChartVariableData = getAllVariableFlowchartToSave(GetdataToSave, variableFlowCharts, dictionary.TObeSAVED);
        }
        if (loadData())
        {
            /* saves lads last save to add the cages on.
             * dosent save vem
             */ 
            GetdataToSave = new SaveData();
            dataToSave.flowChartVariableData = getAllVariableFlowchartToSave(GetData,variableFlowCharts,dictionary.SAVED);
        }
        else
        {
            GetdataToSave = new SaveData();
            GetdataToSave.flowChartVariableData = getVariableData(variableFlowCharts);
        }

        //spare down the inventory items;
        GetdataToSave.itemsInInentory = DeconstructInventoryItem(monoInventory.invetoryItems);

        //Save curent scene
        dataToSave.currentScene = nextScene;

        //Save all condition variables
        GetdataToSave.conditions.spareAllcondition();

        // Register that data has ben saved
        GetdataToSave.hasSAveData = true;




        //===============================
        // saves the data
        //===============================

        GetdataToSave.getSetAchivment = GetAchivmetData;
        newAhivmentFlowchart = false;
        hasNotSavedData = true;

    }

    public void SaveInGame()
    {
        // hack: saves not saved data betfor doing the actual save
        // (don so not neading to redo any of the old save code
        if (hasNotSavedData)
        {
            Save();
            hasNotSavedData = false;
        }
        GetdataToSave = new SaveData();

        Fungus.Flowchart[] flowChartsInScene = FindObjectsOfType(typeof(Fungus.Flowchart)) as Fungus.Flowchart[];
        Fungus.Flowchart[] variableFlowCharts = getVariableFlowCharts(flowChartsInScene);

        if (loadData())
        {
            dataToSave.flowChartVariableData = getAllVariableFlowchartToSave(GetData, variableFlowCharts,dictionary.SAVED);
        }
        else
        {
            GetdataToSave.flowChartVariableData = getVariableData(variableFlowCharts);
        }

        //spare down the inventory items;
        GetdataToSave.itemsInInentory   = DeconstructInventoryItem(monoInventory.invetoryItems);
        GetdataToSave.currentScene      = SceneManager.GetSceneAt(1).name;
        GetdataToSave.conditions.spareAllcondition();
        GetdataToSave.playerPosData.savePlayerPosition();

        // Register that data has ben saved
        GetdataToSave.hasSAveData = true;

        Save();
    }

    /// <summary>
    /// onlyFor saving data 
    /// </summary>
    public void StartNewGAme()
    {
        GetdataToSave = new SaveData();
        Save();
    }


    public void Save()
    {

        //gets achivment data
        GetdataToSave.getSetAchivment = GetAchivmetData;
        newAhivmentFlowchart          = false;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file    = File.Open(Application.persistentDataPath + filename, FileMode.OpenOrCreate);


        bf.Serialize(file, GetdataToSave);
        file.Close();
        savedData = GetdataToSave;
        UppdateSavedReckord(dictionary.SAVED, savedData);

    }

    

    
    /// <summary>
    /// Loades data from the save file
    /// </summary>
    /// <returns>TRUE: it existed saved data and it was loaded
    ///          FALSE: Dident exist anny saved data, nothing was loaded</returns>
    private bool loadData()
    {
        if (File.Exists(Application.persistentDataPath + filename))
        {

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + filename, FileMode.Open);
            savedData = (SaveData)bf.Deserialize(file);
            file.Close();
            return true;
        }

        return false;
    }


    /// <summary>
    /// Uppdates all flowcharts in 
    /// current sceen whit walus form 
    /// loade data
    /// </summary>
    public void UppdateFlowcharts(SaveData data, dictionary dictionaryToUse)
    {
        Fungus.Flowchart[] flowChartsInScene = FindObjectsOfType(typeof(Fungus.Flowchart)) as Fungus.Flowchart[];

        Dictionary<string, int> temp = (dictionaryToUse == dictionary.SAVED) ? hasBenSaved : hasNotBenSaved;

        foreach(Fungus.Flowchart chart in flowChartsInScene)
        {
            int index;
            if(temp.TryGetValue(chart.name, out index) )
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
            else if (isAchivmentFlochart(chart.name) && data.hasAvchivmentData)
            {
                foreach (valueData value in data.getSetAchivment.variabelValues)
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
    private void UppdateSavedReckord(dictionary uppdatet, SaveData data)
    {
        //uppdate the saved dictonary


        switch(uppdatet)
        {
            case dictionary.SAVED:

                hasBenSaved.Clear();
                for (int i = 0; i < data.flowChartVariableData.Length; i++)
                {
                    hasBenSaved.Add(data.flowChartVariableData[i].flowChartName, i);
                }
                break;
            case dictionary.TObeSAVED:
                hasNotBenSaved.Clear();
                for (int i = 0; i < data.flowChartVariableData.Length; i++)
                {
                    hasNotBenSaved.Add(data.flowChartVariableData[i].flowChartName, i);
                }
                break;
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
        for (int i = 0; i < savedData.flowChartVariableData.Length; i++)
        {
            hasBenSaved.Add(savedData.flowChartVariableData[i].flowChartName, i);
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
            int difff   = name.Length - baseNameOfVariableCharts.Length;// to allow dubble didget flowcharts
            if(difff > 0)
            {
                if (name.Remove(name.Length - difff) == baseNameOfVariableCharts)
                {
                    // Ignores the achivments flowchart here, it will get spared in other placeses
                    if (difff == 1 && name[baseNameOfVariableCharts.Length] != '0')
                    {
                        numberOfVariableFlowCharts++;
                        indexOfvariableFlowCharts.Add(i);
                    }

                   
                }
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
    private variableData[] getAllVariableFlowchartToSave(SaveData data, Fungus.Flowchart[] variableFlowCharts, dictionary dicionaryToUse)
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
                    if (dicionaryToUse == dictionary.SAVED)
                    {
                        hasBenSaved.TryGetValue(serceName, out indexInData);// gets actually index
                    }
                    else
                    {
                       hasNotBenSaved.TryGetValue(serceName, out indexInData);// gets actually index
                    }

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



    /// <summary>
    /// gets vartiabel datas from list of flocharts
    /// </summary>
    /// <param name="flowcharts"></param>
    /// <returns></returns>
    private variableData[] getVariableData(Fungus.Flowchart[] flowcharts)
    {
        List<variableData> completeDataTosave = new List<variableData>();
        foreach (Fungus.Flowchart variabelChart in flowcharts)
        {
            variableData variabledata = new variableData(variabelChart);
            completeDataTosave.Add(variabledata);
        }
        return completeDataTosave.ToArray();
    }


    private string[] DeconstructInventoryItem(SOBJ_Item[] itemsInInventory)
    {
        // string[] names = new string[itemsInInventory.Length];
        List<string> names = new List<string>();
        for (int i = 0; i < itemsInInventory.Length; i++)
        {
            // names[i] = itemsInInventory[i].name;
            if(itemsInInventory[i] != null)
            {
                names.Add(itemsInInventory[i].getName);
            }
        }
        return names.ToArray();
    }
    public SOBJ_Item[] ReconstructInventoryItems(string[] names)
    {
        SOBJ_Item[] items = new SOBJ_Item[names.Length];
        for (int i = 0; i < names.Length; i++)
        {
            items[i] = Resources.Load<SOBJ_Item>("InventoryItems/" + names[i]);
        }
        return items;
    }


//==========================================================================================
// LOAD HELPER
//==========================================================================================

    /// <summary>
    /// Loads data from save file if it exsist, mostly used then loading a game, 
    /// then going from menu to game and betwene menus
    /// </summary>
    /// <param name="applayLodedData"> TRUE: emidiet applays the loaded infomration 
    ///                                FALSE: only loade the values</param>
    public void handLoad(bool applayLodedData)
    {
        GetdataToSave = new SaveData();
        hasNotSavedData = false;

        if (loadData())
        {
            UppdateSavedReckord(dictionary.SAVED, GetData);
            if (applayLodedData)
            {
                UppdateFlowcharts(GetData, dictionary.SAVED);
            }

        }
    }

    /// <summary>
    /// Uppdates from temp save, used betwene scenes in the game
    /// </summary>
    /// <param name="applayLodedData"></param>
    public void loadNotSavedData(bool applayLodedData)
    {

        if (hasNotSavedData)
        {
            UppdateSavedReckord(dictionary.TObeSAVED, GetdataToSave);
            if (applayLodedData)
            {
                UppdateFlowcharts(GetdataToSave, dictionary.TObeSAVED);
            }

        }
    }


//==========================================================================================
 // Achivment HELPER
//==========================================================================================
    /// <summary>
    /// to se if a flowchart is the achivmet flowchart
    /// </summary>
    /// <param name="name"> name if flowchart to check</param>
    /// <returns></returns>
    public bool isAchivmentFlochart(string name)
    { 
        int difff = name.Length - baseNameOfVariableCharts.Length;// to allow dubble didget flowcharts
        return (difff > 0) ? (name.Remove(name.Length - difff) == baseNameOfVariableCharts) : false;
    }

    /// <summary>
    /// Destroys the achivments, cepps rest of the data.
    /// </summary>
    public void ClearAchivments()
    {
        if (loadData())
        {
            savedData.getSetAchivment = null;
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + filename, FileMode.OpenOrCreate);


            bf.Serialize(file, savedData);
            file.Close();
            UppdateSavedReckord(dictionary.TObeSAVED, GetData);
        }

    }




    /// <summary>
    /// Contins the ovar information thant need
    /// to be saved ( i.e like flowcharts and curent scene scene)
    /// </summary>
    [System.Serializable]
    public class SaveData
    {
        public bool hasAvchivmentData = false;
        [SerializeField]
        private variableData achivments = null; // spece for achivemts to ceep it

        public variableData getSetAchivment
        {
            get
            {
                return achivments;
            }
            set
            {
                hasAvchivmentData = (value != null);
                achivments = value;
            }
        }

        public bool hasSAveData                     = false;                // to set if load button is interactable
        public string currentScene                  = "";                   // to load rigth scene
        public string[] itemsInInentory             = new string[0];       // for reqonstrukting the inventory
        public variableData[] flowChartVariableData = new variableData[0]; // all the flowchart variables

        public PlayerPositionData playerPosData = new PlayerPositionData(); // to set the players position

        public AllCondition_ConditiondsValues conditions = new AllCondition_ConditiondsValues(); 




    }
    /// <summary>
    /// Contains a list all the variables
    /// (and the name) of flowchart, variable 
    /// data can be instantiated from a nother
    /// variable data (then it gets cept on
    /// a save) and from a flowchart)
    /// </summary>
    [System.Serializable]
    public class variableData
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
    public class valueData
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

    /// <summary>
    /// Apperantly so will scriptable object not
    /// presist betwene plays then the game is
    /// builded, so we must manula save all the bool
    /// variabels. this contains a list of hashs and
    /// a list of bools
    /// </summary>
    [System.Serializable]
    public class AllCondition_ConditiondsValues
    {
       // public int[]    hashes      = new int[0];
        public int[]    allConditionConditionIndex      = new int[0];
        public bool[]   allConditionConditionsatesfied  = new bool[0];

        /// <summary>
        /// Spares all the values in all conditioon,
        /// will be cald then saving
        /// </summary>
        public void spareAllcondition()
        {
            SOBJ_ConditionAdvanced[] conditions = SOBJ_AllConditions.Instance.conditions;
            int length = conditions.Length;

            allConditionConditionIndex      = new int[length];
            allConditionConditionsatesfied                       = new bool[length];

            for (int i = 0; i < length; i++)
            {
                SOBJ_Condition condition = conditions[i] as SOBJ_Condition;

                allConditionConditionIndex[i]   = (condition == null) ? -1 : i;
                allConditionConditionsatesfied[i]                    = conditions[i].satisfied;
            }  
        }
        /// <summary>
        /// Uppdates all the valus in all condition, 
        /// will be called form MONO_Menu then loading
        /// </summary>
        public void uppdatAllCondition()
        {
            for (int i = 0; i < allConditionConditionsatesfied.Length; i++)
            {
                if(allConditionConditionIndex[i] != -1)
                {
                    SOBJ_AllConditions.Instance.setConditionValues(allConditionConditionIndex[i], allConditionConditionsatesfied[i]);
                }


            }
        }

    }

    /// <summary>
    /// Contains data from that position the 
    /// player thas on then save occured.
    /// has funktion to uppdate the position
    /// </summary>
    [System.Serializable]
    public class PlayerPositionData
    {

        [SerializeField]
        public float tX, tY, tZ;
        [SerializeField]
        public float qX, qY, qZ, qW;


        public Vector3 getPos
        {
            get
            {
                return new Vector3(tX, tY, tZ);
            }
        }
        public Quaternion getRotation
        {
            get
            {
                return new Quaternion(qX, qY, qZ, qW);
            }

        }

        /// <summary>
        /// Saves curent transoforms rotation and stuff
        /// </summary>
        public void savePlayerPosition()
        {
            GameObject playerTransform = GameObject.FindGameObjectWithTag("Player");

            if (playerTransform != null)
            {
                tX = playerTransform.transform.position.x;
                tY = playerTransform.transform.position.y;
                tZ = playerTransform.transform.position.z;
                
                qX = playerTransform.transform.rotation.x;
                qY = playerTransform.transform.rotation.y;
                qZ = playerTransform.transform.rotation.z;
                qW = playerTransform.transform.rotation.w;
            }


        }
    }


}
