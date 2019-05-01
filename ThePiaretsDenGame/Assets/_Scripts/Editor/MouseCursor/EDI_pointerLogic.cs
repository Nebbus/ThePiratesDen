using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MONO_pointerLogic))]
public class EDI_pointerLogic : Editor {

    private MONO_pointerLogic monoPointerLogic;

    private SerializedProperty currentActionProperty;

    private SerializedProperty debugAllPrayHitProperty;
    private SerializedProperty debugAllGrayHitProperty;
    private SerializedProperty debugCurentHitProperty;

 
    private SerializedProperty museButtonProperty;
    private SerializedProperty keyButtonProperty;
    private SerializedProperty usedClickKeyProperty;


    private const string monoPointerLogicName        = "currentAction";

    private const string debugAllPrayHitPropertyName = "allGraphicalHitsDebug";
    private const string debugAllGrayHitPropertyName = "allPhysicalHitsDebug";
    private const string debugCurentHitPropertyName  = "overCurentObject";

    private const string museButtonPropertyyName    = "mouseKey";
    private const string keyButtonPropertyName      = "keabordKey";
    private const string usedClickKeyPropertyName   = "usedClickKey";


    public void OnEnable()
    {
        monoPointerLogic = (MONO_pointerLogic)target;


        currentActionProperty       = serializedObject.FindProperty(monoPointerLogicName);
        debugAllPrayHitProperty     = serializedObject.FindProperty(debugAllPrayHitPropertyName);
        debugAllGrayHitProperty     = serializedObject.FindProperty(debugAllGrayHitPropertyName);
        debugCurentHitProperty      = serializedObject.FindProperty(debugCurentHitPropertyName);
        museButtonProperty          = serializedObject.FindProperty(museButtonPropertyyName);
        keyButtonProperty           = serializedObject.FindProperty(keyButtonPropertyName);
        usedClickKeyProperty        = serializedObject.FindProperty(usedClickKeyPropertyName);

    }


    public override void OnInspectorGUI()
    {
       // base.OnInspectorGUI();
        serializedObject.Update();
        DrawEditor();
        DrawDebug();

    

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawEditor()
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);
            GUI.enabled = false; 
                EditorGUILayout.PropertyField(usedClickKeyProperty);
                EditorGUILayout.PropertyField(currentActionProperty);
            GUI.enabled = true;
            EditorGUILayout.BeginHorizontal(GUI.skin.box);
                EditorGUILayout.PropertyField(museButtonProperty);
                EditorGUILayout.PropertyField(keyButtonProperty);
            EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

    }

    private void DrawDebug()
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUI.indentLevel++;
            monoPointerLogic.debugAll = EditorGUILayout.Toggle("Debug all raycast hits", monoPointerLogic.debugAll);
            EditorGUILayout.PropertyField(debugCurentHitProperty);
            if (monoPointerLogic.debugAll)
            {
                EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.BeginVertical(GUI.skin.box);
                        EditorGUILayout.PropertyField(debugAllPrayHitProperty, true);
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.BeginVertical(GUI.skin.box);
                        EditorGUILayout.PropertyField(debugAllGrayHitProperty, true);
                    EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
    }

}
