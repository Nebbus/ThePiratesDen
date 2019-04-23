using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;


[CustomEditor(typeof(SOBJ_Item))]
[CanEditMultipleObjects]
public class EDI_Item2 : Editor
{

    //The target item

    private SerializedProperty spriteProperty;
    private SerializedProperty conditioAndReactionsProperty;

    private const string spritePropertyName = "sprite";
    private const string conditioAndReactionsPropertyName = "conditioAndReactions";




    private SOBJ_Item sobjItem;

    private EDI_ItemInteractable2[] itemInteractablesEditors;

    private const float collectionButtonWidth = 170f;  // Width in pixels of the button for adding to the conditioAndReactions array.             
    private const float ownSpriteWidth        = 1 / 3f;
    private const float ownSpriteHight        = 1 / 3f;
    private const float spacing               = 1 / 5f;

    private void OnEnable()
    {

        sobjItem = (SOBJ_Item)target;

        conditioAndReactionsProperty = serializedObject.FindProperty(conditioAndReactionsPropertyName);
        spriteProperty = serializedObject.FindProperty(spritePropertyName);

        /* If there aren't any Conditions on the target, 
       * create an empty array of Conditions.
       */
        if (sobjItem.conditioAndReactions == null)
        {
            sobjItem.conditioAndReactions = new SOBJ_ItemInteractable[0];
        }


        // If there aren't any editors, create them.
        if (itemInteractablesEditors == null)
        {
            CreateEditors();
        }

    }
    private void CreateEditors()
    {
        // Create a new array for the editors which is the same length at the conditions array.
        itemInteractablesEditors = new EDI_ItemInteractable2[sobjItem.conditioAndReactions.Length];

        // Go through all the empty array...
        for (int i = 0; i < itemInteractablesEditors.Length; i++)
        {
            // ... and create an editor with an editor type to display correctly.
            itemInteractablesEditors[i] = CreateEditor(TryGetItemInteractionAt(i)) as EDI_ItemInteractable2;
            itemInteractablesEditors[i].itemInteractableProperty = conditioAndReactionsProperty;
            itemInteractablesEditors[i].paranteEditor = this;
        }
    }
  

    private void OnDisable()
    {
        // Destroy all the editors.
        for (int i = 0; i < itemInteractablesEditors.Length; i++)
        {
            DestroyImmediate(itemInteractablesEditors[i]);
        }

        // Null out the editor array.
        itemInteractablesEditors = null;

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        //base.OnInspectorGUI();
        CurentItem(EditorGUIUtility.currentViewWidth);
        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
        DrawInteractableCollections();
        serializedObject.ApplyModifiedProperties();

    }


    private void DrawInteractableCollections()
    { 
        
        
        // Pull information from the target into the serializedObject.
        serializedObject.Update();
        if (itemInteractablesEditors.Length != TryGetItemInteractionsLength())
        {
            // Destroy all the old editors.
            for (int i = 0; i < itemInteractablesEditors.Length; i++)
            {
                DestroyImmediate(itemInteractablesEditors[i]);
            }

            // Create new editors.
            CreateEditors();
        }



        // Display all of the itemInteractablesEditors.
        for (int i = 0; i < itemInteractablesEditors.Length; i++)
        {
            itemInteractablesEditors[i].OnInspectorGUI();
            EditorGUILayout.Space();
        }

        /* Create a right-aligned button which when clicked,
         * creates a new ConditionCollection in the
         * ConditionCollections array.
         */
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Add Item interactable", GUILayout.Width(collectionButtonWidth)))
        {

            AddItemInteractable("ItemInteraction" + sobjItem.conditioAndReactions.Length);
            //conditioAndReactionsProperty.AddToObjectArray(newCollection);
        }
        EditorGUILayout.EndHorizontal();

        /* Push information back to the target from the
         * serializedObject.
         */
        serializedObject.ApplyModifiedProperties();
    }



    /// <summary>
    /// draws the image of that will reptrsetn 
    /// this item in the inventory
    /// </summary>
    /// <param name="width"> width of the inventory box</param>
    private void CurentItem(float width)
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);


        EditorGUILayout.PropertyField(spriteProperty, GUIContent.none,
                                                      GUILayout.Width(width * ownSpriteWidth));
        if (sobjItem.sprite != null)
        {
            //  GUILayout.Box(sobjItem.sprite.texture, GUILayout.Width(width), GUILayout.Height(width * ownSpriteHight));
            GUILayout.Box(sobjItem.sprite.texture, GUILayout.Width(width * ownSpriteWidth),
                                                   GUILayout.Height(width * ownSpriteHight));
        }

        EditorGUILayout.EndVertical();
    }

    private void AddItemInteractable(string description)
    {

        // Create a condition based on the description.
        SOBJ_ItemInteractable newCondition = CreateInstance<SOBJ_ItemInteractable>();
        // SOBJ_ConditionAdvanced newCondition = EDI_ConditionAdvanced.CreateCondition();
        // The name is what is displayed by the asset so set that too.
        newCondition.name = description;
        newCondition.description = "New interaction";

        // Record all operations on the newConditions so they can be undone.
        Undo.RecordObject(newCondition, "Created new SOBJ_ItemInteractable");

        // Attach the Condition to the AllConditions asset.
        AssetDatabase.AddObjectToAsset(newCondition, sobjItem);

        //// Import the asset so it is recognised as a joined asset.
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(newCondition));

        //// Add the Condition to the AllConditions array.
        ArrayUtility.Add(ref sobjItem.conditioAndReactions, newCondition);

        //// Mark the AllConditions asset as dirty so the editor knows to save changes to it when a project save happens.
        EditorUtility.SetDirty(sobjItem);
    }
    public void RemoveItemInteractable(SOBJ_ItemInteractable itemInteraction)
    {
      

        /* Record all operations on the AllConditions 
         * asset so they can be undone.
         */
        Undo.RecordObject(sobjItem, "Removing SOBJ_ItemInteractable");

        // Remove the specified condition from the AllConditions array.
        ArrayUtility.Remove(ref sobjItem.conditioAndReactions, itemInteraction);

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

    private SOBJ_ItemInteractable TryGetItemInteractionAt(int index)
    {
        // Cache the AllConditions array.
        SOBJ_ItemInteractable[] itemInteractions = sobjItem.conditioAndReactions;

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
    private int TryGetItemInteractionsLength()
    {
        // If there is no Conditions array, return a length of 0.
        if (sobjItem.conditioAndReactions == null)
        {
            return 0;
        }


        // Otherwise return the length of the array.
        return sobjItem.conditioAndReactions.Length;
    }


}
