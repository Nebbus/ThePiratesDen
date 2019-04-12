using UnityEditor;
[CustomEditor(typeof(SOBJ_TextDialog))]
public class EDI_TextDialog : EDI_Reaction {


	protected override string GetFoldoutLabel()
	{
		return "TextDialog";
	}


}
