using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[CustomEditor(typeof(MONO_Inventory))]
public class EDI_Inventory : Editor {

    private bool[] showItemSlosts = new bool[MONO_Inventory.numberItemSlots];

    private SerializedProperty itemsImagesProperty;
    private SerializedProperty itemsProperty;

    private SerializedProperty inventoryProperty;

    private const string inventoryPropItemsImageName = "invetoryItemsImages";
    private const string inventoryPropItemsName      = "invetoryItems";
    private const string inventoryPropInventoryName  = "inventory";

    private const string itemSlotImageChildeName = "itemImage";



    private float buttonWhidt = 30f;
    private GameObject itemTemplet;
    private MONO_Inventory monoInventory;

    private void OnEnable()
    {
        monoInventory = (MONO_Inventory)target;
        itemTemplet = AssetDatabase.LoadAssetAtPath("Assets/_Prefabs/Inventory/itemTemplet.prefab", typeof(GameObject)) as GameObject;

        itemsImagesProperty   = serializedObject.FindProperty(inventoryPropItemsImageName);
        itemsProperty         = serializedObject.FindProperty(inventoryPropItemsName);
        inventoryProperty = serializedObject.FindProperty(inventoryPropInventoryName);
    }



    public override void OnInspectorGUI()
     {

         serializedObject.Update();

         for (int i = 0; i < MONO_Inventory.numberItemSlots; i++)
         {
             ItemSlotGUI(i);
         }

        buttons();

        EditorGUILayout.PropertyField(inventoryProperty);
        serializedObject.ApplyModifiedProperties();

     }


    private void buttons()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("+", GUILayout.Width(buttonWhidt)))
        {
            MONO_Inventory.numberItemSlots++;
            GameObject temp = GameObject.Instantiate(itemTemplet);
            temp.name       = "item" + MONO_Inventory.numberItemSlots;
            temp.transform.SetParent(monoInventory.inventory.transform);

            Image[] invetoryItemsImagesTemp = new Image[MONO_Inventory.numberItemSlots];
            SOBJ_Item[] invetoryItemsTemp   = new SOBJ_Item[MONO_Inventory.numberItemSlots];
            GameObject[] inventorySlotsTemp = new GameObject[MONO_Inventory.numberItemSlots];
            bool[] showItemSlostsTemp       = new bool[MONO_Inventory.numberItemSlots];

            for (int i = 0; i < MONO_Inventory.numberItemSlots-1; i++)
            {
                invetoryItemsImagesTemp[i] = monoInventory.invetoryItemsImages[i];
                invetoryItemsTemp[i]       = monoInventory.invetoryItems[i];
                inventorySlotsTemp[i]      = monoInventory.inventorySlots[i];
                showItemSlostsTemp[i]      = showItemSlosts[i];
            }

            invetoryItemsImagesTemp[MONO_Inventory.numberItemSlots - 1] = temp.transform.Find(itemSlotImageChildeName).GetComponent<Image>();
            inventorySlotsTemp[MONO_Inventory.numberItemSlots - 1]      = temp;


            monoInventory.invetoryItemsImages = invetoryItemsImagesTemp;
            monoInventory.invetoryItems       = invetoryItemsTemp;
            monoInventory.inventorySlots      = inventorySlotsTemp;
            showItemSlosts                    = showItemSlostsTemp;
        }

        if (GUILayout.Button("-", GUILayout.Width(buttonWhidt)))
        {
            MONO_Inventory.numberItemSlots--;

            Image[] invetoryItemsImagesTemp = new Image[MONO_Inventory.numberItemSlots];
            SOBJ_Item[] invetoryItemsTemp   = new SOBJ_Item[MONO_Inventory.numberItemSlots];
            GameObject[] inventorySlotsTemp = new GameObject[MONO_Inventory.numberItemSlots];
            bool[] showItemSlostsTemp       = new bool[MONO_Inventory.numberItemSlots];

            for (int i = 0; i < MONO_Inventory.numberItemSlots; i++)
            {
                invetoryItemsImagesTemp[i]  = monoInventory.invetoryItemsImages[i];
                invetoryItemsTemp[i]        = monoInventory.invetoryItems[i];
                inventorySlotsTemp[i]       = monoInventory.inventorySlots[i];
                showItemSlostsTemp[i]       = showItemSlosts[i];
            }

            monoInventory.invetoryItems[MONO_Inventory.numberItemSlots]       = null;
            monoInventory.invetoryItemsImages[MONO_Inventory.numberItemSlots] = null;

            DestroyImmediate(monoInventory.inventorySlots[MONO_Inventory.numberItemSlots], false); 

            monoInventory.invetoryItemsImages   = invetoryItemsImagesTemp;
            monoInventory.invetoryItems         = invetoryItemsTemp;
            monoInventory.inventorySlots        = inventorySlotsTemp;
            showItemSlosts                      = showItemSlostsTemp;
        }


        EditorGUILayout.EndHorizontal();
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
