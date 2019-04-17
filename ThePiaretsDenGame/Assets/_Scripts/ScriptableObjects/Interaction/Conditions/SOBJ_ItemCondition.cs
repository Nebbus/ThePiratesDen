using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOBJ_ItemCondition : SOBJ_ConditionAdvanced
{

    public SOBJ_Item holdingItem;

    protected override bool advancedCondition()
    {
        return holdingItem.getHash == MONO_PickedUpItem.instance.currentItem.getHash;
    }
}
