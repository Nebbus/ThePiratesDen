﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOBJ_InventoryLostItemRection : SOBJ_DelayedReaction
{
    public SOBJ_Item item;                    // The item asset to be added to the Inventory.

    private MONO_Inventory monoInventory;    // Reference to the Inventory component.


    protected override void SpecificInit()
    {
        monoInventory = FindObjectOfType<MONO_Inventory>();

    }


    protected override void ImmediateReaction()
    {
        if (monoInventory == null)
        {
            monoInventory = FindObjectOfType<MONO_Inventory>();
        }
        monoInventory.RemoveItem(item);
    }
}
