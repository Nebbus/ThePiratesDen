using UnityEditor;
[CustomEditor(typeof(SOBJ_ObjectReaction))]
public class EDI_ObjectReaction : EDI_Reaction {


    protected override string GetFoldoutLabel()
    {
        return "ObjectReaction";
    }


}
