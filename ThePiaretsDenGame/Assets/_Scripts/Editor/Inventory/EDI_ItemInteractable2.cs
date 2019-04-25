using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(SOBJ_ItemInteractable))]
public class EDI_ItemInteractable2 : Editor
{


    public SerializedProperty itemInteractableProperty;     // Represents the array of MONO_ItemInteractable that the target belongs to.  

    public EDI_Item2 paranteEditor;                         // Referens to the parent so this can be removed
    public EDI_Item2.CurentInteractions type;               // if this is hover or click interactable so this can be removed

    private SOBJ_ItemInteractable itemInteractabe;          // Reference to the target.

    private SerializedProperty descriptionProperty;         // Represents a string description for the target.
    private SerializedProperty reactionsProperty;           // Represents the array of Reactions.
    private SerializedProperty conditionsProperty;          // Represents an array of Conditions for the target.


    // Arrays that contains the objects edditors.
    private EDI_ConditionAdvanced[] conditionEditors;
    private EDI_Reaction[]          reactionsEditors;
    private bool[]                  reactionShowCash;


    // reactions variables
    private Type[]   reactionTypes;                         // All the non-abstract types which inherit from Reaction.  This is used for adding new Reactions.
    private int      reactionSelectedIndex;                 // The index of the currently selected Reaction type.  
    private string[] reactionTypeNames;                     // The names of all appropriate Reaction types.

    // condition variables
    private Type[]   conditionTypes;                        // All the non-abstract types which inherit from SOBJ_ConditionAdvanced.  This is used for adding new Reactions.
    private int      conditionSelectedIndex;                // The index of the currently selected SOBJ_ConditionAdvanced type.
    private string[] conditionTypeNames;                    // The names of all appropriate SOBJ_ConditionAdvanced types.

    private const float dropAreaHeight    = 50f;           // Height in pixels of the area for dropping scripts.
    private const float controlSpacing    = 5f;            // Width in pixels between the popup type selection and drop area.
    private const float removeButtonWidth = 170f;

    //Names of the variables in SOBJ_ItemInteractable2
    private const string reactionsPropName   = "itemInteractionReactions";
    private const string conditionsPropName  = "requiredConditions";
    private const string descriptionPropName = "description";

    // Caching the vertical spacing between GUI elements.
    private readonly float verticalSpacing = EditorGUIUtility.standardVerticalSpacing;

    public void OnEnable()
    {
        // Cache the target.
        itemInteractabe = (SOBJ_ItemInteractable)target;

        SaftetyCatch();

        // Cache the SerializedProperty
        reactionsProperty   = serializedObject.FindProperty(reactionsPropName);
        descriptionProperty = serializedObject.FindProperty(descriptionPropName);
        conditionsProperty  = serializedObject.FindProperty(conditionsPropName);

        //Set the array of types and type names of subtypes of Reaction and condition.
        EXT_GetListOfScriptableObjects.SetGenericNamesArray(typeof(SOBJ_Reaction),          out reactionTypes,  out reactionTypeNames);
        EXT_GetListOfScriptableObjects.SetGenericNamesArray(typeof(SOBJ_ConditionAdvanced), out conditionTypes, out conditionTypeNames);
    }

    /// <summary>
    /// Saftety catch to avoid errors
    /// </summary>
    private void SaftetyCatch()
    {
       
        if (target == null)
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
        if (reactionShowCash == null)
        {
            reactionShowCash = new bool[itemInteractabe.itemInteractionReactions.Length];
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
     

    }


    private void OnDisable()
    {
        if(conditionEditors != null)
        {
            // Destroy all the editors.
            for (int i = 0; i < conditionEditors.Length; i++)
            {
                DestroyImmediate(conditionEditors[i]);
            }
            conditionEditors = null;
        }
       
        if(reactionsEditors != null)
        {
            for (int i = 0; i < reactionsEditors.Length; i++)
            {
                DestroyImmediate(reactionsEditors[i]);
            }
            reactionsEditors = null;

        }

    }

    public override void OnInspectorGUI()
    {
      // base.OnInspectorGUI();

        serializedObject.Update();
        EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUI.indentLevel++;
            EditorGUILayout.BeginHorizontal();
                /* Use the isExpanded bool for the descriptionProperty 
                 * to store whether the foldout is open or closed.
                 */
                descriptionProperty.isExpanded = EditorGUILayout.Foldout(descriptionProperty.isExpanded, descriptionProperty.stringValue, true);

                /* Display a button with a 'Remove Item interactable' that when clicked removes
                 * the target from the ConditionCollection's conditions array.
                 */

                bool destoryMe = GUILayout.Button("Remove Item interactable", GUILayout.Width(removeButtonWidth));

            EditorGUILayout.EndHorizontal();

            // If the foldout is open show the expanded GUI.
            if (descriptionProperty.isExpanded)
            {
                ExpandedGUI();
            }
            EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
        serializedObject.ApplyModifiedProperties();

        /*Destroys the interactable object 
         * and all underlaying reactions
         * and conditions
         */
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
            paranteEditor.RemoveItemInteractable(itemInteractabe, type);
        }
    }

    /// <summary>
    /// The itemInteraction Editor
    /// </summary>
    private void ExpandedGUI()
    { 
        EditorGUI.indentLevel++;

        EditorGUILayout.Space();

        // Display the description for editing.
        itemInteractabe.description = EditorGUILayout.TextField(itemInteractabe.description);

        EditorGUILayout.Space();

        DrawConditions();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        DrawReactions();

        EditorGUI.indentLevel--;   
    }

    /// <summary>
    /// The conditons collection editor
    /// </summary>
    private void DrawConditions()
    {
        serializedObject.Update();
        EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUI.indentLevel++;
            //Uppdates teh conditions editors array
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
        serializedObject.ApplyModifiedProperties();
    }

    /// <summary>
    /// The editor for the reaction
    /// collection
    /// </summary>
    private void DrawReactions()
    {
        serializedObject.Update();
        EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUI.indentLevel++;

       

        //Uppdaes the condition editor array
        if (reactionShowCash.Length != TryGetreactionLength() || reactionShowCash.Length != TryGetreactionLength())
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
        for (int i = 0; i < reactionsEditors.Length; i++)
        {
            // reactionsEditors[i].showReaction = true;// simple fix for that the fold out button stop to work
            reactionShowCash[i] = reactionsEditors[i].OnItemInteractionGui(reactionShowCash[i]);
      
         }


            // If there are Reactions, add a space.
            if (itemInteractabe.itemInteractionReactions.Length > 0)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Space();
            }

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
        serializedObject.ApplyModifiedProperties();
    }


    /// <summary>
    /// The drop dow menu and button
    /// for adding new conditions
    /// </summary>
    /// <param name="containingRect"> the rectangle that is
    /// just to contin dropdow menu and button  </param>
    private void ConditionTypeSelectionGUI(Rect containingRect)
    {
        // Create Rects for the top and bottom half.
        Rect topHalf     = containingRect;
        topHalf.height  *= 0.5f;
        Rect bottomHalf  = topHalf;
        bottomHalf.y    += bottomHalf.height;

        // Display a popup in the top half showing all the reaction types.
        conditionSelectedIndex = EditorGUI.Popup(topHalf, conditionSelectedIndex, conditionTypeNames);

        // Display a button in the bottom half that if clicked...
        if (GUI.Button(bottomHalf, "Add Selected condition"))
        {
            AddCondition();

        }
    }

    /// <summary>
    /// The drop dow menu and button
    /// for adding new reactions
    /// </summary>
    /// <param name="containingRect"> the rectangle that is
    /// just to contin dropdow menu and button  </param>
    private void ReactionTypeSelectionGUI(Rect containingRect)
    {
        // Create Rects for the top and bottom half.
        Rect topHalf    = containingRect;
        topHalf.height *= 0.5f;
        Rect bottomHalf = topHalf;
        bottomHalf.y    += bottomHalf.height;

        // Display a popup in the top half showing all the reaction types.
        reactionSelectedIndex = EditorGUI.Popup(topHalf, reactionSelectedIndex, reactionTypeNames);

        // Display a button in the bottom half that if clicked...
        if (GUI.Button(bottomHalf, "Add Selected Reaction"))
        {
            AddReaction(); 
        }
    }

    /// <summary>
    /// Creatse the new reactio and adds it 
    /// to the SOBJ_item asset
    /// </summary>
    private void AddReaction()
    {
        //creat reaction ands sets its name
        Type reactionType         = reactionTypes[reactionSelectedIndex];
        SOBJ_Reaction newReaction = EDI_Reaction.CreateReaction(reactionType);
        newReaction.name          = reactionType.Name;

        // Record all operations on the newConditions so they can be undone.
        Undo.RecordObject(newReaction, "Added SOBJ_Reaction");

        // Attach the Condition to the AllConditions asset.
        AssetDatabase.AddObjectToAsset(newReaction, itemInteractabe);
       

        // Import the asset so it is recognised as a joined asset.
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(newReaction));

        // Add the Condition to the AllConditions array.
        ArrayUtility.Add(ref itemInteractabe.itemInteractionReactions, newReaction);

        // Mark the AllConditions asset as dirty so the editor knows to save changes to it when a project save happens.
        EditorUtility.SetDirty(itemInteractabe);

        //uppdats the reation editor arrays
        CreateReactionEditors();
    }

    /// <summary>
    /// Funktion that removes/delets a reaction
    /// </summary>
    /// <reaction>Reaction to be removed</reaction>
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


    /// <summary>
    /// Creatse the new condition and adds it 
    /// to the SOBJ_item asset
    /// </summary>
    private void AddCondition()
    {
        // Ceats a the conditon and sets its name
        Type conditionType                  = conditionTypes[conditionSelectedIndex];
        SOBJ_ConditionAdvanced newCondition = EDI_ConditionAdvanced.CreateCondition(conditionType);     
        newCondition.name                   = conditionType.Name; //Dosent hav controll over the naming since this shuold work as conditonCollection 

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

    /// <summary>
    /// Function that removs and delets a conditon
    /// </summary>
    /// <param name="condition"> condition to be removed</param>
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
        DestroyImmediate(condition, true);
        AssetDatabase.SaveAssets();
     
        /* Mark the AllConditions asset as dirty so the editor 
         * knows to save changes to it when a project save happens.
         */
        EditorUtility.SetDirty(itemInteractabe);

        // Updates the reaction editor array
        CreateReactionEditors();

    }

    /// <summary>
    /// Chaches the ediors for all conditions
    /// </summary>
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

    /// <summary>
    /// Chaches the ediors for all reactions
    /// </summary>
    private void CreateReactionEditors()
    {
        // Create a new array for the editors which is the same length at the conditions array.
        reactionsEditors = new EDI_Reaction[itemInteractabe.itemInteractionReactions.Length];
        bool[] temp      = new bool[itemInteractabe.itemInteractionReactions.Length];

        if (reactionShowCash.Length < temp.Length)
        {
            for (int i = 0; i < reactionShowCash.Length; i++)
            {
                temp[i] = reactionShowCash[i];
            }
        }
        else if(reactionShowCash.Length > temp.Length)
        {
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] = reactionShowCash[i];
            }
        }
        else
        {
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] = reactionShowCash[i];
            }
        }
        reactionShowCash = temp;


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
    /// </summary>
    /// <param name="condition"> the condition whos index we want</param>
    /// <returns>returns -1 if it wasent found</returns>
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
    /// </summary>
    /// <param name="reaction"> the reaction whos index we want</param>
    /// <returns>returns -1 if it wasent found</returns>
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
