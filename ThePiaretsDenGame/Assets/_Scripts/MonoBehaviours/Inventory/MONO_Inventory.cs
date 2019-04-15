using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;



public class MONO_Inventory : MonoBehaviour {

    public GameObject inventory;

    public Image[]     invetoryItemsImages = new Image[numberItemSlots];
    public SOBJ_Item[] invetoryItems       = new SOBJ_Item[numberItemSlots];
    public GameObject[] inventorySlots     = new GameObject[numberItemSlots];

    public static int numberItemSlots = 0;




    public void AddItem(SOBJ_Item itemToAdd)
     {
         for (int i = 0; i < invetoryItems.Length; i++)
         {
             if (invetoryItems[i] == null)
             {
                 invetoryItems[i]               = itemToAdd;
                invetoryItemsImages[i].sprite  = itemToAdd.sprite;
                invetoryItemsImages[i].enabled = true;
                 return;
             }

         }
     }

     public void RemoveItem(SOBJ_Item itemToRemove)
     {
         for (int i = 0; i < invetoryItems.Length; i++)
         {
             if (invetoryItems[i] == null)
             {
                 invetoryItems[i]                = null;
                invetoryItemsImages[i].sprite   = null;
                invetoryItemsImages[i].enabled  = false;
                 return;
             }

         }
     }








}
