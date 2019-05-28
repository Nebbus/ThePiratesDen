using UnityEditor;
[CustomEditor(typeof(SOBJ_OnOffGUIelemet))]
public class EDI_OnOffGUIelemet : EDI_Reaction
{


    protected override string GetFoldoutLabel()
    {
        return "Disavel/Enable GUI elemet";
    }


}
