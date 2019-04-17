using UnityEditor;

[CustomEditor(typeof(SOBJ_InventorInputReaction))]
public class EDI_InventorInputReaction : EDI_Reaction
{

    protected override string GetFoldoutLabel()
    {
        return "Inventor Input Reaction";
    }

    
}
