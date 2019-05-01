using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SOBJ_LevelMusicAmbientEmiters))]
public class EDI_LevelMusicAmbientEmiters : Editor
{

    // The path that the AllConditions asset is created at.
    private const string creationPath = "Assets/Resources/SOBJ_LevelMusicAmbientEmiters.asset";


    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("This is only a chach to get the paramters from the music and ambion event,", MessageType.Info);
        base.OnInspectorGUI();
    }

    // Call this function when the menu item is selected.
    [MenuItem("Assets/Create/LevelMusicAmbientEmiters")]
    private static void CreateAllConditionsAsset()
    {
        // If there's already an AllConditions asset, do nothing.
        if (SOBJ_LevelMusicAmbientEmiters.Instance)
        {
            return;
        }


        // Create an instance of the AllConditions object and make an asset for it.
        SOBJ_LevelMusicAmbientEmiters instance = CreateInstance<SOBJ_LevelMusicAmbientEmiters>();
        AssetDatabase.CreateAsset(instance, creationPath);

        // Set this as the singleton instance.
        SOBJ_LevelMusicAmbientEmiters.Instance = instance;

    }
}
