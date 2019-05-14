using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MONO_FadeObject : MonoBehaviour {
	public float fadeDuration = 1.0f;
	[Tooltip("Mesh rendererers of the object to fade")]
	public MeshRenderer[] meshRenderers;
	[Tooltip("Skinned mesh rendererers of the object to fade")]
	public SkinnedMeshRenderer[] skinnedMeshRenderers;


	private Color[] startColor;
	private Color[] skinnedStartColor;
	private Color[] targetColor;
	private Color[] skinnedTargetColor;
	private float currentAlpha;
	private float targetAlpha;
	private bool isFading;

	void Start()
	{
		startColor = new Color[meshRenderers.Length];
		skinnedStartColor = new Color[skinnedMeshRenderers.Length];
	}


	public void StartFade(float newAlpha)
	{
		targetAlpha = newAlpha;
		currentAlpha = (newAlpha + 1) % 1;
		if (meshRenderers == null && skinnedMeshRenderers == null) 
		{
			Debug.LogError ("You cannot fade an object without at least one mesh renderer or skinned mesh renderer.");
			return;
		}
		for (int i = 0; i < meshRenderers.Length; i++) 
		{
			startColor[i] = meshRenderers [i].material.color;
			targetColor [i] = startColor [i];
			targetColor [i].a = targetAlpha;
		}
		for (int i = 0; i < skinnedMeshRenderers.Length; i++) 
		{
			skinnedStartColor[i] = skinnedMeshRenderers [i].material.color;
			targetColor [i] = startColor [i];
			targetColor [i].a = targetAlpha;
		}

		isFading = true;
	}


	void Update()
	{
		float fadeSpeed = (Mathf.Abs (currentAlpha - targetAlpha)) / fadeDuration;

		// While the CanvasGroup hasn't reached the final alpha yet...
		while (!Mathf.Approximately (currentAlpha, targetAlpha))
		{
			for (int i = 0; i < meshRenderers.Length; i++)
			{
				Color objectColor = meshRenderers [i].material.color;
				objectColor = Color.Lerp (objectColor, targetColor[i], fadeSpeed);
				meshRenderers [i].material.color = objectColor;
			}
			for (int i = 0; i < skinnedMeshRenderers.Length; i++)
			{
				skinnedMeshRenderers[i].material.color = Color.Lerp (skinnedMeshRenderers[i].material.color, targetColor[i], fadeSpeed);
			}

			if (meshRenderers [0] != null)
			{
				currentAlpha = meshRenderers [0].material.color.a;
			} 
			else if (skinnedMeshRenderers [0] != null) 
			{
				currentAlpha = skinnedMeshRenderers [0].material.color.a;
			} 
			else
			{
				Debug.Log ("There is no mesh renderer or skinned mesh renderer to be faded.");
			}

		}

		isFading = false;
	}


}
