using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;



public class MONO_Inventory : MonoBehaviour {

    [Tooltip("The object that gas the Grid Layout Group on it, it will have all items as children")]
    public GameObject inventoryGroup;

    public Image[]     invetoryItemsImages = new Image[numberItemSlots];
    public SOBJ_Item[] invetoryItems       = new SOBJ_Item[numberItemSlots];
    public GameObject[] inventorySlots     = new GameObject[numberItemSlots];

    public static int numberItemSlots = 0;



    
    


    public void HandleInventoryClick()
    {
        if (inventoryGroup.activeSelf)
        {
            hideInventory();
        }
        else
        {
            showInventory();
        }
    }

    public void showInventory()
    {
        inventoryGroup.SetActive(true);
    }

    public void hideInventory()
    {
        inventoryGroup.SetActive(false);
    }


    /// <summary>
    /// Adds item to the inventory
    /// </summary>
    /// <param name="itemToAdd"> item to add</param>
    public void AddItem(SOBJ_Item itemToAdd)
     {
         for (int i = 0; i < invetoryItems.Length; i++)
         {
             if (invetoryItems[i] == null)
             {
                invetoryItems[i]               = itemToAdd;
                invetoryItemsImages[i].sprite  = itemToAdd.sprite;
                invetoryItemsImages[i].enabled = true;
                pickUpReaction();
                 return;
             }

         }
     }
    /// <summary>
    /// Removes item from inventory
    /// </summary>
    /// <param name="itemToRemove">item to be removed</param>
     public void RemoveItem(SOBJ_Item itemToRemove)
     {
         for (int i = 0; i < invetoryItems.Length; i++)
         {
             if (invetoryItems[i] == itemToRemove)
             {
                invetoryItems[i]                = null;
                invetoryItemsImages[i].sprite   = null;
                invetoryItemsImages[i].enabled  = false;
                 return;
             }

         }
     }

    private void pickUpReaction()
    {
        this.GetComponent<MONO_HiglightObject>().startFlashing();
    }







}
