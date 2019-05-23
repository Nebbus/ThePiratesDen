using UnityEngine;

public class SOBJ_ChangeTagReaction : SOBJ_DelayedReaction
{
	public GameObject target;
	public string newTag;


    protected override void ImmediateReaction()
	{
		target.tag = newTag;
	}
}

