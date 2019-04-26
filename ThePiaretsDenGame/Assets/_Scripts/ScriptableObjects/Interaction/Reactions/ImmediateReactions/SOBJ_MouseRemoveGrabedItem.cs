using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOBJ_MouseRemoveGrabedItem : SOBJ_Reaction
{
    protected override void ImmediateReaction()
    {
        MONO_AdventureCursor.instance.getMonoHoldedItem.ReturnItemToInventory();

    }
}
