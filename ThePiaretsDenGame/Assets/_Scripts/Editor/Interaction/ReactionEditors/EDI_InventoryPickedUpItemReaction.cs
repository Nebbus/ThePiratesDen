using UnityEditor;

[CustomEditor(typeof(SOBJ_InventoryPickedUpItemReaction))]
public class EDI_InventoryPickedUpItemReaction : EDI_Reaction
{
    protected override string GetFoldoutLabel()
    {
        return "Picked Up Item Reaction";
    }
}

