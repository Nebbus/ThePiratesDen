using UnityEngine;

public class SOBJ_ShowAlternativesReaction : SOBJ_Reaction
{
	
	public GameObject[] reactions = new GameObject[3];

	protected override void ImmediateReaction()
	{
		for (int i = 0; i < reactions.Length; i++) 
		{
			reactions [i].SetActive (true);
		}
	}
}

