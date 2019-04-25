using UnityEditor;

[CustomEditor(typeof(SOBJ_InventoryReturnItemFromMouseReaction))]
public class EDI_InventoryReturnItemFromMouseReaction : EDI_Reaction
{
    protected override string GetFoldoutLabel()
    {
        return "Inventory Return Item From MouseReaction";
    }
}
