using UnityEditor;
[CustomEditor(typeof(SOBJ_ShowAlternativesReaction))]
public class EDI_ShowAlternativesReaction : EDI_Reaction {


	protected override string GetFoldoutLabel()
	{
		return "ShowAlternatives";
	}
}
