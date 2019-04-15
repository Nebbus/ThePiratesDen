using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MONO_Fade : MonoBehaviour 
{
	public int fadeDuration;
	public bool fadeOut;
	public CanvasGroup canvas;

	private float finalAlpha;

	public void Fade()
	{
		StartCoroutine (DoFade ());

	}

	IEnumerator DoFade()
	{
		if (fadeOut)
		{
			finalAlpha = 1f;
			while (canvas.alpha < finalAlpha) 
			{
				canvas.alpha += Time.deltaTime / fadeDuration;
				yield return null;
			}
		} 
		else 
		{
			finalAlpha = 0f;
			while (canvas.alpha > finalAlpha) 
			{
				canvas.alpha += Time.deltaTime / fadeDuration;
				yield return null;
			}
		}
	}
}
