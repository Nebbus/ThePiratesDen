using UnityEditor;

[CustomEditor(typeof(SOBJ_LostItemRection))]
public class EDI_LostItemRection : EDI_Reaction
{
    protected override string GetFoldoutLabel()
    {
        return "Lost Item Reaction";
    }
}
