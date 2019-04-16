﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[CustomEditor(typeof(MONO_Inventory))]
public class EDI_Inventory : Editor {

    private bool[] showItemSlosts = new bool[MONO_Inventory.numberItemSlots];

    private SerializedProperty itemsImagesProperty;
    private SerializedProperty itemsProperty;
    private SerializedProperty inventorySlotsProperty;
    private SerializedProperty inventoryProperty;

    private const string inventoryPropItemsImageName = "invetoryItemsImages";
    private const string inventoryPropItemsName      = "invetoryItems";
    private const string inventoryPropInventoryName  = "inventoryGroup";
    private const string inventorySlotPropsName      = "inventorySlots";
    private const string itemSlotImageChildeName     = "itemImage";
    private const string pathToItemSlotPrethab       = "Assets/_Prefabs/Inventory/itemTemplet.prefab";



    private float buttonWhidt = 30f;
    private GameObject itemTemplet;
    private MONO_Inventory monoInventory;

    private bool addSlot = false;
    private bool remobeSlot = false;

    private void OnEnable()
    {

        monoInventory = (MONO_Inventory)target;

        itemTemplet = AssetDatabase.LoadAssetAtPath(pathToItemSlotPrethab, typeof(GameObject)) as GameObject;

        itemsImagesProperty    = serializedObject.FindProperty(inventoryPropItemsImageName);
        itemsProperty          = serializedObject.FindProperty(inventoryPropItemsName);
        inventoryProperty      = serializedObject.FindProperty(inventoryPropInventoryName);
        inventorySlotsProperty = serializedObject.FindProperty(inventorySlotPropsName);

        // Control that the count is upp to date 
        if (MONO_Inventory.numberItemSlots != monoInventory.inventorySlots.Length)
        {
            MONO_Inventory.numberItemSlots = monoInventory.inventorySlots.Length;
        }

    }




    public override void OnInspectorGUI()
     {

         serializedObject.Update();
        // saftety uppdate. 
        if (MONO_Inventory.numberItemSlots != monoInventory.inventorySlots.Length)
        {
            MONO_Inventory.numberItemSlots = monoInventory.inventorySlots.Length;
            showItemSlosts = new bool[MONO_Inventory.numberItemSlots];
        }

        if (addSlot)
        {
            addSlot = false;
            AddInvetorySlotV2();
        }

        if (remobeSlot)
        {
            remobeSlot = false;
            RemoveInventorySlotV2();
        }
  

        EditorGUILayout.PropertyField(inventoryProperty);

        for (int i = 0; i < MONO_Inventory.numberItemSlots; i++)
         {
             ItemSlotGUI(i);
           
         }

        // The buttosn for adding and removin item slots in the invetory
        EditorGUILayout.BeginHorizontal();
         addSlot     = GUILayout.Button("+", GUILayout.Width(buttonWhidt));
         remobeSlot  = GUILayout.Button("-", GUILayout.Width(buttonWhidt));
        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();


    }

    /// <summary>
    /// Function that adds a item slot to the inventory
    /// </summary>
    private void AddInvetorySlotV2()
    {
        MONO_Inventory.numberItemSlots++;

        //Creates new invetory slot slot
        GameObject newItemSLot = GameObject.Instantiate(itemTemplet);
        newItemSLot.name       = "item" + (MONO_Inventory.numberItemSlots - 1);
        newItemSLot.transform.SetParent(monoInventory.inventoryGroup.transform);


        // Updates all arrays
        EXT_SerializedProperty.AddToObjectArray<Image>(itemsImagesProperty, newItemSLot.transform.Find(itemSlotImageChildeName).GetComponent<Image>());
        EXT_SerializedProperty.AddToObjectArray<GameObject>(inventorySlotsProperty, newItemSLot);
        EXT_SerializedProperty.AddToObjectArray<SOBJ_Item>(itemsProperty, null);
        showItemSlosts = new bool[MONO_Inventory.numberItemSlots];
    }

    /// <summary>
    /// Function that removes a item slot from the inventory
    /// </summary>
    private void RemoveInventorySlotV2()
    {
        MONO_Inventory.numberItemSlots--;


        GameObject lastSlot = monoInventory.inventorySlots[MONO_Inventory.numberItemSlots]; 

        EXT_SerializedProperty.RemoveFromObjectArrayAt(itemsImagesProperty, MONO_Inventory.numberItemSlots);
        EXT_SerializedProperty.RemoveFromObjectArrayAt(inventorySlotsProperty, MONO_Inventory.numberItemSlots);
        EXT_SerializedProperty.RemoveFromObjectArrayAt(itemsProperty, MONO_Inventory.numberItemSlots);

        DestroyImmediate(lastSlot, false);

        showItemSlosts = new bool[MONO_Inventory.numberItemSlots];
    }



    private void ItemSlotGUI(int index)
    {

        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;
       
        showItemSlosts[index] = EditorGUILayout.Foldout(showItemSlosts[index], "Item slot " + index);

        if (showItemSlosts[index])
        {
            EditorGUILayout.PropertyField(itemsImagesProperty.GetArrayElementAtIndex(index));
            EditorGUILayout.PropertyField(itemsProperty.GetArrayElementAtIndex(index));

        }
        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();

    }



}
