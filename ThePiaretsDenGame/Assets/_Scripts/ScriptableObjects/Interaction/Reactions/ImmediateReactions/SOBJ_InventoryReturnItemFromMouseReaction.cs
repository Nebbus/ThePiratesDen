using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOBJ_InventoryReturnItemFromMouseReaction : SOBJ_Reaction
{

    private MONO_Inventory monoInventory;    // Reference to the Inventory component.

    protected override void SpecificInit()
    {
        monoInventory = FindObjectOfType<MONO_Inventory>();
    }

    protected override void ImmediateReaction()
    {
        monoInventory.ReturnToInventory(MONO_itemGradFromTheInventory.instance.getSetIndex);
        MONO_itemGradFromTheInventory.instance.ReturnItemToInventory();
    }
}