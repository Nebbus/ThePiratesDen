using UnityEngine;

public class SOBJ_TextDialog : SOBJ_Reaction
{
	public Fungus.Flowchart flowchart;       // The gameobject to be turned on or off.
	public string interactionBlock;

	protected override void ImmediateReaction()
	{
		flowchart.ExecuteBlock (interactionBlock);
	}
}

