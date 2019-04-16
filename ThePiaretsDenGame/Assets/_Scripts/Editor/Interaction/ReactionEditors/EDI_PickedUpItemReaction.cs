using UnityEditor;

[CustomEditor(typeof(SOBJ_PickedUpItemReaction))]
public class EDI_PickedUpItemReaction : EDI_Reaction
{
    protected override string GetFoldoutLabel()
    {
        return "Picked Up Item Reaction";
    }
}

