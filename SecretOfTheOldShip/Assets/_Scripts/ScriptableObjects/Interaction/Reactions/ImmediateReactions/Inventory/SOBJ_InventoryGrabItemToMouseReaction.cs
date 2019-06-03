using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOBJ_InventoryGrabItemToMouseReaction : SOBJ_Reaction
{
    [SerializeField]
    private SOBJ_Item itemToGrab;
    public SOBJ_Item getItemToGrabe
    {
        get
        {
            return itemToGrab;
        }
    }

    private MONO_Inventory monoInventory;    // Reference to the Inventory component.

    protected override void SpecificInit()
    {
        monoInventory = FindObjectOfType<MONO_Inventory>();

    }

    protected override void ImmediateReaction()
    {
        if(monoInventory == null)
        {
            monoInventory = FindObjectOfType<MONO_Inventory>();
        } 

        monoInventory.GrabItem(itemToGrab.getHash);  
    }
}
