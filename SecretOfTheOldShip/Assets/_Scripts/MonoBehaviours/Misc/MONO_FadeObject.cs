using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MONO_FadeObject : MonoBehaviour {
	public float fadeDuration = 1.0f;

	private Color startColor;
	private Color targetColor;
	private float objectAlpha;
	private float targetAlpha;
	private bool isFading;


	public void StartFade(float newAlpha)
	{
		targetAlpha = newAlpha;
		startColor = gameObject.GetComponentInChildren<MeshRenderer> ().material.color;
		targetColor = startColor;
		targetColor.a = targetAlpha;
		objectAlpha = startColor.a;
		isFading = true;
		//StartCoroutine (Fade (targetAlpha));
	}


	void Update()
	{
		float fadeSpeed = Mathf.Abs (objectAlpha - targetAlpha) / fadeDuration;

		// While the CanvasGroup hasn't reached the final alpha yet...
		while (!Mathf.Approximately (objectAlpha, targetAlpha))
		{
			Color objectColor = Color.Lerp (startColor, targetColor, fadeSpeed);
			objectAlpha = objectColor.a;
			gameObject.GetComponent<MeshRenderer> ().material.color = objectColor;
		}
		isFading = false;
	}


}
