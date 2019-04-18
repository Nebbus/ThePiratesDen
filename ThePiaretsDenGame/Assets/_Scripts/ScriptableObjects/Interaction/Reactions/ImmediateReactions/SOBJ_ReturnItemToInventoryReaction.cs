using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOBJ_ReturnItemToInventoryReaction : SOBJ_Reaction
{
    protected override void ImmediateReaction()
    {
       if(MONO_itemGradFromTheInventory.instance.currentItem == null)
        {
            return;
        }
        MONO_itemGradFromTheInventory.instance.ReturnItemToInventory();

    }
}
