using UnityEditor;

[CustomEditor(typeof(SOBJ_InventoryGrabItemToMouseReaction))]
public class EDI_InventoryGrabItemToMouseReaction : EDI_Reaction
{
    protected override string GetFoldoutLabel()
    {
        return "Inventory Grab Item To Mouse Reaction";
    }
}
