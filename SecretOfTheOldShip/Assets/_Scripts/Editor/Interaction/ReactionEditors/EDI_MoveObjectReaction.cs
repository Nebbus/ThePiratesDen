using UnityEditor;
[CustomEditor(typeof(SOBJ_MoveObjectReaction))]
public class EDI_MoveObjectReaction : EDI_Reaction {


	protected override string GetFoldoutLabel()
	{
		return "MoveObject";
	}


}
