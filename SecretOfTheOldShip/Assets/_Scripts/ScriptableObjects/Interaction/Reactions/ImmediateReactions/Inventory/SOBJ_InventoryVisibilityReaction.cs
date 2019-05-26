using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOBJ_InventoryVisibilityReaction : SOBJ_Reaction
{



    private MONO_Inventory monoInventory;    // Reference to the Inventory component.
    public bool visible;               


    protected override void SpecificInit()
    {
        monoInventory = FindObjectOfType<MONO_Inventory>();

    }


    protected override void ImmediateReaction()
    {
        monoInventory.HideInventory();
    }
}
