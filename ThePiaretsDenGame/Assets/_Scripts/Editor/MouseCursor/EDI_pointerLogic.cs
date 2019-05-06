using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MONO_pointerLogic))]
public class EDI_pointerLogic : Editor {

    private MONO_pointerLogic monoPointerLogic;

    private SerializedProperty currentActionProperty;


    private SerializedProperty mainCameraGraycasterProperty;
    private SerializedProperty presistentCanvansPraycasterProperty;


    private SerializedProperty debugAllPrayHitProperty;
    private SerializedProperty debugAllGrayHitProperty;
    private SerializedProperty debugCurentHitProperty;

 
    private SerializedProperty museButtonProperty;
    private SerializedProperty keyButtonProperty;
    private SerializedProperty usedClickKeyProperty;

    private SerializedProperty lastClickTimeProperty;
    private SerializedProperty timeThresholdProperty;
    private SerializedProperty debugTimedeltaProperty;

    private const string mainCameraGraycasterPropertyName           = "mainCameraGraycaster";
    private const string presistentCanvansPraycasterPropertyName    = "presistentCanvansPraycaster";

    private const string monoPointerLogicName                       = "currentAction";

    private const string debugAllPrayHitPropertyName                = "allGraphicalHitsDebug";
    private const string debugAllGrayHitPropertyName                = "allPhysicalHitsDebug";
    private const string debugCurentHitPropertyName                 = "overCurentObject";

    private const string museButtonPropertyyName                    = "mouseKey";
    private const string keyButtonPropertyName                      = "keabordKey";
    private const string usedClickKeyPropertyName                   = "usedClickKey";

    private const string lastClickTimePropertyName                  = "lastClickTime";
    private const string timeThresholdPropertyName                  = "timeThreshold";
    private const string debugTimedeltaPropertyName                 = "timedelta";


    public void OnEnable()
    {
        monoPointerLogic = (MONO_pointerLogic)target;

        mainCameraGraycasterProperty        = serializedObject.FindProperty(mainCameraGraycasterPropertyName);
        presistentCanvansPraycasterProperty = serializedObject.FindProperty(presistentCanvansPraycasterPropertyName);

        currentActionProperty               = serializedObject.FindProperty(monoPointerLogicName);
        debugAllPrayHitProperty             = serializedObject.FindProperty(debugAllPrayHitPropertyName);
        debugAllGrayHitProperty             = serializedObject.FindProperty(debugAllGrayHitPropertyName);
        debugCurentHitProperty              = serializedObject.FindProperty(debugCurentHitPropertyName);
        museButtonProperty                  = serializedObject.FindProperty(museButtonPropertyyName);
        keyButtonProperty                   = serializedObject.FindProperty(keyButtonPropertyName);
        usedClickKeyProperty                = serializedObject.FindProperty(usedClickKeyPropertyName);
        lastClickTimeProperty               = serializedObject.FindProperty(lastClickTimePropertyName);
        timeThresholdProperty               = serializedObject.FindProperty(timeThresholdPropertyName);
        debugTimedeltaProperty              = serializedObject.FindProperty(debugTimedeltaPropertyName);

    }


    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
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
            EditorGUILayout.BeginVertical(GUI.skin.box);
                EditorGUILayout.HelpBox("The time that has to pas for a click to be registerd as a click, " +
                                         "to prevent readings of multiple click the dubble tapping",MessageType.Info);
               EditorGUILayout.PropertyField(timeThresholdProperty);
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginHorizontal(GUI.skin.box);
                EditorGUILayout.PropertyField(museButtonProperty);
                EditorGUILayout.PropertyField(keyButtonProperty);
            EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

    }

    private void DrawDebug()
    {
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUI.indentLevel++;
            EditorGUILayout.LabelField("DEBUG");

            //Debugs the time since the last click
            EditorGUILayout.BeginHorizontal(GUI.skin.box);
                GUI.enabled = false;
                 EditorGUILayout.HelpBox("The time between the two most resent clicks",MessageType.Info);
                EditorGUILayout.PropertyField(debugTimedeltaProperty,GUIContent.none);
                GUI.enabled = true;
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginVertical(GUI.skin.box);
                monoPointerLogic.debugAll = EditorGUILayout.Toggle("Debug all raycast hits", monoPointerLogic.debugAll);
                EditorGUILayout.PropertyField(debugCurentHitProperty);
                if (monoPointerLogic.debugAll)
                {
                    EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.BeginVertical(GUI.skin.box);
                            EditorGUILayout.PropertyField(presistentCanvansPraycasterProperty, true);
                            EditorGUILayout.PropertyField(debugAllPrayHitProperty, true);
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.BeginVertical(GUI.skin.box);
                            EditorGUILayout.PropertyField(mainCameraGraycasterProperty, true);
                            EditorGUILayout.PropertyField(debugAllGrayHitProperty, true);
                        EditorGUILayout.EndVertical();
                    EditorGUILayout.EndHorizontal();
                }
            EditorGUILayout.EndVertical();
            EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
    }

}
