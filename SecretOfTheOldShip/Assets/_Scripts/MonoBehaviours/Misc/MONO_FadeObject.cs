using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MONO_FadeObject : MonoBehaviour {
	public float fadeDuration = 1.0f;
	[Tooltip("Mesh renderer of the object to fade")]
	public MeshRenderer meshRenderer;

	private Color startColor;
	private Color targetColor;
	private float objectAlpha;
	private float targetAlpha;
	private bool isFading;



	public void StartFade(float newAlpha)
	{
		targetAlpha = newAlpha;
		startColor = meshRenderer.material.color;
		targetColor = startColor;
		targetColor.a = targetAlpha;
		objectAlpha = startColor.a;
		isFading = true;
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
