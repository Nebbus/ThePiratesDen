using UnityEngine;

public class SOBJ_ChangeTagReaction : SOBJ_DelayedReaction
{
	public GameObject objectWithNewTag;
	public string newTag;

	protected override void ImmediateReaction()
	{
		objectWithNewTag.tag = newTag;
	}
}

