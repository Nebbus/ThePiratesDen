using UnityEngine;

public class SOBJ_AnimationReaction : SOBJ_DelayedReaction
{
	public Animator animation;
	public string state;

	protected override void ImmediateReaction()
	{
		animation.Play (state);
	}
}

