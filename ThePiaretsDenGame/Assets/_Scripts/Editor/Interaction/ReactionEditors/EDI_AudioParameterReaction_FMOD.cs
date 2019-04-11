using FMODUnity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SOBJ_AudioParameterReaction_FMOD))]
[CanEditMultipleObjects]
public class EDI_AudioParameterReaction_FMOD : EDI_Reaction
{

    protected override string GetFoldoutLabel()
    {
        return "SOBJ_AudioParameterReaction_FMOD";
    }

    /// <summary>
    /// _UpdateParamsOnEmitter part 1.
    /// Updates the values of each parameter in the emitter. 
    /// Function is copied from Assets\Plugins\Editor\FMOD\EditorUtils.cs
    /// </summary>
    /// <param name="serializedObject"> Object that will react with audio reaction </param>
    /// <param name="path"> Path to script that holds reference to FMOD parameters </param>
    public void _UpdateParamsOnEmitter(SerializedObject serializedObject, string path)
    {
        if (String.IsNullOrEmpty(path) || EventManager.EventFromPath(path) == null)
        {
            return;
        }

        var eventRef = EventManager.EventFromPath(path);
        serializedObject.ApplyModifiedProperties();
        if (serializedObject.isEditingMultipleObjects)
        {
            foreach (var obj in serializedObject.targetObjects)
            {
                _UpdateParamsOnEmitter(obj, eventRef);
            }
        }
        else
        {
            _UpdateParamsOnEmitter(serializedObject.targetObject, eventRef);
        }
        serializedObject.Update();
    }
    /// <summary>
    /// _UpdateParamsOnEmitter part 2.
    /// Updates the values of each parameter in the emitter. 
    /// Function is copied from Assets\Plugins\Editor\FMOD\EditorUtils.cs 
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="eventRef"></param>
    private void _UpdateParamsOnEmitter(UnityEngine.Object obj, EditorEventRef eventRef)
    {
        var emitter = obj as SOBJ_AudioReaction_FMOD;
        if (emitter == null)
        {
            // Custom game object
            return;
        }

        for (int i = 0; i < emitter.Params.Length; i++)
        {
            if (!eventRef.Parameters.Exists((x) => x.Name == emitter.Params[i].Name))
            {
                int end = emitter.Params.Length - 1;
                emitter.Params[i] = emitter.Params[end];
                Array.Resize<ParamRef>(ref emitter.Params, end);
                i--;
            }
        }
    }

    protected override void DrawReaction()
    {

        var tag    = serializedObject.FindProperty("CollisionTag");
        var ev     = serializedObject.FindProperty("Event");
        var param = serializedObject.FindProperty("Params");
        var preload = serializedObject.FindProperty("Preload");



        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(ev, new GUIContent("Event"));

        EditorEventRef editorEvent = EventManager.EventFromPath(ev.stringValue);


        if (editorEvent != null)
        {

            param.isExpanded = EditorGUILayout.Foldout(param.isExpanded, "Parameter sets to Values");
            if (ev.hasMultipleDifferentValues)
            {
                if (param.isExpanded)
                {
                    GUILayout.Box("Cannot change parameters when different events are selected", GUILayout.ExpandWidth(true));
                }
            }
            else
            {
                var eventRef = EventManager.EventFromPath(ev.stringValue);
                if (param.isExpanded && eventRef != null)
                {
                    foreach (var paramRef in eventRef.Parameters)
                    {
                        bool set;
                        float value;
                        bool matchingSet, matchingValue;
                        CheckParameter(paramRef.Name, out set, out matchingSet, out value, out matchingValue);

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.PrefixLabel(paramRef.Name);
                        EditorGUI.showMixedValue = !matchingSet;
                        EditorGUI.BeginChangeCheck();
                        bool newSet = EditorGUILayout.Toggle(set, GUILayout.Width(20));
                        EditorGUI.showMixedValue = false;

                        if (EditorGUI.EndChangeCheck())
                        {
                            Undo.RecordObjects(serializedObject.isEditingMultipleObjects ? serializedObject.targetObjects : new UnityEngine.Object[] { serializedObject.targetObject }, "Inspector");
                            if (newSet)
                            {
                                AddParameterValue(paramRef.Name, paramRef.Default);
                            }
                            else
                            {
                                DeleteParameterValue(paramRef.Name);
                            }
                            set = newSet;
                        }

                        EditorGUI.BeginDisabledGroup(!newSet);
                        if (set)
                        {
                            EditorGUI.showMixedValue = !matchingValue;
                            EditorGUI.BeginChangeCheck();
                            value = EditorGUILayout.Slider(value, paramRef.Min, paramRef.Max);
                            if (EditorGUI.EndChangeCheck())
                            {
                                Undo.RecordObjects(serializedObject.isEditingMultipleObjects ? serializedObject.targetObjects : new UnityEngine.Object[] { serializedObject.targetObject }, "Inspector");
                                SetParameterValue(paramRef.Name, value);
                            }
                            EditorGUI.showMixedValue = false;
                        }
                        else
                        {
                            EditorGUI.showMixedValue = !matchingValue;
                            EditorGUILayout.Slider(paramRef.Default, paramRef.Min, paramRef.Max);
                            EditorGUI.showMixedValue = false;
                        }
                        EditorGUI.EndDisabledGroup();
                        EditorGUILayout.EndHorizontal();
                    }

                }
            }

        }


        serializedObject.ApplyModifiedProperties();
    }

    void CheckParameter(string name, out bool set, out bool matchingSet, out float value, out bool matchingValue)
    {
        value = 0;
        set = false;
        if (serializedObject.isEditingMultipleObjects)
        {
            bool first = true;
            matchingValue = true;
            matchingSet = true;
            foreach (var obj in serializedObject.targetObjects)
            {
                var emitter = obj as SOBJ_AudioParameterReaction_FMOD;
                var param = emitter.Params != null ? emitter.Params.FirstOrDefault((x) => x.Name == name) : null;
                if (first)
                {
                    set = param != null;
                    value = set ? param.Value : 0;
                    first = false;
                }
                else
                {
                    if (set)
                    {
                        if (param == null)
                        {
                            matchingSet = false;
                            matchingValue = false;
                            return;
                        }
                        else
                        {
                            if (param.Value != value)
                            {
                                matchingValue = false;
                            }
                        }
                    }
                    else
                    {
                        if (param != null)
                        {
                            matchingSet = false;
                        }
                    }
                }
            }
        }
        else
        {
            matchingSet = matchingValue = true;

            var emitter = serializedObject.targetObject as SOBJ_AudioParameterReaction_FMOD;
            var param = emitter.Params != null ? emitter.Params.FirstOrDefault((x) => x.Name == name) : null;
            if (param != null)
            {
                set = true;
                value = param.Value;
            }
        }
    }

    void SetParameterValue(string name, float value)
    {
        if (serializedObject.isEditingMultipleObjects)
        {
            foreach (var obj in serializedObject.targetObjects)
            {
                SetParameterValue(obj, name, value);
            }
        }
        else
        {
            SetParameterValue(serializedObject.targetObject, name, value);
        }
    }

    void SetParameterValue(UnityEngine.Object obj, string name, float value)
    {
        var emitter = obj as SOBJ_AudioParameterReaction_FMOD;
        var param = emitter.Params != null ? emitter.Params.FirstOrDefault((x) => x.Name == name) : null;
        if (param != null)
        {
            param.Value = value;
        }
    }


    void AddParameterValue(string name, float value)
    {
        if (serializedObject.isEditingMultipleObjects)
        {
            foreach (var obj in serializedObject.targetObjects)
            {
                AddParameterValue(obj, name, value);
            }
        }
        else
        {
            AddParameterValue(serializedObject.targetObject, name, value);
        }
    }

    void AddParameterValue(UnityEngine.Object obj, string name, float value)
    {
        var emitter = obj as SOBJ_AudioParameterReaction_FMOD;
        var param = emitter.Params != null ? emitter.Params.FirstOrDefault((x) => x.Name == name) : null;
        if (param == null)
        {
            int end = emitter.Params.Length;
            Array.Resize<ParamRef>(ref emitter.Params, end + 1);
            emitter.Params[end] = new ParamRef();
            emitter.Params[end].Name = name;
            emitter.Params[end].Value = value;
        }
    }

    void DeleteParameterValue(string name)
    {
        if (serializedObject.isEditingMultipleObjects)
        {
            foreach (var obj in serializedObject.targetObjects)
            {
                DeleteParameterValue(obj, name);
            }
        }
        else
        {
            DeleteParameterValue(serializedObject.targetObject, name);
        }
    }

    void DeleteParameterValue(UnityEngine.Object obj, string name)
    {
        var emitter = obj as SOBJ_AudioReaction_FMOD;
        int found = -1;
        for (int i = 0; i < emitter.Params.Length; i++)
        {
            if (emitter.Params[i].Name == name)
            {
                found = i;
            }
        }
        if (found >= 0)
        {
            int end = emitter.Params.Length - 1;
            emitter.Params[found] = emitter.Params[end];
            Array.Resize<ParamRef>(ref emitter.Params, end);
        }
    }





}