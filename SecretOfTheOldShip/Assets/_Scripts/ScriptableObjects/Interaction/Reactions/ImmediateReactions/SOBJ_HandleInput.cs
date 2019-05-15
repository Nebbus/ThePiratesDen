using UnityEngine;

public class SOBJ_HandleInput : SOBJ_DelayedReaction
{
	public bool enabled;

	protected override void ImmediateReaction()
	{
		FindObjectOfType<MONO_SceneManager> ().SetHandleInput (enabled);
	}
}

