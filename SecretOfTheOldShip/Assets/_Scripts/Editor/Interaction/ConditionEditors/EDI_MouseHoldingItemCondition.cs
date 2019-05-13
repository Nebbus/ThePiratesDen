using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.UI;

// This class controls all the GUI for Conditions
// in all the places they are found.
[CustomEditor(typeof(SOBJ_MouseHoldingItemCondition))]
public class EDI_MouseHoldingItemCondition : EDI_ConditionAdvanced
{
    // Name of the field that represents requierd item
    private const string conditionPropholdingItem = "requierdHoldingItem";

    private float imageSide = 125f;

    // The SOBJ_Item that represents requierd item
    protected SerializedProperty holdingItemProperty;         

  
    protected override void Init()
    {
        holdingItemProperty = serializedObject.FindProperty(conditionPropholdingItem);
    }

    protected override void DrawConditionInteractableGUI()
    {
        // The width for the Popup, item window, the image and remove Button.
        float width = EditorGUIUtility.currentViewWidth /3f;
        
        EditorGUILayout.BeginVertical(GUILayout.Width(imageSide));
            EditorGUILayout.PropertyField(holdingItemProperty, GUIContent.none, GUILayout.Width(width + toggleOffset));
            EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("the mous is holdin: ", GUILayout.Width(170f));
               EditorGUILayout.PropertyField(satisfiedProperty, GUIContent.none, GUILayout.Width(width/2f));
            EditorGUILayout.EndHorizontal();
            DrawImages(width + toggleOffset);

        EditorGUILayout.EndVertical();
    }

  
    /// <summary>
    /// Displays the sprite of the item in the conditions
    /// </summary>
    private void DrawImages(float width)
    {
      
        SOBJ_MouseHoldingItemCondition targetCondition = (SOBJ_MouseHoldingItemCondition)target;

        if (targetCondition                               != null && 
            targetCondition.getRequierdHoldingItem        != null && 
            targetCondition.getRequierdHoldingItem.sprite != null)
        {
            GUILayout.Box(targetCondition.getRequierdHoldingItem.sprite.texture, GUILayout.Width(imageSide), GUILayout.Height(imageSide));
           
        }
    }

    public override string[] getListOfReleveantConditions()
    {
        return EDI_AllConditions.getListOfReleveantConditions<SOBJ_MouseHoldingItemCondition>(); ;
    }
}
