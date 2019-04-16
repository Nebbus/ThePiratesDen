using UnityEditor;
[CustomEditor(typeof(SOBJ_SceneChangeReaction))]
public class EDI_SceneChangeReaction : EDI_Reaction {


	protected override string GetFoldoutLabel()
	{
		return "SceneChange";
	}


}
