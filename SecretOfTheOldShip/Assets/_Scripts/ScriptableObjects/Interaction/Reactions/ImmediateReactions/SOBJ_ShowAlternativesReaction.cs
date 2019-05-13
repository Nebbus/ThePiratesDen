using UnityEngine;

public class SOBJ_ShowAlternativesReaction : SOBJ_Reaction
{
	public GameObject rootReaction;

	protected override void ImmediateReaction()
	{
		rootReaction.GetComponent<MONO_ShowAlternatives> ().ShowAlternatives ();
	}
}

