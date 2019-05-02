using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MONO_FadeObject : MonoBehaviour {
	public float fadeDuration = 1.0f;

	private Color startColor;
	private Color targetColor;
	private float objectAlpha;
	private bool isFading;

	public void StartFade(float targetAlpha)
	{
		StartCoroutine (Fade (targetAlpha));
	}


	private IEnumerator Fade(float targetAlpha)
	{
		startColor = gameObject.GetComponent<MeshRenderer> ().material.color;
		targetColor = startColor;
		targetColor.a = targetAlpha;
		objectAlpha = startColor.a;


		isFading = true;

		/* Calculate how fast the object should fade based on it's current alpha, 
		 * it's final alpha and how long it has to change between the two.
		 */
		float fadeSpeed = Mathf.Abs (objectAlpha - targetAlpha) / fadeDuration;

		// While the CanvasGroup hasn't reached the final alpha yet...
		while (!Mathf.Approximately (objectAlpha, targetAlpha))
		{
			Color objectColor = Color.Lerp (startColor, targetColor, fadeSpeed);
			objectAlpha = objectColor.a;
			gameObject.GetComponent<MeshRenderer> ().material.color = objectColor;

			// Wait for a frame then continue.
			yield return null;
		}

		// Set the flag to false since the fade has finished.
		isFading = false;
	}

}
