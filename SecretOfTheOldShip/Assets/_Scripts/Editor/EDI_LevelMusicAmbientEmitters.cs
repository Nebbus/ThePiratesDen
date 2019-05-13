using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SOBJ_LevelMusicAmbientEmitters))]
public class EDI_LevelMusicAmbientEmitters : Editor
{

    // The path that the AllConditions asset is created at.
    private const string creationPath = "Assets/Resources/SOBJ_LevelMusicAmbientEmitters.asset";


    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("This is only a chach to get the paramters from the music and ambion event,", MessageType.Info);
        base.OnInspectorGUI();
    }

    // Call this function when the menu item is selected.
    [MenuItem("Assets/Create/LevelMusicAmbientEmitters")]
    private static void CreateAllConditionsAsset()
    {
        // If there's already an AllConditions asset, do nothing.
        if (SOBJ_LevelMusicAmbientEmitters.Instance)
        {
            return;
        }


        // Create an instance of the AllConditions object and make an asset for it.
        SOBJ_LevelMusicAmbientEmitters instance = CreateInstance<SOBJ_LevelMusicAmbientEmitters>();
        AssetDatabase.CreateAsset(instance, creationPath);

        // Set this as the singleton instance.
        SOBJ_LevelMusicAmbientEmitters.Instance = instance;

    }
}
