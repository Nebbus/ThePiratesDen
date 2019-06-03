using UnityEngine;

public class SOBJ_ChangeTextureReaction : SOBJ_DelayedReaction
{
	public Material target;
	public GameObject gameObject;



    protected override void ImmediateReaction()
	{
		gameObject.GetComponent<Renderer> ().material.mainTexture = target;
	}
}

