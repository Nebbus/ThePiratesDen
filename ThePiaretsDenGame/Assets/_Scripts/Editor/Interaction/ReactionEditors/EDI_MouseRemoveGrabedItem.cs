using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SOBJ_MouseRemoveGrabedItem))]
public class EDI_ReturnItemToInventoryReaction : EDI_Reaction
{
    protected override string GetFoldoutLabel()
    {
        return "Removes grabed item, not returns to inventory";
    }

}
