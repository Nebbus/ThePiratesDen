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
    private SerializedProperty inventorySlotsProperty;
    private SerializedProperty inventoryProperty;

    private const string inventoryPropItemsImageName = "invetoryItemsImages";
    private const string inventoryPropItemsName      = "invetoryItems";
    private const string inventoryPropInventoryName  = "inventory";
    private const string inventorySlotPropsName      = "inventorySlots";
    private const string itemSlotImageChildeName     = "itemImage";
    private const string pathToItemSlotPrethab       = "Assets/_Prefabs/Inventory/itemTemplet.prefab";



    private float buttonWhidt = 30f;
    private GameObject itemTemplet;
    private MONO_Inventory monoInventory;

    private void OnEnable()
    {

        monoInventory = (MONO_Inventory)target;

        // Control that the count is upp to date 
        monoInventory = (MONO_Inventory)target;
       

        itemTemplet = AssetDatabase.LoadAssetAtPath(pathToItemSlotPrethab, typeof(GameObject)) as GameObject;

        itemsImagesProperty    = serializedObject.FindProperty(inventoryPropItemsImageName);
        itemsProperty          = serializedObject.FindProperty(inventoryPropItemsName);
        inventoryProperty      = serializedObject.FindProperty(inventoryPropInventoryName);
        inventorySlotsProperty = serializedObject.FindProperty(inventorySlotPropsName);

        if (MONO_Inventory.numberItemSlots != inventorySlotsProperty.arraySize)
        {
            MONO_Inventory.numberItemSlots = inventorySlotsProperty.arraySize;
        }

    }




    public override void OnInspectorGUI()
     {

         serializedObject.Update();
        if (MONO_Inventory.numberItemSlots != inventorySlotsProperty.arraySize)
        {
            MONO_Inventory.numberItemSlots = inventorySlotsProperty.arraySize;
        }


        for (int i = 0; i < MONO_Inventory.numberItemSlots; i++)
         {
             ItemSlotGUI(i);
           
         }
        // The buttosn for adding and removin item slots in the invetory
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("+", GUILayout.Width(buttonWhidt)))
        {
            AddInvetorySlotV1();
        }

        if (GUILayout.Button("-", GUILayout.Width(buttonWhidt)))
        {
            RemoveInventorySlotV2();
        }


        EditorGUILayout.EndHorizontal();
        EditorGUILayout.PropertyField(inventoryProperty);
        serializedObject.ApplyModifiedProperties();


    }



    private void AddInvetorySlotV1()
    {
        MONO_Inventory.numberItemSlots++;
        GameObject temp = GameObject.Instantiate(itemTemplet);
        temp.name       = "item" + MONO_Inventory.numberItemSlots;
        temp.transform.SetParent(monoInventory.inventory.transform);


            Image[]      invetoryItemsImagesTemp = new Image[MONO_Inventory.numberItemSlots];
            SOBJ_Item[]  invetoryItemsTemp       = new SOBJ_Item[MONO_Inventory.numberItemSlots];
            GameObject[] inventorySlotsTemp      = new GameObject[MONO_Inventory.numberItemSlots];
            bool[]       showItemSlostsTemp      = new bool[MONO_Inventory.numberItemSlots];

            for (int i = 0; i < MONO_Inventory.numberItemSlots - 1; i++)
            {
                invetoryItemsImagesTemp[i]  = monoInventory.invetoryItemsImages[i];
                invetoryItemsTemp[i]        = monoInventory.invetoryItems[i];
                inventorySlotsTemp[i]       = monoInventory.inventorySlots[i];
                showItemSlostsTemp[i]       = showItemSlosts[i];
            }

            invetoryItemsImagesTemp[MONO_Inventory.numberItemSlots - 1] = temp.transform.Find(itemSlotImageChildeName).GetComponent<Image>();
            inventorySlotsTemp[MONO_Inventory.numberItemSlots - 1]      = temp;


            monoInventory.invetoryItemsImages   = invetoryItemsImagesTemp;
            monoInventory.invetoryItems         = invetoryItemsTemp;
            monoInventory.inventorySlots        = inventorySlotsTemp;
            showItemSlosts                      = showItemSlostsTemp;
    }
    private void AddInvetorySlotV2()
    {
        MONO_Inventory.numberItemSlots++;
        GameObject temp = GameObject.Instantiate(itemTemplet);
        temp.name       = "item" + MONO_Inventory.numberItemSlots;
        temp.transform.SetParent(monoInventory.inventory.transform);


        EXT_SerializedProperty.AddToObjectArray<Image>(itemsImagesProperty, temp.transform.Find(itemSlotImageChildeName).GetComponent<Image>());
        EXT_SerializedProperty.AddToObjectArray<GameObject>(inventorySlotsProperty, temp);
        EXT_SerializedProperty.AddToObjectArray<SOBJ_Item>(itemsProperty, null);

        bool[] showItemSlostsTemp = new bool[MONO_Inventory.numberItemSlots];

        for (int i = 0; i < MONO_Inventory.numberItemSlots - 1; i++)
        {
            showItemSlostsTemp[i] = showItemSlosts[i];
        }
        showItemSlosts = showItemSlostsTemp;
    }

    private void RemoveInventorySlotV1()
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

        monoInventory.invetoryItemsImages = invetoryItemsImagesTemp;
        monoInventory.invetoryItems       = invetoryItemsTemp;
        monoInventory.inventorySlots      = inventorySlotsTemp;
        showItemSlosts                    = showItemSlostsTemp;
    }
    private void RemoveInventorySlotV2()
    {
        MONO_Inventory.numberItemSlots--;


        GameObject temp = monoInventory.inventorySlots[MONO_Inventory.numberItemSlots]; 
        EXT_SerializedProperty.RemoveFromObjectArrayAt(itemsImagesProperty, MONO_Inventory.numberItemSlots);
        EXT_SerializedProperty.RemoveFromObjectArrayAt(inventorySlotsProperty, MONO_Inventory.numberItemSlots);
        EXT_SerializedProperty.RemoveFromObjectArrayAt(itemsProperty, MONO_Inventory.numberItemSlots);
        bool[] showItemSlostsTemp = new bool[MONO_Inventory.numberItemSlots];

        for (int i = 0; i < MONO_Inventory.numberItemSlots; i++)
        {
            showItemSlostsTemp[i] = showItemSlosts[i];
        }

        DestroyImmediate(temp, false);

        showItemSlosts = showItemSlostsTemp;
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
