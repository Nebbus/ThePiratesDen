using UnityEngine;

public class SOBJ_MoveObjectReaction : SOBJ_DelayedReaction
{
	public bool fade;
	public bool keepMoving;
	public MONO_FadeObject fadeScript;
	public MONO_Lerp lerpScript;


	protected override void ImmediateReaction()
	{
		if (fade)
		{
			fadeScript.StartFade (0.0f);
		}

		lerpScript.StartLerp (keepMoving);

		if (fade) 
		{
			fadeScript.StartFade (1.0f);
		}
	}
}

