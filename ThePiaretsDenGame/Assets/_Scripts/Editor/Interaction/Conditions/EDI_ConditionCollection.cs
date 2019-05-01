using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(SOBJ_ConditionCollection))]
public class EDI_ConditionCollection : EDI_EditorWithSubEditors<EDI_ConditionAdvanced, SOBJ_ConditionAdvanced>
{
    private SOBJ_ConditionCollection conditionCollection;         // Reference to the target.

    public SerializedProperty  collectionsProperty;         // Represents the array of ConditionCollections that the target belongs to.  
    private SerializedProperty descriptionProperty;         // Represents a string description for the target.
    private SerializedProperty conditionsProperty;          // Represents an array of Conditions for the target.
    private SerializedProperty reactionCollectionProperty;  // Represents the ReactionCollection that is referenced by the target.


    private const float conditionButtonWidth = 30f;             // Width of the button for adding a new Condition.
    private const float collectionButtonWidth = 125f;           // Width of the button for removing the target from it's Interactable.
   
    // Name of the field that represents a string description for the target.
    private const string conditionCollectionPropDescriptionName        = "description"; 

    // Name of the field that represents an array of Conditions for the target.
    private const string conditionCollectionPropRequiredConditionsName = "requiredConditions"; 
    
    // Name of the field that represents the ReactionCollection that is referenced by the target.
    private const string conditionCollectionPropReactionCollectionName = "reactionCollection";


    // Caching the vertical spacing between GUI elements.
    private readonly float verticalSpacing = EditorGUIUtility.standardVerticalSpacing;


    private Type[]   conditionTypes;          // All the non-abstract types which inherit from SOBJ_ConditionAdvanced.  This is used for adding new Reactions.
    private string[] conditionTypeNames;      // The names of all appropriate SOBJ_ConditionAdvanced types.
    private int      selectedIndex;           // The index of the currently selected SOBJ_ConditionAdvanced type.


    private const float dropAreaHeight = 50f;  // Height in pixels of the area for dropping scripts.
    private const float controlSpacing = 5f;   // Width in pixels between the popup type selection and drop area.
    private const float buttonWidth    = 30f;  // Width in pixels of the button to create Conditions.
    
    private void OnEnable()
    {
        // Cache a reference to the target.
        conditionCollection = (SOBJ_ConditionCollection)target;

        // If this Editor exists but isn't targeting anything destroy it.
        if (target == null)
        {
            DestroyImmediate(this);
            return;
        }

        // Cache the SerializedProperties.
        descriptionProperty        = serializedObject.FindProperty(conditionCollectionPropDescriptionName);
        conditionsProperty         = serializedObject.FindProperty(conditionCollectionPropRequiredConditionsName);
        reactionCollectionProperty = serializedObject.FindProperty(conditionCollectionPropReactionCollectionName);

        // Check if the Editors for the Conditions need creating and optionally create them.
        CheckAndCreateSubEditors(conditionCollection.requiredConditions);
        EXT_GetListOfScriptableObjects.SetGenericNamesArray(typeof(SOBJ_ConditionAdvanced), out conditionTypes, out conditionTypeNames);
    }


    private void OnDisable()
    {
        // When this Editor ends, destroy all it's subEditors.
        CleanupEditors();
    }


    // This is called immediately when a subEditor is created.
    protected override void SubEditorSetup(EDI_ConditionAdvanced editor)
    {
        // Set the editor type so that the correct GUI for Condition is shown.
        editor.editorType = EDI_ConditionAdvanced.EditorType.ConditionCollection;

        /* Assign the conditions property so that the 
         * ConditionEditor can remove its target if necessary.
         */ 
        editor.conditionsProperty = conditionsProperty;
    }


    public override void OnInspectorGUI()
    {
        // Pull the information from the target into the serializedObject.
        serializedObject.Update();
      
        /* Check if the Editors for the Conditions need 
         * creating and optionally create them.
         */
        CheckAndCreateSubEditors(conditionCollection.requiredConditions);

        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;

        EditorGUILayout.BeginHorizontal();

        /* Use the isExpanded bool for the descriptionProperty 
         * to store whether the foldout is open or closed.
         */ 
        descriptionProperty.isExpanded = EditorGUILayout.Foldout(descriptionProperty.isExpanded, descriptionProperty.stringValue);

        /* Display a button showing 'Remove Collection' which 
         * removes the target from the Interactable when clicked.
         */
        if (GUILayout.Button("Remove Collection", GUILayout.Width(collectionButtonWidth)))
        {
            collectionsProperty.RemoveFromObjectArray(conditionCollection);
        }

        EditorGUILayout.EndHorizontal();

        // If the foldout is open show the expanded GUI.
        if (descriptionProperty.isExpanded)
        {
            ExpandedGUI();
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();

        // Push all changes made on the serializedObject back to the target.
        serializedObject.ApplyModifiedProperties();
    }


    private void ExpandedGUI()
    {
        EditorGUILayout.Space();

        // Display the description for editing.
        EditorGUILayout.PropertyField(descriptionProperty);

        EditorGUILayout.Space();

        /* Display the Labels for the Conditions evenly split over
         * the width of the inspector.
         */ 
        float space = EditorGUIUtility.currentViewWidth / 3f;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Condition", GUILayout.Width(space));
        EditorGUILayout.LabelField("Satisfied?", GUILayout.Width(space));
        EditorGUILayout.LabelField("Add/Remove", GUILayout.Width(space));
        EditorGUILayout.EndHorizontal();

        // Display each of the Conditions.
        EditorGUILayout.BeginVertical(GUI.skin.box);
        for (int i = 0; i < subEditors.Length; i++)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            subEditors[i].OnInspectorGUI();
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndHorizontal();


        TypeSelectionGUI();

        EditorGUILayout.Space();

        // Display the reference to the ReactionCollection for editing.
        EditorGUILayout.PropertyField(reactionCollectionProperty);
    }

   
    private void TypeSelectionGUI()
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


        // Create Rects for the top and bottom half.
        Rect topHalf     = leftAreaRect;
        topHalf.height  *= 0.5f;
        Rect bottomHalf  = topHalf;
        bottomHalf.y    += bottomHalf.height;

        // Display a popup in the top half showing all the reaction types.
        selectedIndex = EditorGUI.Popup(topHalf, selectedIndex, conditionTypeNames);

        // Display a button in the bottom half that if clicked...
        if (GUI.Button(bottomHalf, "Add Selected Condition type"))
        {
            // ... finds the type selected by the popup, creates an appropriate reaction and adds it to the array.
            Type reactionType                  = conditionTypes[selectedIndex];
            SOBJ_ConditionAdvanced newReaction = EDI_ConditionAdvanced.CreateCondition(reactionType);
            conditionsProperty.AddToObjectArray(newReaction);
        }
    }
   
    /// <summary>
    /// This function is static such that it
    /// can be called without an editor being instanced.
    /// creats a defult condition reaction
    /// </summary>
    /// <returns></returns>
    public static SOBJ_ConditionCollection CreateConditionCollection()
    {
        // Create a new instance of ConditionCollection.
        SOBJ_ConditionCollection newConditionCollection = CreateInstance<SOBJ_ConditionCollection>();

        // Give it a default description.
        newConditionCollection.description = "New condition collection";

        //Give it a single default Condition.
        newConditionCollection.requiredConditions    = new SOBJ_Condition[1];
        newConditionCollection.requiredConditions[0] = EDI_ConditionAdvanced.CreateCondition();
        return newConditionCollection;
    }
}
