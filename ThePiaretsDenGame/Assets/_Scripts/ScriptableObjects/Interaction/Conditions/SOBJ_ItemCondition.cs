using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOBJ_ItemCondition : SOBJ_ConditionAdvanced
{

    public SOBJ_Item holdingItem;

    protected override bool advancedCondition()
    {
        
        if (MONO_itemGradFromTheInventory.instance.currentItem != null)
        {
       
            return holdingItem.getHash == MONO_itemGradFromTheInventory.instance.currentItem.getHash;
        }
        
        return false;
    }
}
