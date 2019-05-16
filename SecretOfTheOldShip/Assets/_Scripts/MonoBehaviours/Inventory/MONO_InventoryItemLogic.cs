using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MONO_InventoryItemLogic : MONO_InteractionBase
{


    public int hashCode = -1;

    private int index;
    private bool hasBenGrabed = false;
    private MONO_Inventory monoInventory;

    private int realHashCode = -1;

    public int getSetItemsHashCode
    {
        get
        {
            return realHashCode;
        }
        set
        {
            realHashCode = value;
            hashCode = realHashCode;
        }

    }


    public MONO_Inventory setMonoInventory
    {
        set
        {
            monoInventory = value;
        }

    }
    /// <summary>
    /// Sets the index of this itemSlot, 
    /// is cald then the slot is created.
    /// </summary>
    public int getStetIndex
    {
        set
        {
            index = value;
        }
        get
        {
            return index;
        }
    }


    public void Start()
    {
        if(monoInventory == null)
        {
            monoInventory = FindObjectOfType<MONO_Inventory>();

        }

    }

    /// <summary>
    /// Run throug the ractions the item has attached to it.
    /// </summary>
    public void ClickReact()
    {
        /*prevents the slot from caling
        * reactions then no item
        * is precent
        */
        bool doNotreturnItem = true;
        if (getSetItemsHashCode != -1)
        {

            doNotreturnItem = !monoInventory.GetItem(getSetItemsHashCode).OnClickInteractionRun(this);
           
        }
        // if no reaction was don ore no item is held, return to inventory
        if (doNotreturnItem)
        {
            monoInventory.ReturnToInventory(MONO_AdventureCursor.instance.getMonoHoldedItem.getSetIndex);
        }

    }

    /// <summary>
    /// Run throug the ractions the item has attached to it.
    /// </summary>
    public void HovorReact()
    {
        if (getSetItemsHashCode != -1)
        {
            
            monoInventory.GetItem(getSetItemsHashCode).OnHoverInteractionRun(this);
        }
    }

    public override void OnClick()
    {
        ClickReact();
    }

    public override void OnHoverEnterd()
    {
        HovorReact();
    }

    public override void OnHover()
    {
      
    }

    public override void OnHoverExit()
    {
       
    }
}
