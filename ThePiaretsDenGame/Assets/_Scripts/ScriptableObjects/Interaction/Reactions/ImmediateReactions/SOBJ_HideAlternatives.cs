using UnityEngine;

public class SOBJ_HideAlternatives : SOBJ_Reaction
{
	public GameObject thisObj;

	protected override void ImmediateReaction()
	{
		thisObj.GetComponent<MONO_ShowAlternatives> ().HideAlternatives ();
		FindObjectOfType<MONO_SceneManager> ().SetHandleInput (true);
	}
}

