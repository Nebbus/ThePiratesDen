using UnityEditor;
[CustomEditor(typeof(SOBJ_ChangeTagReaction))]
public class EDI_ChangeTagReaction : EDI_Reaction {


	protected override string GetFoldoutLabel()
	{
		return "ChangeTagReaction";
	}
}
