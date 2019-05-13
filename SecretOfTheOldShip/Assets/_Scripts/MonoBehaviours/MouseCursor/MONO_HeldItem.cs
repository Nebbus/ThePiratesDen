
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
    private bool holding = false;

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
        holding                     = true;
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
        holding                   = false;
        getSetIndex               = -1;
    }


   

  
}
