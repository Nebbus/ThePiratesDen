using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SOBJ_ReturnItemToInventoryReaction))]
public class EDI_ReturnItemToInventoryReaction : EDI_Reaction
{
    protected override string GetFoldoutLabel()
    {
        return "Return Item To Inventory Reaction";
    }

}
