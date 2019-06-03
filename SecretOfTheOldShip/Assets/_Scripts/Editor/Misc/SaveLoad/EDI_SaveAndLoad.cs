using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(MONO_SaveAndLoad))]
public class EDI_SaveAndLoad : Editor {

    private MONO_SaveAndLoad monoSaveLoad;
    private bool restSace = false;

    private void OnEnable()
    {
        monoSaveLoad = (MONO_SaveAndLoad)target;
        monoSaveLoad.UppdateSavedReckordEditor();
    }


    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("NOTE: [baseNameOfVariableCharts]0 (this base namen whit 0 after) is allocated for the achivments flowchart, this is so that it dosen't get whiped then geting starting a new game", MessageType.Info);
       
        if (EditorGUILayout.Toggle(restSace))
        {
            monoSaveLoad.GetdataToSave = new MONO_SaveAndLoad.SaveData();
            monoSaveLoad.Save();
            monoSaveLoad.ClearAchivments();
        }
        base.OnInspectorGUI();
    }
}
