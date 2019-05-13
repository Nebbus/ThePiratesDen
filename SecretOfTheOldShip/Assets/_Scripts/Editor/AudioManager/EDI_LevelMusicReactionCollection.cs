using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;


[CustomEditor(typeof(MONO_LevelMusicReactionCollection))]
public class EDI_LevelMusicReactionCollection : EDI_EditorWithSubEditors<EDI_Reaction, SOBJ_FMODreaction>
{
    private MONO_LevelMusicReactionCollection FMODreactionCollection;          // Reference to the target.
    private SerializedProperty               FMODreactionsProperty;           // Represents the array of Reactions.

    private Type[]   FMODreactionTypes;                           // All the non-abstract types which inherit from Reaction.  This is used for adding new Reactions.
    private string[] FMODreactionTypeNames;                       // The names of all appropriate Reaction types.
    private int      FMODselectedIndex;                           // The index of the currently selected Reaction type.

    private const float dropAreaHeight = 50f;           // Height in pixels of the area for dropping scripts.
    private const float controlSpacing = 5f;            // Width in pixels between the popup type selection and drop area.
    private const string reactionsPropName = "FMODreactions";   // Name of the field for the array of Reactions.

    // Caching the vertical spacing between GUI elements.
    private readonly float verticalSpacing = EditorGUIUtility.standardVerticalSpacing;

    private void OnEnable()
    {
        // Cache the target.
        FMODreactionCollection = (MONO_LevelMusicReactionCollection)target;

        // Cache the SerializedProperty
        FMODreactionsProperty = serializedObject.FindProperty(reactionsPropName);

        // If new editors are required for Reactions, create them.
        CheckAndCreateSubEditors(FMODreactionCollection.FMODreactions);

        // Set the array of types and type names of subtypes of Reaction.
        EXT_GetListOfScriptableObjects.SetGenericNamesArray(typeof(SOBJ_FMODreaction), out FMODreactionTypes, out FMODreactionTypeNames);
    }


    private void OnDisable()
    {
        // Destroy all the subeditors.
        CleanupEditors();
    }

    /// <summary>
    /// This is called immediately after each ReactionEditor is created.
    /// </summary>
    /// <param name="editor">Editor to be set up, will get a 
    /// refference to its reaction type </param>
    protected override void SubEditorSetup(EDI_Reaction editor)
    {
        /* Make sure the EDI_Reaction have a reference
         *to the array that contains their targets.
         */
        editor.reactionsProperty = FMODreactionsProperty;
    }


    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox(" Spesific reaction colection for the music and ambiens (allso can acces other FMOD reactions)", MessageType.Info);

        // Pull all the information from the target into the serializedObject.
        serializedObject.Update();

        // If new editors for Reactions are required, create them.
        CheckAndCreateSubEditors(FMODreactionCollection.FMODreactions);

        // Display all the Reactions.
        for (int i = 0; i < subEditors.Length; i++)
        {
            subEditors[i].OnReactionCollectionGUI();
        }

        // If there are Reactions, add a space.
        if (FMODreactionCollection.FMODreactions.Length > 0)
        {
            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }

        /* Create a Rect for the full width of the
         * inspector with enough height for the drop area.
         */
        Rect fullWidthRect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(dropAreaHeight + verticalSpacing));

        // Create a Rect for the left GUI controls.
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

        /* Create a Rect for the right GUI controls that is
         * the same as the left Rect except...
         */
        Rect rightAreaRect = leftAreaRect;

        // ... it should be on the right.
        rightAreaRect.x += rightAreaRect.width + controlSpacing;

        // Display the GUI for the type popup and button on the left.
        TypeSelectionGUI(leftAreaRect);

        // Push the information back from the serializedObject to the target.
        serializedObject.ApplyModifiedProperties();
    }


    private void TypeSelectionGUI(Rect containingRect)
    {
        // Create Rects for the top and bottom half.
        Rect topHalf = containingRect;
        topHalf.height *= 0.5f;
        Rect bottomHalf = topHalf;
        bottomHalf.y += bottomHalf.height;

        // Display a popup in the top half showing all the reaction types.
        FMODselectedIndex = EditorGUI.Popup(topHalf, FMODselectedIndex, FMODreactionTypeNames);

        // Display a button in the bottom half that if clicked...
        if (GUI.Button(bottomHalf, "Add Selected Reaction"))
        {
            // ... finds the type selected by the popup, creates an appropriate reaction and adds it to the array.
            Type reactionType = FMODreactionTypes[FMODselectedIndex];
            SOBJ_Reaction newReaction = EDI_Reaction.CreateReaction(reactionType);
            FMODreactionsProperty.AddToObjectArray(newReaction);
        }



    }
}
