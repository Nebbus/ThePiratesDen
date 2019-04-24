using UnityEngine;

public class SOBJ_ShowAlternativesReaction : SOBJ_Reaction
{
	public GameObject thisObj;

	protected override void ImmediateReaction()
	{
		thisObj.GetComponent<MONO_ShowAlternatives> ().ShowAlternatives ();
	}
}

