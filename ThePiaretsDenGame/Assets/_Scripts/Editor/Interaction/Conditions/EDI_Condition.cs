using UnityEngine;
using UnityEditor;

// This class controls all the GUI for Conditions
// in all the places they are found.
[CustomEditor(typeof(SOBJ_Condition))]
public class EDI_Condition : EDI_ConditionAdvanced
{
    private const string conditionPropSatisfiedName = "satisfied";           // Name of the field that represents whether or not the Condition is satisfied.
    protected SerializedProperty satisfiedProperty;       // Represents a bool of whether this Editor's target is satisfied.
    
    protected override void Init()
    {
        condition = (SOBJ_Condition)target;
        satisfiedProperty = serializedObject.FindProperty(conditionPropSatisfiedName);
    }

    protected override void DrawConditionInteractableGUI()
    {
        // The width for the Popup, Toggle and remove Button.
        float width = EditorGUIUtility.currentViewWidth / 3f;

        // Display the toggle for the satisfied bool.
        EditorGUILayout.PropertyField(satisfiedProperty, GUIContent.none, GUILayout.Width(width + toggleOffset));

    }
}

