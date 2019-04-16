using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MONO_Fade : MonoBehaviour 
{
	[HideInInspector]
	public bool isFading = false;					//whether we are currently fading or not.
	public float fadeDuration = 1f;					//how long to fade for.
	public CanvasGroup canvas;						//The canvas used to fade.

	private float finalAlpha;

	/// <summary>
	/// Initalize the fade with a coroutine based on an input target alpha.
	/// </summary>
	/// <param name="final">The target alpha.</param>
	public void Fade(float final)
	{
		finalAlpha = final;
		StartCoroutine(DoFade (finalAlpha));
	}
		
	/// <summary>
	/// Coroutine where the fade is actually done.
	/// </summary>
	/// <param name="targetAlpha">Target alpha sent from previous function.</param>
	IEnumerator DoFade(float targetAlpha)
	{
		// Set the fading flag to true so the FadeAndSwitchScenes coroutine won't be called again.
		isFading = true;

		/* Calculate how fast the CanvasGroup should fade based on it's current alpha, 
		 * it's final alpha and how long it has to change between the two.
		 */
		float fadeSpeed = Mathf.Abs (canvas.alpha - targetAlpha) / fadeDuration;

		// While the CanvasGroup hasn't reached the final alpha yet...
		while (!Mathf.Approximately (canvas.alpha, targetAlpha))
		{
			// ... move the alpha towards it's target alpha.
			canvas.alpha = Mathf.MoveTowards (canvas.alpha, targetAlpha,
				fadeSpeed * Time.deltaTime);
			// Wait for a frame then continue.
			yield return null;
		}

		// Set the flag to false since the fade has finished.
		isFading = false;
	}
}
