using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SOBJ_MouseRemoveGrabbedItem))]
public class EDI_ReturnItemToInventoryReaction : EDI_Reaction
{
    protected override string GetFoldoutLabel()
    {
        return "Removes grabbed item, doesn't return to inventory";
    }

}
