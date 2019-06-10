
using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is a singleton 
/// </summary>
public class MONO_HeldItem : MonoBehaviour
{

    [Tooltip("Should be the null item at the start" +
            "(an empty item that represents null)")]
    public SOBJ_Item currentItem = null;
 
    public Image currentItemImage;
    private int  currentItemInventoryIndex;


    private SOBJ_Item nullStartItem;

    public int getSetIndex
    {
        get
        {
            return currentItemInventoryIndex;
        }
        set
        {
            currentItemInventoryIndex = value;
        }
    }

    public bool isHoldingItem
    {
        get
        {
            return currentItem.getHash != nullStartItem.getHash;
        }
    }

    public void Start()
    {
       nullStartItem = currentItem;
    }

    /// <summary>
    /// Sets the item that is holded for the movment,
    /// is caled from te MONO_Inventory script
    /// </summary>
    /// <param name="item"> item thats grabbed</param>
    /// <param name="sprite"> the items sprite</param>
    /// <param name="index"> the array index of the item in MONO_Inventory,
    /// will ve used to return the item to the inventory later </param>
    public void GrabbedItem(SOBJ_Item item, Sprite sprite, int index)
    {
        currentItem                 = item;
        currentItemImage.sprite     = sprite;
        currentItemImage.enabled    = true;
        getSetIndex                 = index;

    }

    /// <summary>
    /// This class only removes the item from teh mous,
    /// it wont remove it, it cald from MONO_inventory
    /// and from the reaction SOBJ_MouseRemoveGrabedObject
    /// </summary>
    public void ReturnItemToInventory()
    {
        currentItem               = nullStartItem;
        currentItemImage.sprite   = null;
        currentItemImage.enabled  = false;
        getSetIndex               = -1;
    }

    /// <summary>
    /// Clears the items and return the indx of the item that was clered
    /// (Will be caled from MONO_SceneManager on scene change)
    /// </summary>
    /// <returns>index of item that was clerd (-1 if no item was held)</returns>
   public int ReturnItemToInventorySceneChange()
    {
        int returnIndex = getSetIndex;
        if (returnIndex == -1)
        {
            return -1;
        }

        currentItem              = nullStartItem;
        currentItemImage.sprite  = null;
        currentItemImage.enabled = false;
        getSetIndex              = -1;


        return returnIndex;
    }

  
}
