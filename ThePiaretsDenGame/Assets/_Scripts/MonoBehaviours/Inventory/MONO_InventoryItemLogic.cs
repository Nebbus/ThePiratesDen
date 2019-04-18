using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MONO_InventoryItemLogic : MonoBehaviour
{

    
    public int hashCode;

    private int  index;
    private bool hasBenGrabed = false;
    private MONO_Inventory monoInventory;

    public MONO_Inventory setMonoInventory
    {
        set
        {
            monoInventory = value;
        }

    }
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


    /// <summary>
    /// Will be caled by unitys event system
    /// </summary>
    public void HandleClickInput()
    {
        if(MONO_itemGradFromTheInventory.instance.currentItem != null)
        {
            React();
        }
        else if ( hasBenGrabed && MONO_itemGradFromTheInventory.instance.currentItem == null)
        {
            PickMeUpp();
        }
        
    }

    /// <summary>
    /// cald by unity event system to pick upp the objet
    /// dose this by calling a funktion in the inventory
    /// that set the mouse values
    /// </summary>
    public void PickMeUpp()
    {
        monoInventory.GrabItem(getStetIndex);
        hasBenGrabed = false;
    }

    /// <summary>
    /// Run throug the ractions the item has attached to it.
    /// </summary>
    public void React()
    {
        monoInventory.invetoryItems[index].InteractionRun(this);
    }

}
