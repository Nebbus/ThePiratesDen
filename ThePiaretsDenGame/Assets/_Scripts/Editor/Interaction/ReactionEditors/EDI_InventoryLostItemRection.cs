using UnityEditor;

[CustomEditor(typeof(SOBJ_InventoryLostItemRection))]
public class EDI_InventoryLostItemRection : EDI_Reaction
{
    protected override string GetFoldoutLabel()
    {
        return "Lost Item Reaction";
    }
}
