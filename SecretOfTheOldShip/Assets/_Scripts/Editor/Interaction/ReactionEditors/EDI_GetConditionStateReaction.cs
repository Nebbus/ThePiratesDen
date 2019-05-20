using UnityEditor;

[CustomEditor(typeof(SOBJ_GetConditionStateReaction))]
public class EDI_GetConditionStateReaction : EDI_Reaction
{
	protected override string GetFoldoutLabel()
	{
		return "Get Condition State Reaction";
	}
}
