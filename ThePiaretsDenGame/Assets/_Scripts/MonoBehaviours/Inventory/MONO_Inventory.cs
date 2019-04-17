using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;



public class MONO_Inventory : MonoBehaviour {

    [Tooltip("The object that gas the Grid Layout Group on it, it will have all items as children")]
    public GameObject inventoryGroup;
    public GameObject inventoryImage;

    public Image[]      invetoryItemsImages = new Image[numberItemSlots];
    public SOBJ_Item[]  invetoryItems       = new SOBJ_Item[numberItemSlots];
    public GameObject[] inventorySlots      = new GameObject[numberItemSlots];

    public static int numberItemSlots = 0;

    private bool HandleInput = true;


    public void HandleInventoryClick()
    {
        if (HandleInput)
        {
            if (inventoryGroup.activeSelf)
            {
                HideInventory();
            }
            else
            {
                ShowInventory();
            }
        }

    }

    public void ShowInventory()
    {
        inventoryGroup.SetActive(true);
        inventoryImage.SetActive(true);
    }

    public void HideInventory()
    {
        inventoryGroup.SetActive(false);
        inventoryImage.SetActive(false);
    }

 
    /// <summary>
    /// Sets if the inventory should handel input or not
    /// </summary>
    /// <param name="setTo"> the value the HandleInpu variabler 
    ///  is going to be set to</param>
    public void SetHandleINput( bool setTo)
    {
        HandleInput = setTo;
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
                PickUpReaction();
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

    private void PickUpReaction()
    {
        this.GetComponent<MONO_HiglightObject>().startFlashing();
    }







}
