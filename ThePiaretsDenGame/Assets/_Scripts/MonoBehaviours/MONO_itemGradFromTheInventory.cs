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

    public SOBJ_Item currentItem = null;
    public Image     currentItemImage;
    private int      currentItemInventoryIndex;
    private bool holding = false;

   
    


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
        currentItem               = null;
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
