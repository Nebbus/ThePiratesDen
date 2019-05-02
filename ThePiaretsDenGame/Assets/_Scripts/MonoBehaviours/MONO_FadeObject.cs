using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MONO_FadeObject : MonoBehaviour {

	private Color startColor;
	private Color targetColor;
	private float objectAlpha;
	private float startTime;
	public float fadeDuration = 1.0f;

	private float isFading;

	public void StartFade(float targetAlpha)
	{
		
		StartCoroutine (Fade (targetAlpha));
	}



	private IEnumerator Fade(float targetAlpha)
	{
		objectAlpha = gameObject.GetComponent<MeshRenderer> ().material.color.a;
		// Set the fading flag to true so the FadeAndSwitchScenes coroutine won't be called again.
		isFading = true;

		/* Calculate how fast the object should fade based on it's current alpha, 
		 * it's final alpha and how long it has to change between the two.
		 */
		float fadeSpeed = Mathf.Abs (objectAlpha - targetAlpha) / fadeDuration;

		// While the CanvasGroup hasn't reached the final alpha yet...
		while (!Mathf.Approximately (objectAlpha, targetAlpha))
		{
			gameObject.GetComponent<MeshRenderer> ().material.color = Color.Lerp (startColor, targetColor, fadeSpeed);


			// Wait for a frame then continue.
			yield return null;
		}

		// Set the flag to false since the fade has finished.
		isFading = false;
	}

}
