using UnityEditor;
[CustomEditor(typeof(SOBJ_ChangeTextureReaction))]
public class EDI_ChangeTextureReaction : EDI_Reaction {


	protected override string GetFoldoutLabel()
	{
		return "ChangeTexture";
	}
}
