using System;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SOBJ_ChangeTagReaction))]
public class EDI_ChangeTagReaction : EDI_Reaction {


    private int selectedIndex;

    private SerializedProperty newTagProperty;
    private SerializedProperty targetProperty;
    private SerializedProperty delayProperty;

    private const string newTagPropertyName = "newTag";
    private const string targetPropertyName = "target";
    private const string delayPropertyName  = "delay";


//===========================================
// Drop down menu variables
//===========================================
    private readonly float verticalSpacing  = EditorGUIUtility.standardVerticalSpacing;  // Caching the vertical spacing between GUI elements.
    private const float     dropAreaHeight  = 50f;                                      // Height in pixels of the area for dropping scripts.
    private const float     controlSpacing  = 5f;                                       // Width in pixels between the popup type selection and drop area.
    private const float     buttonWidth     = 30f;                                      // Width in pixels of the button to create Conditions.

    private string[] getTags
    {
        get
        {
            return UnityEditorInternal.InternalEditorUtility.tags;
        }
    }


    protected override string GetFoldoutLabel()
    {
        return "ChangeTagReaction";
    }

    protected override void Init()
    {
        newTagProperty = serializedObject.FindProperty(newTagPropertyName);
        targetProperty = serializedObject.FindProperty(targetPropertyName);
        delayProperty  = serializedObject.FindProperty(delayPropertyName);
    }




    protected override void DrawReaction()
    {
        EditorGUILayout.HelpBox("Changes the tag of the target to the selected tag", MessageType.Info);
        TagSelection();
        EditorGUILayout.PropertyField(targetProperty);
        EditorGUILayout.PropertyField(delayProperty);
    }


    private void TagSelection()
    {
        Rect fullWidthRect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(dropAreaHeight + verticalSpacing));

        Rect leftAreaRect = fullWidthRect;

        // It should be in half a space from the top.
        leftAreaRect.y += verticalSpacing * 0.5f;

        /* The width should be slightly less than half 
         * the width of the inspector.
         */
        leftAreaRect.width *= 0.5f;
        leftAreaRect.width -= controlSpacing * 0.5f;

        // The height should be the same as the drop area.
        leftAreaRect.height = dropAreaHeight;


        // Create Rects for the top and .
        Rect topHalf = leftAreaRect;
        topHalf.height *= 0.5f;
        selectedIndex = EditorGUI.Popup(topHalf, selectedIndex, getTags);

        newTagProperty.stringValue = getTags[selectedIndex];



        
    }




}
