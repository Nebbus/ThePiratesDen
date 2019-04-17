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
        EditorGUILayout.PropertyField(holdingItemProperty, GUIContent.none, GUILayout.Width(width + toggleOffset));
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


}
