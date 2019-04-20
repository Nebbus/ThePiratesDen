using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.UI;

// This class controls all the GUI for Conditions
// in all the places they are found.
[CustomEditor(typeof(SOBJ_ItemCondition))]
public class EDI_ItemCondition : EDI_ConditionAdvanced
{
    private const string conditionPropholdingItem = "holdingItem";           // Name of the field that represents whether or not the Condition is satisfied.
    protected SerializedProperty holdingItemProperty;       // Represents a bool of whether this Editor's target is satisfied.

    private const float ownSpriteWidth = 1 / 3f;
    private const float ownSpriteHight = 1 / 3f;
    protected override void Init()
    {
        condition           = (SOBJ_ItemCondition)target;
        holdingItemProperty = serializedObject.FindProperty(conditionPropholdingItem);
    }

    protected override void DrawConditionInteractableGUI()
    {
        // The width for the Popup, Toggle and remove Button.
        float width = EditorGUIUtility.currentViewWidth / 3f;

        // Display the toggle for the satisfied bool.
        //
        EditorGUILayout.BeginVertical();
        EditorGUILayout.PropertyField(holdingItemProperty, GUIContent.none, GUILayout.Width(width + toggleOffset));
        EditorGUILayout.PropertyField(satisfiedProperty, GUIContent.none, GUILayout.Width(width + toggleOffset));



        EditorGUILayout.EndVertical();
        DrawImages();
    }

  

    /// <summary>
    /// Displays the sprite of the item in the conditions
    /// </summary>
    private void DrawImages()
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);
        SOBJ_ItemCondition targetCondition = (SOBJ_ItemCondition)target;

        if (targetCondition != null && targetCondition.holdingItem != null && targetCondition.holdingItem.sprite != null)
        {
            Texture2D myTexture = AssetPreview.GetAssetPreview(targetCondition.holdingItem.sprite);
            float width         = EditorGUIUtility.currentViewWidth;
            EditorGUI.DrawPreviewTexture(GUILayoutUtility.GetRect(myTexture.width, myTexture.height, GUIStyle.none), myTexture);
        }

        EditorGUILayout.EndVertical();
    }

    public override string[] getListOfReleveantConditions()
    {
        /* Create a new array that has the same number 
         * of elements as there are Conditions.
         */
        string[] allConditions = EDI_AllConditions.AllConditionDescriptions;

        /* Go through the array and assign the description 
         * of the condition at the same index.
         */
        int count = 0;
        for (int i = 0; i < allConditions.Length; i++)
        {
            SOBJ_ItemCondition temp = EDI_AllConditions.TryGetConditionAt(i) as SOBJ_ItemCondition;
            if (temp != null)
            {
                allConditions[count] = EDI_AllConditions.TryGetConditionAt(i).description;
                count++;
            }
        }
        string[] releventConditionsName = new string[count];
        for (int i = 0; i < count; i++)
        {
            releventConditionsName[i] = allConditions[i];
        }

        relevatnConditionDescriptions = releventConditionsName;
        return releventConditionsName;
    }
}
