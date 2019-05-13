using UnityEditor;
[CustomEditor(typeof(SOBJ_HideAlternatives))]
public class EDI_HideAlternatives : EDI_Reaction {


	protected override string GetFoldoutLabel()
	{
		return "HideAlternatives";
	}
}
