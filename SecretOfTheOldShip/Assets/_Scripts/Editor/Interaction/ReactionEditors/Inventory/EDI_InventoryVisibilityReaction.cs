using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SOBJ_InventoryVisibilityReaction))]
public class EDI_InventoryVisibilityReaction : EDI_Reaction
{
    protected override string GetFoldoutLabel()
    {
        return "Inventory Visibility";
    }

    
}
