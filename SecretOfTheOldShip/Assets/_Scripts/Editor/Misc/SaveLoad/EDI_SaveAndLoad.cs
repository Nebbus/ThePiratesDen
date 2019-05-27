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
       
        if (EditorGUILayout.Toggle(restSace))
        {
            monoSaveLoad.GetdataToSave = new MONO_SaveAndLoad.SaveData();
            monoSaveLoad.Save();
        }
        base.OnInspectorGUI();
    }
}
