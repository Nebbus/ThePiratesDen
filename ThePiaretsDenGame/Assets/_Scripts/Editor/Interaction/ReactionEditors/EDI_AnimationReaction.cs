using UnityEditor;
[CustomEditor(typeof(SOBJ_AnimationReaction))]
public class EDI_AnimationReaction : EDI_Reaction {


	protected override string GetFoldoutLabel()
	{
		return "AnimationReaction";
	}
}
