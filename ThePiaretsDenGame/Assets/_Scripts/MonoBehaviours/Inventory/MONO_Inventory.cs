using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;



public class MONO_Inventory : MonoBehaviour {

    



    [Tooltip("The object that gas the Grid Layout Group on it, it will have all items as children")]
    public GameObject inventoryGroup;
    [SerializeField]
    private GameObject inventoryImage;

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
                itemToAdd.InitReaction();// ini all reactions
                invetoryItems[i]               = itemToAdd;
                invetoryItemsImages[i].sprite  = itemToAdd.sprite;
                invetoryItemsImages[i].enabled = true;
                inventorySlots[i].GetComponent<MONO_InventoryItemLogic>().hashCode = itemToAdd.getHash;
                PickUpReaction();
                return;
             }

         }
        Debug.LogError("The inventory is overflowing!!!!!");
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
                inventorySlots[i].GetComponent<MONO_InventoryItemLogic>().hashCode = -1;
                 return;
             }

         }
     }


    /// <summary>
    /// Removes item from inventory
    /// </summary>
    /// <param name="itemToRemove">item to be removed</param>
    public void RemoveItem(int itemToRemoveIndex)
    {
        if (itemToRemoveIndex < invetoryItems.Length && itemToRemoveIndex > (-1))
        {
            invetoryItems[itemToRemoveIndex]               = null;
            invetoryItemsImages[itemToRemoveIndex].sprite  = null;
            invetoryItemsImages[itemToRemoveIndex].enabled = false;
            inventorySlots[itemToRemoveIndex].GetComponent<MONO_InventoryItemLogic>().hashCode = -1;
        }

    }


    /// <summary>
    /// Sets the value of the item that the pointer is holdin
    /// </summary>
    /// <param name="itemIndex">index of item to grabe</param>
    public void GrabItem(int itemIndex)
    {
        if (invetoryItemsImages[itemIndex].sprite != null && itemIndex < invetoryItems.Length && itemIndex > (-1))
        {
            MONO_itemGradFromTheInventory.instance.GrabdItem(invetoryItems[itemIndex], invetoryItemsImages[itemIndex].sprite, itemIndex);
            invetoryItemsImages[itemIndex].enabled = false;
        }
    }
    /// <summary>
    /// Activates the items that was holded by the mouse pointer
    /// </summary>
    /// <param name="itemIndex"></param>
    public void ReturnToInventory(int itemIndex)
    {
        Debug.Log("osinrg  "+ itemIndex);
        if (itemIndex < invetoryItems.Length && itemIndex > (-1))
        {
            Debug.Log("osinrg2 " + itemIndex);
            invetoryItemsImages[itemIndex].enabled = true;
        }
    }


    /// <summary>
    /// Combinds to inventory object intwo a new, dose it by 
    /// removing the towitems and crating the new one
    /// </summary>
    /// <param name="hashPartOne"></param>
    /// <param name="hashPartTwo"></param>
    /// <param name="result"></param>
    public void combindeItems(int indexPartOne, int indexPartTwo, SOBJ_Item result)
    {
        RemoveItem(indexPartOne);
        RemoveItem(indexPartTwo);
        AddItem(result);
    }

   



    private void PickUpReaction()
    {
        this.GetComponent<MONO_HiglightObject>().startFlashing();
    }

    private void OnMouseEnter()
    {
     
    }


}
