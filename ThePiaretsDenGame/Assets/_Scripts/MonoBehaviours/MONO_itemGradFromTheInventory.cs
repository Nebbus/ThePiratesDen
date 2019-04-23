using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is a singleton 
/// </summary>
public class MONO_itemGradFromTheInventory : MonoBehaviour
{
    public static MONO_itemGradFromTheInventory instance;
    public MONO_Inventory           monoInventory;


    [Tooltip("Should be the null item at the start" +
        "(a empty item thet represents null)")]
    public SOBJ_Item currentItem = null;
 
    public Image     currentItemImage;
    private int      currentItemInventoryIndex;
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

    public void Start()
    {

        if( instance == null)
        {
            instance = this;
            nullStartItem = currentItem;
            monoInventory = FindObjectOfType<MONO_Inventory>();
        }
        else
        {
            GameObject.Destroy(this);
        }
        
    }

    public void GrabdItem(SOBJ_Item item, Sprite sprite, int index)
    {
        currentItem                 = item;
        currentItemImage.sprite     = sprite;
        currentItemImage.enabled    = true;
        currentItemInventoryIndex   = index;
        holding                     = true;
    }

    public void ReturnItemToInventory()
    {
        currentItem               = nullStartItem;
        currentItemImage.sprite   = null;
        currentItemImage.enabled  = false;
        holding                   = false;
        currentItemInventoryIndex = -1;
    }


    void Update ()
    {
     
        transform.position =  Input.mousePosition;

	}


  
}
