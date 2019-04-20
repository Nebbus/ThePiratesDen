using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;



// This class controls all the GUI for Conditions
// in all the places they are found.
[CustomEditor(typeof(SOBJ_Condition))]
public class EDI_Condition : EDI_ConditionAdvanced
{

    private const string conditionsPropName = "conditions";
    private Type[]      conditionTypes;                    // All the non-abstract types which inherit from Reaction.  This is used for adding new Reactions.
    private string[]    conditionTypeNames;                // The names of all appropriate Reaction types.
    private int         selectedIndex;                     // The index of the currently selected Reaction type.

    protected override void Init()
    {
        condition = (SOBJ_Condition)target;
        EXT_GetListOfScriptableObjects.SetGenericNamesArray(typeof(SOBJ_Condition), out conditionTypes, out conditionTypeNames);
    }

    protected override void DrawConditionInteractableGUI()
    {
        // The width for the Popup, Toggle and remove Button.
        float width = EditorGUIUtility.currentViewWidth / 3f;

        // Display the toggle for the satisfied bool.
        EditorGUILayout.PropertyField(satisfiedProperty, GUIContent.none, GUILayout.Width(width + toggleOffset));

    }
   
    public override string[] getListOfReleveantConditions()
    {
    //    /* Create a new array that has the same number 
    //     * of elements as there are Conditions.
    //     */
    //    string[] allConditions = EDI_AllConditions.AllConditionDescriptions;

    //    /* Go through the array and assign the description 
    //     * of the condition at the same index.
    //     */
    //    int count = 0;
    //    for (int i = 0; i < allConditions.Length; i++)
    //    {
    //        SOBJ_Condition temp = EDI_AllConditions.TryGetConditionAt(i) as SOBJ_Condition;
    //        if (temp != null)
    //        {
    //            allConditions[count] = EDI_AllConditions.TryGetConditionAt(i).description;
    //            count++;
    //        }
    //    }
        return EDI_AllConditions.getListOfReleveantConditions<SOBJ_Condition>();
    }
}

