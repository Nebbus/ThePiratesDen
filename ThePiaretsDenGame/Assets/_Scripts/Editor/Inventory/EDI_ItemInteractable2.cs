using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(SOBJ_ItemInteractable))]
public class EDI_ItemInteractable2 : Editor
{
    private SOBJ_ItemInteractable itemInteractabe;          // Reference to the target.

    private SerializedProperty descriptionProperty;         // Represents a string description for the target.
    private SerializedProperty reactionsProperty;           // Represents the array of Reactions.
    private SerializedProperty conditionsProperty;          // Represents an array of Conditions for the target.

    public SerializedProperty itemInteractableProperty;     // Represents the array of MONO_ItemInteractable that the target belongs to.  

    public EDI_Item2 paranteEditor; // referens to the parent so this can be removed


    private EDI_ConditionAdvanced[] conditionEditors;
    private EDI_Reaction[]          reactionsEditors;

    private Type[]   reactionTypes;                           // All the non-abstract types which inherit from Reaction.  This is used for adding new Reactions.
    private string[] reactionTypeNames;                       // The names of all appropriate Reaction types.
    private int      reactionSelectedIndex;                   // The index of the currently selected Reaction type.

    private Type[]   conditionTypes;                        // All the non-abstract types which inherit from SOBJ_ConditionAdvanced.  This is used for adding new Reactions.
    private string[] conditionTypeNames;                    // The names of all appropriate SOBJ_ConditionAdvanced types.
    private int      conditionSelectedIndex;                // The index of the currently selected SOBJ_ConditionAdvanced type.

    private const float dropAreaHeight = 50f;           // Height in pixels of the area for dropping scripts.
    private const float controlSpacing = 5f;            // Width in pixels between the popup type selection and drop area.
    private const float removeuttonWidth = 170f;


    private const string reactionsPropName   = "itemInteractionReactions";
    private const string conditionsPropName  = "requiredConditions";
    private const string descriptionPropName = "description";

    // Caching the vertical spacing between GUI elements.
    private readonly float verticalSpacing = EditorGUIUtility.standardVerticalSpacing;

    public void OnEnable()
    {

        // Cache the target.
        itemInteractabe = (SOBJ_ItemInteractable)target;

        if(target == null)
        {
            DestroyImmediate(this);
        }

        if (itemInteractabe.requiredConditions == null)
        {
            itemInteractabe.requiredConditions = new SOBJ_ConditionAdvanced[0];

        }
        if (itemInteractabe.itemInteractionReactions == null)
        {
            itemInteractabe.itemInteractionReactions = new SOBJ_Reaction[0];

        }

        // If there aren't any editors, create them.
        if (conditionEditors == null)
        {
            CreateConditionEditors();
        }
        // If there aren't any editors, create them.
        if (reactionsEditors == null)
        {
            CreateReactionEditors();
        }


        // Cache the SerializedProperty
        reactionsProperty   = serializedObject.FindProperty(reactionsPropName);
        descriptionProperty = serializedObject.FindProperty(descriptionPropName);
        conditionsProperty  = serializedObject.FindProperty(conditionsPropName);


        //Set the array of types and type names of subtypes of Reaction and condition.
        EXT_GetListOfScriptableObjects.SetGenericNamesArray(typeof(SOBJ_Reaction), out reactionTypes, out reactionTypeNames);
        EXT_GetListOfScriptableObjects.SetGenericNamesArray(typeof(SOBJ_ConditionAdvanced), out conditionTypes, out conditionTypeNames);
    }

    private void OnDisable()
    {
        // Destroy all the editors.
        for (int i = 0; i < conditionEditors.Length; i++)
        {
            DestroyImmediate(conditionEditors[i]);
        }
        for (int i = 0; i < reactionsEditors.Length; i++)
        {
            DestroyImmediate(reactionsEditors[i]);
        }
    }

    public override void OnInspectorGUI()
    {
      // base.OnInspectorGUI();

        serializedObject.Update();
        EditorGUILayout.BeginVertical(GUI.skin.box);

        EditorGUILayout.BeginHorizontal();
        /* Use the isExpanded bool for the descriptionProperty 
         * to store whether the foldout is open or closed.
         */
        descriptionProperty.isExpanded = EditorGUILayout.Foldout(descriptionProperty.isExpanded, descriptionProperty.stringValue, true);

        /* Display a button with a 'Remove Item interactable' that when clicked removes
        * the target from the ConditionCollection's conditions array.
       */

        bool destoryMe = GUILayout.Button("Remove Item interactable", GUILayout.Width(removeuttonWidth));
     
        EditorGUILayout.EndHorizontal();

        // If the foldout is open show the expanded GUI.
        if (descriptionProperty.isExpanded)
        {
            ExpandedGUI();
        }
        EditorGUILayout.EndVertical();
        serializedObject.ApplyModifiedProperties();
        if (destoryMe)
        {

            for (int i = 0; i < reactionsEditors.Length; i++)
            {
                reactionsEditors[i].voidEdiorItemRemove();
            }
            for (int i = 0; i < conditionEditors.Length; i++)
            {
                conditionEditors[i].voidEdiorItemRemove();
            }
            // itemInteractableProperty.RemoveFromObjectArray(itemInteractabe);
            paranteEditor.RemoveItemInteractable(itemInteractabe);
        }
    }

    private void ExpandedGUI()
    { 
        EditorGUI.indentLevel++;

        EditorGUILayout.Space();

        // Display the description for editing.
        //EditorGUILayout.PropertyField(descriptionProperty);
        itemInteractabe.description = EditorGUILayout.TextField(itemInteractabe.description);

        EditorGUILayout.Space();
        // If new editors are required for Reactions and conditions, create them.
        serializedObject.Update();
        DrawConditions();
        serializedObject.ApplyModifiedProperties();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        serializedObject.Update();
        DrawReactions();
        serializedObject.ApplyModifiedProperties();
        EditorGUI.indentLevel--;
        
    }


    private void DrawConditions()
    {
        if (conditionEditors.Length != TryGetConditionsLength())
        {
            // Destroy all the old editors.
            for (int i = 0; i < conditionEditors.Length; i++)
            {
                DestroyImmediate(conditionEditors[i]);
            }

            // Create new editors.
            CreateConditionEditors();
        }

        EditorGUILayout.BeginVertical(GUI.skin.box);
        /* Display the Labels for the Conditions evenly split over
         * the width of the inspector.
         */
        float space = EditorGUIUtility.currentViewWidth / 3f;

        EditorGUI.indentLevel++;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Condition", GUILayout.Width(space));
        EditorGUILayout.LabelField("Satisfied?", GUILayout.Width(space));
        EditorGUILayout.LabelField("Add/Remove", GUILayout.Width(space));
        EditorGUILayout.EndHorizontal();

        // Display each of the Conditions.
        EditorGUILayout.BeginVertical(GUI.skin.box);
        for (int i = 0; i < conditionEditors.Length; i++)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            conditionEditors[i].OnInspectorGUI();
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndVertical();

        //-----------------------------------------------------------
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
        //-----------------------------------------------------------

        ConditionTypeSelectionGUI(leftAreaRect);
        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();

    }
    private void DrawReactions()
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;

        if (conditionEditors.Length != TryGetreactionLength())
        {
            // Destroy all the old editors.
            for (int i = 0; i < reactionsEditors.Length; i++)
            {
                DestroyImmediate(reactionsEditors[i]);
            }

            // Create new editors.
            CreateReactionEditors();
        }

        // Display all the Reactions.
        EditorGUILayout.BeginVertical(GUI.skin.box);
        for (int i = 0; i < reactionsEditors.Length; i++)
        {
             reactionsEditors[i].showReaction = true;// simple fix for that the fold out button stop to work
               reactionsEditors[i].OnInspectorGUI();
      
        }
        EditorGUILayout.EndVertical();

        // If there are Reactions, add a space.
        //if (itemInteractabe.itemInteractionReactions.Length > 0)
        //{
        //    EditorGUILayout.Space();
        //    EditorGUILayout.Space();
        //}

        //-----------------------------------------------------------
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
        //-----------------------------------------------------------


        // Display the GUI for the type popup and button on the left.
        ReactionTypeSelectionGUI(leftAreaRect);

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();


    }

    private void ConditionTypeSelectionGUI(Rect containingRect)
    {
        // Create Rects for the top and bottom half.
        Rect topHalf = containingRect;
        topHalf.height *= 0.5f;
        Rect bottomHalf = topHalf;
        bottomHalf.y += bottomHalf.height;

        // Display a popup in the top half showing all the reaction types.
        conditionSelectedIndex = EditorGUI.Popup(topHalf, conditionSelectedIndex, conditionTypeNames);

        // Display a button in the bottom half that if clicked...
        if (GUI.Button(bottomHalf, "Add Selected condition"))
        {
            // ... finds the type selected by the popup, creates an appropriate condition and adds it to the array.
            //Type conditionType = conditionTypes[conditionSelectedIndex];
            //SOBJ_ConditionAdvanced newCondition = EDI_ConditionAdvanced.CreateCondition(conditionType);
            //conditionsProperty.AddToObjectArray(newCondition);
            AddCondition();
          
        }
    }
    private void ReactionTypeSelectionGUI(Rect containingRect)
    {
        // Create Rects for the top and bottom half.
        Rect topHalf = containingRect;
        topHalf.height *= 0.5f;
        Rect bottomHalf = topHalf;
        bottomHalf.y += bottomHalf.height;

        // Display a popup in the top half showing all the reaction types.
        reactionSelectedIndex = EditorGUI.Popup(topHalf, reactionSelectedIndex, reactionTypeNames);

        // Display a button in the bottom half that if clicked...
        if (GUI.Button(bottomHalf, "Add Selected Reaction"))
        {
            // ... finds the type selected by the popup, creates an appropriate reaction and adds it to the array.

            // reactionsProperty.AddToObjectArray(newReaction);
            AddReaction();
           
        }
    }

    private void AddReaction()
    {
        Type reactionType         = reactionTypes[reactionSelectedIndex];
        SOBJ_Reaction newReaction = EDI_Reaction.CreateReaction(reactionType);

        newReaction.name = reactionType.Name;

        // Record all operations on the newConditions so they can be undone.
        Undo.RecordObject(newReaction, "Added SOBJ_Reaction");

        // Attach the Condition to the AllConditions asset.
        AssetDatabase.AddObjectToAsset(newReaction, itemInteractabe);

        //// Import the asset so it is recognised as a joined asset.
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(newReaction));

        //// Add the Condition to the AllConditions array.
        ArrayUtility.Add(ref itemInteractabe.itemInteractionReactions, newReaction);

        //// Mark the AllConditions asset as dirty so the editor knows to save changes to it when a project save happens.
        EditorUtility.SetDirty(itemInteractabe);
        CreateReactionEditors();
    }
    public void RemoveReaction(SOBJ_Reaction reaction)
    {

        /* Record all operations on the AllConditions 
         * asset so they can be undone.
         */
        Undo.RecordObject(itemInteractabe, "Removing SOBJ_Reaction");

        // Remove the specified condition from the AllConditions array.
        ArrayUtility.Remove(ref itemInteractabe.itemInteractionReactions, reaction);

        /* Destroy the condition, including it's asset and 
         * save the assets to recognise the change.
         */
        DestroyImmediate(reaction, true);

        AssetDatabase.SaveAssets();

        /* Mark the AllConditions asset as dirty so the editor 
         * knows to save changes to it when a project save happens.
         */
        EditorUtility.SetDirty(itemInteractabe);
        CreateReactionEditors();
    }

    private void AddCondition()
    {
        Type conditionType                  = conditionTypes[conditionSelectedIndex];
        SOBJ_ConditionAdvanced newCondition = EDI_ConditionAdvanced.CreateCondition(conditionType);
        
        newCondition.name = conditionType.Name;

        // Record all operations on the newConditions so they can be undone.
        Undo.RecordObject(newCondition, "Added SOBJ_ConditionAdvanced");

        // Attach the Condition to the AllConditions asset.
        AssetDatabase.AddObjectToAsset(newCondition, itemInteractabe);

        //// Import the asset so it is recognised as a joined asset.
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(newCondition));

        //// Add the Condition to the AllConditions array.
        ArrayUtility.Add(ref itemInteractabe.requiredConditions, newCondition);

        //// Mark the AllConditions asset as dirty so the editor knows to save changes to it when a project save happens.
        EditorUtility.SetDirty(itemInteractabe);
        CreateConditionEditors();
    }
    public void RemoveCondition(SOBJ_ConditionAdvanced condition)
    {

        /* Record all operations on the AllConditions 
         * asset so they can be undone.
         */
        Undo.RecordObject(itemInteractabe, "Removing SOBJ_ConditionAdvanced");

        // Remove the specified condition from the AllConditions array.
        ArrayUtility.Remove(ref itemInteractabe.requiredConditions, condition);

        /* Destroy the condition, including it's asset and 
         * save the assets to recognise the change.
         */
        //clean u
        if (conditionEditors != null)
        {
            // Destroy all the old editors.
            for (int i = 0; i < conditionEditors.Length; i++)
            {
                DestroyImmediate(conditionEditors[i]);
            }
        }
        DestroyImmediate(condition, true);

        AssetDatabase.SaveAssets();
     
        /* Mark the AllConditions asset as dirty so the editor 
         * knows to save changes to it when a project save happens.
         */
        EditorUtility.SetDirty(itemInteractabe);
        CreateReactionEditors();

    }


    private void CreateConditionEditors()
    {
       

        // Create a new array for the editors which is the same length at the conditions array.
        conditionEditors = new EDI_ConditionAdvanced[itemInteractabe.requiredConditions.Length];
    
        // Go through all the empty array...
        for (int i = 0; i < conditionEditors.Length; i++)
        {
            // ... and create an editor with an editor type to display correctly.
            conditionEditors[i]                    = CreateEditor(TryGetConditionAt(i)) as EDI_ConditionAdvanced;
            conditionEditors[i].editorType         = EDI_ConditionAdvanced.EditorType.ConditionCollection;
            conditionEditors[i].conditionsProperty = conditionsProperty;
            conditionEditors[i].parentEditor       = this;
        }

    }
    private void CreateReactionEditors()
    {
        // Create a new array for the editors which is the same length at the conditions array.
        reactionsEditors = new EDI_Reaction[itemInteractabe.itemInteractionReactions.Length];

        // Go through all the empty array...
        for (int i = 0; i < reactionsEditors.Length; i++)
        {
            // ... and create an editor with an editor type to display correctly.
            reactionsEditors[i]                   = CreateEditor(TryGetReactionAt(i)) as EDI_Reaction;
            reactionsEditors[i].reactionsProperty = reactionsProperty;
            reactionsEditors[i].parentEditor      = this;
        }
    }
    
    
    /// <summary>
    /// Attemts to get length of conditions array
    /// </summary>
    /// <returns>returns 0 if array is null elles
    /// retuns the length</returns>
    private int TryGetConditionsLength()
    {
        // If there is no Conditions array, return a length of 0.
        if (itemInteractabe.requiredConditions == null)
        {
            return 0;
        }


        // Otherwise return the length of the array.
        return itemInteractabe.requiredConditions.Length;
    }
    /// <summary>
    /// Attemts to get length of reactions array
    /// </summary>
    /// <returns>returns 0 if array is null elles
    /// retuns the length</returns>
    private int TryGetreactionLength()
    {
        // If there is no Conditions array, return a length of 0.
        if (itemInteractabe.itemInteractionReactions == null)
        {
            return 0;
        }


        // Otherwise return the length of the array.
        return itemInteractabe.itemInteractionReactions.Length;
    }
    
    
    /// <summary>
    /// gets the reactions index in the condition array
    /// returns -1 if it wastnt found
    /// </summary>
    /// <param name="condition"> the condition whos index we want</param>
    /// <returns></returns>
    private int TryGetConditionIndex(SOBJ_ConditionAdvanced condition)
    {
        // Go through all the Conditions...
        for (int i = 0; i < TryGetConditionsLength(); i++)
        {
            // ... and if one matches the given Condition, return its index.
            if (TryGetConditionAt(i).hash == condition.hash)
            {
                return i;
            }

        }

        // If the Condition wasn't found, return -1.
        return -1;
    }
    /// <summary>
    /// gets the reactions index in the reactions array
    /// returns -1 if it wastnt found
    /// </summary>
    /// <param name="reaction"> the reaction whos index we want</param>
    /// <returns></returns>
    private int TryGetReactionIndex(SOBJ_Reaction reaction)
    {
        // Go through all the Conditions...
        for (int i = 0; i < TryGetConditionsLength(); i++)
        {
            // ... and if one matches the given Condition, return its index.
            if (TryGetReactionAt(i).getHash == reaction.getHash)
            {
                return i;
            }

        }

        // If the Condition wasn't found, return -1.
        return -1;
    }


    /// <summary>
    /// Atmtes to get condition, 
    /// </summary>
    /// <param name="index"> position in array</param>
    /// <returns>returns null the list is emty,
    /// returns the first ellement if index is greter then the 
    /// length of the array</returns>
    private SOBJ_ConditionAdvanced TryGetConditionAt(int index)
    {
        // Cache the AllConditions array.
        SOBJ_ConditionAdvanced[] conditions = itemInteractabe.requiredConditions;

        // If it doesn't exist or there are null elements, return null.
        if (conditions == null || conditions[0] == null)
        {
            return null;
        }

        // If the given index is beyond the length of the array return the first element.
        if (index >= conditions.Length)
        {
            return conditions[0];
        }


        // Otherwise return the Condition at the given index.
        return conditions[index];
    }
    /// <summary>
    /// Atmtes to get condition, 
    /// </summary>
    /// <param name="index"> position in array</param>
    /// <returns>returns null the list is emty,
    /// returns the first ellement if index is greter then the 
    /// length of the array</returns>
    private SOBJ_Reaction TryGetReactionAt(int index)
    {
        // Cache the AllConditions array.
        SOBJ_Reaction[] reactions = itemInteractabe.itemInteractionReactions;

        // If it doesn't exist or there are null elements, return null.
        if (reactions == null || reactions[0] == null)
        {
            return null;
        }

        // If the given index is beyond the length of the array return the first element.
        if (index >= reactions.Length)
        {
            return reactions[0];
        }


        // Otherwise return the Condition at the given index.
        return reactions[index];
    }
    



}
