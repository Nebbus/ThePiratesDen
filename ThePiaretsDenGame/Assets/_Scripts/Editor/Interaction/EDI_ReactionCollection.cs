using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

/* This is the Editor for the ReactionCollection MonoBehaviour.
 * However, since the ReactionCollection contains many Reactions, 
 * it requires many sub-editors to display them.
 * For more details see the EditorWithSubEditors class.
 * There are two ways of adding Reactions to the ReactionCollection:
 * a type selection popup with confirmation button and a drag and drop
 * area.  Details on these are found below.
*/
[CustomEditor(typeof(MONO_ReactionCollection))]
public class EDI_ReactionCollection : EDI_EditorWithSubEditors<EDI_Reaction, SOBJ_Reaction>
{
    private MONO_ReactionCollection reactionCollection;          // Reference to the target.
    private SerializedProperty      reactionsProperty;           // Represents the array of Reactions.

    private Type[]      reactionTypes;                           // All the non-abstract types which inherit from Reaction.  This is used for adding new Reactions.
    private string[]    reactionTypeNames;                       // The names of all appropriate Reaction types.
    private int         selectedIndex;                           // The index of the currently selected Reaction type.

    private const float     dropAreaHeight      = 50f;           // Height in pixels of the area for dropping scripts.
    private const float     controlSpacing      = 5f;            // Width in pixels between the popup type selection and drop area.
    private const string    reactionsPropName   = "reactions";   // Name of the field for the array of Reactions.

    // Caching the vertical spacing between GUI elements.
    private readonly float verticalSpacing = EditorGUIUtility.standardVerticalSpacing;



    private void OnEnable()
    {
        // Cache the target.
        reactionCollection = (MONO_ReactionCollection)target;

        // Cache the SerializedProperty
        reactionsProperty = serializedObject.FindProperty(reactionsPropName);

        // If new editors are required for Reactions, create them.
        CheckAndCreateSubEditors(reactionCollection.reactions);

        // Set the array of types and type names of subtypes of Reaction.
        EXT_GetListOfScriptableObjects.SetGenericNamesArray(typeof(SOBJ_Reaction), out reactionTypes, out reactionTypeNames);
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
        editor.reactionsProperty = reactionsProperty;
    }


    public override void OnInspectorGUI()
    {
        // Pull all the information from the target into the serializedObject.
        serializedObject.Update();

        // If new editors for Reactions are required, create them.
        CheckAndCreateSubEditors(reactionCollection.reactions);

        // Display all the Reactions.
        for (int i = 0; i < subEditors.Length; i++)
        {
            subEditors[i].OnInspectorGUI();
        }

        // If there are Reactions, add a space.
        if (reactionCollection.reactions.Length > 0)
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

        // Display the GUI for the drag and drop area on the right.
        DragAndDropAreaGUI(rightAreaRect);

        // Manage the events for dropping on the right area.
        DraggingAndDropping(rightAreaRect, this);

        // Push the information back from the serializedObject to the target.
        serializedObject.ApplyModifiedProperties();
    }


    private void TypeSelectionGUI(Rect containingRect)
    {
        // Create Rects for the top and bottom half.
        Rect topHalf     = containingRect;
        topHalf.height  *= 0.5f;
        Rect bottomHalf  = topHalf;
        bottomHalf.y    += bottomHalf.height;

        // Display a popup in the top half showing all the reaction types.
        selectedIndex = EditorGUI.Popup(topHalf, selectedIndex, reactionTypeNames);

        // Display a button in the bottom half that if clicked...
        if (GUI.Button(bottomHalf, "Add Selected Reaction"))
        {
            // ... finds the type selected by the popup, creates an appropriate reaction and adds it to the array.
            Type reactionType         = reactionTypes[selectedIndex];
            SOBJ_Reaction newReaction = EDI_Reaction.CreateReaction(reactionType);
            reactionsProperty.AddToObjectArray(newReaction);
        }
    }


    private static void DragAndDropAreaGUI(Rect containingRect)
    {
        // Create a GUI style of a box but with middle aligned text and button text color.
        GUIStyle centredStyle         = GUI.skin.box;
        centredStyle.alignment        = TextAnchor.MiddleCenter;
        centredStyle.normal.textColor = GUI.skin.button.normal.textColor;

        // Draw a box over the area with the created style.
        GUI.Box(containingRect, "Drop new Reactions here", centredStyle);
    }


    private static void DraggingAndDropping(Rect dropArea, EDI_ReactionCollection editor)
    {
        // Cache the current event.
        Event currentEvent = Event.current;

        // If the drop area doesn't contain the mouse then return.
        if (!dropArea.Contains(currentEvent.mousePosition))
            return;

        switch (currentEvent.type)
        {
            // If the mouse is dragging something...
            case EventType.DragUpdated:

                /* ... change whether or not the drag *can* be 
                 * performed by changing the visual mode of the
                 * cursor based on the IsDragValid function.
                 */ 
                DragAndDrop.visualMode = IsDragValid() ? DragAndDropVisualMode.Link : DragAndDropVisualMode.Rejected;

                // Make sure the event isn't used by anything else.
                currentEvent.Use();

                break;

            // If the mouse was dragging something and has released...
            case EventType.DragPerform:

                // ... accept the drag event.
                DragAndDrop.AcceptDrag();

                // Go through all the objects that were being dragged...
                for (int i = 0; i < DragAndDrop.objectReferences.Length; i++)
                {
                    // ... and find the script asset that was being dragged...
                    MonoScript script = DragAndDrop.objectReferences[i] as MonoScript;

                    // ... then find the type of that Reaction...
                    Type reactionType = script.GetClass();

                    // ... and create a Reaction of that type and add it to the array.
                    SOBJ_Reaction newReaction = EDI_Reaction.CreateReaction(reactionType);
                    editor.reactionsProperty.AddToObjectArray(newReaction);
                }

                // Make sure the event isn't used by anything else.
                currentEvent.Use();

                break;
        }
    }


    private static bool IsDragValid()
    {
        // Go through all the objects being dragged...
        for (int i = 0; i < DragAndDrop.objectReferences.Length; i++)
        {
            /* ... and if any of them are not script assets,
             * return that the drag is invalid.
             */ 
            if (DragAndDrop.objectReferences[i].GetType() != typeof(MonoScript))
            {
                return false;
            }

            // Otherwise find the class contained in the script asset.
            MonoScript script = DragAndDrop.objectReferences[i] as MonoScript;
            Type scriptType   = script.GetClass();

            /* If the script does not inherit from Reaction,
             * return that the drag is invalid.
             */ 
            if (!scriptType.IsSubclassOf(typeof(SOBJ_Reaction)))
            {
                return false;
            }

            /* If the script is an abstract, return that 
             * the drag is invalid.
             */ 
            if (scriptType.IsAbstract)
            {
                return false;
            }

        }

        /* If none of the dragging objects returned 
         * that the drag was invalid, return that it is valid.
         */ 
        return true;
    }


    

}
