using UnityEngine;

public class SOBJ_TextDialog : SOBJ_Reaction
{
	public Fungus.Flowchart flowchart;       // The gameobject to be turned on or off.

	protected override void ImmediateReaction()
	{
		flowchart.ExecuteBlock ("Start");
	}
}

