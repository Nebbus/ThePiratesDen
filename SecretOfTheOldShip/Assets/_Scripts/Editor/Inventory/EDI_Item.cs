using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;


[CustomEditor(typeof(SOBJ_Item))]
[CanEditMultipleObjects]
public class EDI_Item : Editor
{
    public enum CurentInteractions { HOVER, CLICK };

    //The target item
    private SOBJ_Item sobjItem;

    private SerializedProperty spriteProperty;
    private SerializedProperty onHowerTextProperty;
    private SerializedProperty onClickConditioAndReactionsProperty;
    private SerializedProperty onHovorConditioAndReactionsProperty;
    private SerializedProperty putDownSoundProperty;


    private const string spritePropertyName                      = "sprite";
    private const string onHowerTextPropertyName                  = "onHowerText";
    private const string onClickConditioAndReactionsPropertyName = "onCLickConditionAndReactions";
    private const string onHovorConditioAndReactionsPropertyName = "onHoverConditionAndReactions";
    private const string putDownSoundPropertyName                = "putDownSound";

    private EDI_ItemInteractable[] onClickItemInteractablesEditors;   // the list of interactions
    private EDI_ItemInteractable[] onHoverItemInteractablesEditors;   // the list of interactions

    private bool showOnClickEditor = false;
  //  private bool showOnHoverEditor = false;

    private const float collectionButtonWidth = 180f;  // Width in pixels of the button for adding to the conditioAndReactions array.             
    private const float ownSpriteWidth        = 1 / 3f;
    private const float ownSpriteHight        = 1 / 3f;
    private const float spacing               = 1 / 5f;

    private void OnEnable()
    {

        sobjItem = (SOBJ_Item)target;

        saftyCache();
        
        onClickConditioAndReactionsProperty = serializedObject.FindProperty(onClickConditioAndReactionsPropertyName);
        onHovorConditioAndReactionsProperty = serializedObject.FindProperty(onHovorConditioAndReactionsPropertyName);
        
        spriteProperty               = serializedObject.FindProperty(spritePropertyName);
        onHowerTextProperty          = serializedObject.FindProperty(onHowerTextPropertyName);
        putDownSoundProperty         = serializedObject.FindProperty(putDownSoundPropertyName);

    }

    /// <summary>
    /// Safty catches to avoid errors
    /// </summary>
    private void saftyCache()
    {
        /* If there aren't any Conditions on the target, 
      * create an empty array of Conditions.
      */
        if (sobjItem.onClickConditionAndReactions == null)
        {
            sobjItem.onClickConditionAndReactions = new SOBJ_ItemInteractable[0];
        }

        if (sobjItem.onHoverConditionAndReactions == null)
        {
            sobjItem.onHoverConditionAndReactions = new SOBJ_ItemInteractable[0];
        }


        // If there aren't any editors, create them.
        if (onClickItemInteractablesEditors == null)
        {
            CreateEditors(CurentInteractions.CLICK);
        }

        // If there aren't any editors, create them.
        if (onHoverItemInteractablesEditors == null)
        {
            CreateEditors(CurentInteractions.HOVER);
        }
    }



    private void OnDisable()
    {
        if (onClickItemInteractablesEditors != null)
        {
            // Destroy all the editors.
            for (int i = 0; i < onClickItemInteractablesEditors.Length; i++)
            {
                DestroyImmediate(onClickItemInteractablesEditors[i]);
            }

            // Null out the editor array.
            onClickItemInteractablesEditors = null;
        }

        if (onHoverItemInteractablesEditors != null)
        {
            // Destroy all the editors.
            for (int i = 0; i < onHoverItemInteractablesEditors.Length; i++)
            {
                DestroyImmediate(onHoverItemInteractablesEditors[i]);
            }

            // Null out the editor array.
            onHoverItemInteractablesEditors = null;
        }

  

    }

    public override void OnInspectorGUI()
    {
       // base.DrawDefaultInspector();
        CurentItem(EditorGUIUtility.currentViewWidth);

      

        EditorGUILayout.HelpBox("The itemInteractabel will be"+
                                 "tested from top to bottom, if a reaction happesn " +
                                 "will it stopp testing. So the order of them is important.",MessageType.Info);
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUI.indentLevel++;
            showOnClickEditor = EditorGUILayout.Foldout(showOnClickEditor, "On click interactions", true);
            if (showOnClickEditor)
            {
                DrawInteractableCollections(CurentInteractions.CLICK);
          
            }
            EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

  //if reactions is wanted for on hover, un coment 
        //EditorGUILayout.BeginVertical(GUI.skin.box);
        //    EditorGUI.indentLevel++;
        //    showOnHoverEditor = EditorGUILayout.Foldout(showOnHoverEditor, "On hover interactions", true);
        //    if (showOnHoverEditor)
        //    {
        //        DrawInteractableCollections(CurentInteractions.HOVER);
        //        EditorGUILayout.Space();
        //    }

        //    EditorGUI.indentLevel--;
        //EditorGUILayout.EndVertical();
    }


    /// <summary>
    /// Displays the property fieald for the selection 
    /// of sprite to represent this item, also displays 
    /// the sprite
    /// </summary>
    /// <param name="width"> width of the inventory box</param>
    private void CurentItem(float width)
    {
        serializedObject.Update();
        EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.PropertyField(spriteProperty, GUIContent.none,GUILayout.Width(width * ownSpriteWidth));
            if (sobjItem.sprite != null)
            {
                GUILayout.Box(sobjItem.sprite.texture, GUILayout.Width(width * ownSpriteWidth), GUILayout.Height(width * ownSpriteHight));
            }
        EditorGUILayout.EndVertical();
        EditorGUILayout.PropertyField(onHowerTextProperty);
        EditorGUILayout.PropertyField(putDownSoundProperty);
        serializedObject.ApplyModifiedProperties();
    }

    /// <summary>
    /// Displays teh editors for the SOBJ_intemInteractalbes
    /// </summary>
    private void DrawInteractableCollections(CurentInteractions type)
    {
        serializedObject.Update();
        EditorGUILayout.BeginVertical();

            uppdateEditorArray(type);
            runThrougTheEditors(type);

            /* Create a right-aligned button which when clicked,
            * creates a new ConditionCollection in the
            * ConditionCollections array.
            */
            EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Add "+type+" Item interactable", GUILayout.Width(collectionButtonWidth)))
                {
                    int number = (type == CurentInteractions.CLICK) ? sobjItem.onClickConditionAndReactions.Length : sobjItem.onHoverConditionAndReactions.Length;
                    AddItemInteractable("ItemInteraction" + number, type);
                }

            EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
        serializedObject.ApplyModifiedProperties();
    }

    /// <summary>
    /// run throug all the edditors
    /// </summary>
    /// <param name="type"></param>
    private void uppdateEditorArray(CurentInteractions type)
    {
        if (type == CurentInteractions.CLICK)
        {
            // Updates the SOBJ_itemInteractabel editors array
            if (onClickItemInteractablesEditors.Length != TryGetItemInteractionsLength(type))
            {
                // Destroy all the old editors.
                for (int i = 0; i < onClickItemInteractablesEditors.Length; i++)
                {
                    DestroyImmediate(onClickItemInteractablesEditors[i]);
                }

                // Create new editors.
                CreateEditors(type);
            }
        }
        else
        {
            // Updates the SOBJ_itemInteractabel editors array
            if (onHoverItemInteractablesEditors.Length != TryGetItemInteractionsLength(type))
            {
                // Destroy all the old editors.
                for (int i = 0; i < onHoverItemInteractablesEditors.Length; i++)
                {
                    DestroyImmediate(onHoverItemInteractablesEditors[i]);
                }

                // Create new editors.
                CreateEditors(type);
            }
        }
    }
    
    /// <summary>
    /// run throug all the edditors
    /// </summary>
    /// <param name="type"></param>
    private void runThrougTheEditors(CurentInteractions type)
    {
        if (type == CurentInteractions.CLICK)
        {
            // Display all of the itemInteractablesEditors.
            for (int i = 0; i < onClickItemInteractablesEditors.Length; i++)
            {
                onClickItemInteractablesEditors[i].OnInspectorGUI();
                EditorGUILayout.Space();
            }
        }
        else
        {
            // Display all of the itemInteractablesEditors.
            for (int i = 0; i < onHoverItemInteractablesEditors.Length; i++)
            {
                onHoverItemInteractablesEditors[i].OnInspectorGUI();
                EditorGUILayout.Space();
            }
        }
    }


    /// <summary>
    /// Caches the itemInteractavles editors
    /// </summary>
    private void CreateEditors(CurentInteractions type)
    {
        if (type == CurentInteractions.CLICK)
        {
            // Create a new array for the editors which is the same length at the conditions array.
            onClickItemInteractablesEditors = new EDI_ItemInteractable[sobjItem.onClickConditionAndReactions.Length];

            // Go through all the empty array...
            for (int i = 0; i < onClickItemInteractablesEditors.Length; i++)
            {
                // ... and create an editor with an editor type to display correctly.
                onClickItemInteractablesEditors[i]                          = CreateEditor(TryGetItemInteractionAt(i, type)) as EDI_ItemInteractable;
                onClickItemInteractablesEditors[i].itemInteractableProperty = onClickConditioAndReactionsProperty;
                onClickItemInteractablesEditors[i].paranteEditor            = this;
                onClickItemInteractablesEditors[i].type                     = type;
            }
        }
        else
        {
            // Create a new array for the editors which is the same length at the conditions array.
            onHoverItemInteractablesEditors = new EDI_ItemInteractable[sobjItem.onHoverConditionAndReactions.Length];

            // Go through all the empty array...
            for (int i = 0; i < onHoverItemInteractablesEditors.Length; i++)
            {
                // ... and create an editor with an editor type to display correctly.
                onHoverItemInteractablesEditors[i]                          = CreateEditor(TryGetItemInteractionAt(i, type)) as EDI_ItemInteractable;
                onHoverItemInteractablesEditors[i].itemInteractableProperty = onHovorConditioAndReactionsProperty;
                onHoverItemInteractablesEditors[i].paranteEditor            = this;
                onHoverItemInteractablesEditors[i].type                     = type;
            }
        }

    }

    /// <summary>
    /// Creates and adds a "conditioncollection" 
    /// </summary>
    /// <param name="description"> the name to give the new itemInteraction</param>
    private void AddItemInteractable(string description, CurentInteractions type)
    {

        // Create a condition based on the description.
        SOBJ_ItemInteractable newInteractable = CreateInstance<SOBJ_ItemInteractable>();
        
        // The name is what is displayed by the asset so set that too.
        newInteractable.name        = description;
        newInteractable.description = "New interaction";

        // Record all operations on the newConditions so they can be undone.
        Undo.RecordObject(newInteractable, "Created new SOBJ_ItemInteractable");

        // Attach the Condition to the AllConditions asset.
        AssetDatabase.AddObjectToAsset(newInteractable, sobjItem);
        AssetDatabase.GetDependencies(AssetDatabase.GetAssetPath(sobjItem));

        // Import the asset so it is recognised as a joined asset.
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(newInteractable));

        if (type == CurentInteractions.CLICK)
        {
            // Add the Condition to the AllConditions array.
            ArrayUtility.Add(ref sobjItem.onClickConditionAndReactions, newInteractable);
        }
        else
        {
            // Add the Condition to the AllConditions array.
            ArrayUtility.Add(ref sobjItem.onHoverConditionAndReactions, newInteractable);
        }


        /*Mark the AllConditions asset as dirty so
         * the editor knows to save changes to it 
         * when a project save happens.
         */ 
        EditorUtility.SetDirty(sobjItem);
    }

    /// <summary>
    /// Deletets and Removes a itemIteractrable
    /// </summary>
    /// <param name="itemInteraction"> the thing to remove</param>
    public void RemoveItemInteractable(SOBJ_ItemInteractable itemInteraction, CurentInteractions type)
    {
      

        /* Record all operations on the AllConditions 
         * asset so they can be undone.
         */
        Undo.RecordObject(sobjItem, "Removing SOBJ_ItemInteractable");

        if(type == CurentInteractions.CLICK)
        {
            // Remove the specified condition from the on click interaction array.
            ArrayUtility.Remove(ref sobjItem.onClickConditionAndReactions, itemInteraction);
        }
        else
        {
            // Remove the specified condition from the on hover interaction array.
            ArrayUtility.Remove(ref sobjItem.onHoverConditionAndReactions, itemInteraction);
        }


        /* Destroy the condition, including it's asset and 
         * save the assets to recognise the change.
         */
        DestroyImmediate(itemInteraction, true);
        AssetDatabase.SaveAssets();

        /* Mark the AllConditions asset as dirty so the editor 
         * knows to save changes to it when a project save happens.
         */
        EditorUtility.SetDirty(sobjItem);

    }

    /// <summary>
    /// Attemts to get the itemInteractable from teh array
    /// </summary>
    /// <param name="index"> the index of the item to get</param>
    /// <returns>null: the array or item was null | 
    /// returns the first object in the list if it was wanted 
    /// or if the indix thas greter then the arrays length
    /// | else return the wanted indez</returns>
    private SOBJ_ItemInteractable TryGetItemInteractionAt(int index, CurentInteractions type)
    {
      
        // Cache the AllConditions array.
        SOBJ_ItemInteractable[] itemInteractions = (type == CurentInteractions.CLICK) ? sobjItem.onClickConditionAndReactions : sobjItem.onHoverConditionAndReactions;


        // If it doesn't exist or there are null elements, return null.
        if (itemInteractions == null || itemInteractions[0] == null)
        {
            return null;
        }

        // If the given index is beyond the length of the array return the first element.
        if (index >= itemInteractions.Length)
        {
            return itemInteractions[0];
        }


        // Otherwise return the Condition at the given index.
        return itemInteractions[index];

    }

    /// <summary>
    /// Attemts to the the length of the 
    /// itemInteractables array
    /// </summary>
    /// <returns>0: if array is null ellers return length</returns>
    private int TryGetItemInteractionsLength(CurentInteractions type)
    {
        if(type == CurentInteractions.CLICK)
        {
            // If there is no Conditions array, return a length of 0.
            if (sobjItem.onClickConditionAndReactions == null)
            {
                return 0;
            }


            // Otherwise return the length of the array.
            return sobjItem.onClickConditionAndReactions.Length;
        }
        else
        { 
            // If there is no Conditions array, return a length of 0.
            if (sobjItem.onHoverConditionAndReactions == null)
            {
                return 0;
            }


            // Otherwise return the length of the array.
            return sobjItem.onHoverConditionAndReactions.Length;

        }
       
    }


}
