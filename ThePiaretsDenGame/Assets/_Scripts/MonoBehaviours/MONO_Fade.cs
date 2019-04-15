using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MONO_Fade : MonoBehaviour 
{
	public float fadeDuration = 1f;
	public CanvasGroup canvas;

	private bool isFading = false;
	private float finalAlpha;

	public void Fade(float final)
	{
		finalAlpha = final;

		StartCoroutine (DoFade (finalAlpha));
	}

	IEnumerator DoFade(float targetAlpha)
	{
		// Set the fading flag to true so the FadeAndSwitchScenes coroutine won't be called again.
		isFading = true;

		// Make sure the CanvasGroup blocks raycasts into the scene so no more input can be accepted.
		canvas.blocksRaycasts = true;

		// Calculate how fast the CanvasGroup should fade based on it's current alpha, it's final alpha and how long it has to change between the two.
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
		// Stop the CanvasGroup from blocking raycasts so input is no longer ignored.
		canvas.blocksRaycasts = false;

	}
}
