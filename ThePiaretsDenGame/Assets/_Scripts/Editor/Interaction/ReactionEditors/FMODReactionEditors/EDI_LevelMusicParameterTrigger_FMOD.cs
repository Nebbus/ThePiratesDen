using FMODUnity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SOBJ_LevelMusicParameterTrigger_FMOD))]
public class EDI_LevelMusicParameterTrigger_FMOD : EDI_Reaction {


    StudioEventEmitter musicTargetEmitter;
    StudioEventEmitter ambientargetEmitter;




    SerializedProperty musicEmiterProperty;
    SerializedProperty ambienceEmiterProperty;



    private const string musicEmiterPropertyName    = "musicEmittor";
    private const string ambienceEmiterPropertyName = "ambienceEmittor";


    private bool[] musicExpanded;
    private bool[] abientExpanded;

    private SOBJ_LevelMusicParameterTrigger_FMOD targetScript ;


    protected override string GetFoldoutLabel()
    {
        return "Level Music and ambience Parameter Trigger FMOD";
    }
    protected override void Init()
    {
        targetScript = (SOBJ_LevelMusicParameterTrigger_FMOD)target;
        musicEmiterProperty     = serializedObject.FindProperty(musicEmiterPropertyName);
        ambienceEmiterProperty  = serializedObject.FindProperty(ambienceEmiterPropertyName);
 

        musicTargetEmitter = null;
  
        for (int i = 0; i < musicEmiterProperty.arraySize; i++)
        {
            musicTargetEmitter = musicEmiterProperty.GetArrayElementAtIndex(i).FindPropertyRelative("Target").objectReferenceValue as StudioEventEmitter;
            if (musicTargetEmitter != null)
            {
                musicTargetEmitter = SOBJ_LevelMusicAmbientEmiters.Instance.getMusicTarget;
                musicExpanded      = new bool[musicTargetEmitter.GetComponents<StudioEventEmitter>().Length];
                break;
            }
        }

        ambientargetEmitter = null;
        for (int i = 0; i < ambienceEmiterProperty.arraySize; i++)
        {
            ambientargetEmitter = ambienceEmiterProperty.GetArrayElementAtIndex(i).FindPropertyRelative("Target").objectReferenceValue as StudioEventEmitter;
            if (ambientargetEmitter != null)
            {
                ambientargetEmitter = SOBJ_LevelMusicAmbientEmiters.Instance.getAmbienceTarget;
                abientExpanded = new bool[ambientargetEmitter.GetComponents<StudioEventEmitter>().Length];
                break;
            }
        }
    }

    protected override void DrawReaction()
    {


       // base.DrawReaction();


        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;
        DrawAmbientGUI(ref ambientargetEmitter, ref ambienceEmiterProperty, ref abientExpanded, SOBJ_LevelMusicAmbientEmiters.Instance.getAmbienceTarget);
        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();

        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;
        DrawAmbientGUI(ref musicTargetEmitter, ref musicEmiterProperty, ref musicExpanded, SOBJ_LevelMusicAmbientEmiters.Instance.getMusicTarget);
        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
    }


    private void DrawAmbientGUI(ref StudioEventEmitter targetEmitter, ref SerializedProperty emitters, ref bool[] expanded, StudioEventEmitter newTargetEmitter)
    {


        //var newTargetEmitter = EditorGUILayout.ObjectField("Target", targetEmitter, typeof(StudioEventEmitter), true) as StudioEventEmitter;

        if (newTargetEmitter != targetEmitter)
        {
            emitters.ClearArray();
            targetEmitter = newTargetEmitter;

            if (targetEmitter == null)
            {
                serializedObject.ApplyModifiedProperties();
                return;
            }

            List<StudioEventEmitter> newEmitters = new List<StudioEventEmitter>();
            targetEmitter.GetComponents<StudioEventEmitter>(newEmitters);
            expanded = new bool[newEmitters.Count];
            foreach (var emitter in newEmitters)
            {
                emitters.InsertArrayElementAtIndex(0);
                emitters.GetArrayElementAtIndex(0).FindPropertyRelative("Target").objectReferenceValue = emitter;
            }
        }

        if (targetEmitter == null)
        {
            return;
        }

        var localEmitters = new List<StudioEventEmitter>();
        targetEmitter.GetComponents<StudioEventEmitter>(localEmitters);

        int emitterIndex = 0;
        foreach (var emitter in localEmitters)
        {
            SerializedProperty emitterProperty = null;
            for (int i = 0; i < emitters.arraySize; i++)
            {
                if (emitters.GetArrayElementAtIndex(i).FindPropertyRelative("Target").objectReferenceValue == emitter)
                {
                    emitterProperty = emitters.GetArrayElementAtIndex(i);
                    break;
                }
            }

            // New emitter component added to game object since we last looked
            if (emitterProperty == null)
            {
                emitters.InsertArrayElementAtIndex(0);
                emitterProperty = emitters.GetArrayElementAtIndex(0);
                emitterProperty.FindPropertyRelative("Target").objectReferenceValue = emitter;
            }


            if (!String.IsNullOrEmpty(emitter.Event))
            {
                expanded[emitterIndex] = EditorGUILayout.Foldout(expanded[emitterIndex], emitter.Event);
                if (expanded[emitterIndex])
                {
                    var eventRef = EventManager.EventFromPath(emitter.Event);
                    foreach (var paramRef in eventRef.Parameters)
                    {
                        bool set = false;
                        int index = -1;
                        for (int i = 0; i < emitterProperty.FindPropertyRelative("Params").arraySize; i++)
                        {
                            if (emitterProperty.FindPropertyRelative("Params").GetArrayElementAtIndex(i).FindPropertyRelative("Name").stringValue == paramRef.Name)
                            {
                                index = i;
                                set = true;
                                break;
                            }
                        }
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.PrefixLabel(paramRef.Name);
                        bool newSet = GUILayout.Toggle(set, "");
                        if (!set && newSet)
                        {
                            index = 0;
                            emitterProperty.FindPropertyRelative("Params").InsertArrayElementAtIndex(0);
                            emitterProperty.FindPropertyRelative("Params").GetArrayElementAtIndex(0).FindPropertyRelative("Name").stringValue = paramRef.Name;
                            emitterProperty.FindPropertyRelative("Params").GetArrayElementAtIndex(0).FindPropertyRelative("Value").floatValue = 0;
                        }
                        if (set && !newSet)
                        {
                            emitterProperty.FindPropertyRelative("Params").DeleteArrayElementAtIndex(index);
                        }
                        set = newSet;
                        EditorGUI.BeginDisabledGroup(!set);
                        if (set)
                        {
                            var valueProperty = emitterProperty.FindPropertyRelative("Params").GetArrayElementAtIndex(index).FindPropertyRelative("Value");
                            valueProperty.floatValue = EditorGUILayout.Slider(valueProperty.floatValue, paramRef.Min, paramRef.Max);
                        }
                        else
                        {
                            EditorGUILayout.Slider(0, paramRef.Min, paramRef.Max);
                        }
                        EditorGUI.EndDisabledGroup();
                        EditorGUILayout.EndHorizontal();
                    }
                }
            }

            emitterIndex++;
        }

        serializedObject.ApplyModifiedProperties();
    }


    


}
