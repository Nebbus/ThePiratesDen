using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MONO_FadeObject : MonoBehaviour {
	public float fadeDuration = 1.0f;
	[Tooltip("Mesh rendererers of the object to fade")]
	public MeshRenderer[] meshRenderers;
	[Tooltip("Skinned mesh rendererers of the object to fade")]
	public SkinnedMeshRenderer[] skinnedMeshRenderers;
	[Tooltip("Sprites to fade")]
	public SpriteRenderer[] spriteRenderers;


	private Color[] meshStartColor;
	private Color[] meshTargetColor;
	private Color[] skinnedMeshStartColor;
	private Color[] skinnedMeshTargetColor;
	private Color[] spriteStartColor;
	private Color[] spriteTargetColor;
	private float currentAlpha;
	private float targetAlpha;
	private bool isFading;

	void Start()
	{
		meshStartColor = new Color[meshRenderers.Length];
		skinnedMeshStartColor = new Color[skinnedMeshRenderers.Length];
		spriteStartColor = new Color[spriteRenderers.Length];
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
			meshStartColor[i] = meshRenderers [i].material.color;
			meshTargetColor [i] = meshStartColor [i];
			meshTargetColor [i].a = targetAlpha;
		}
		for (int i = 0; i < skinnedMeshRenderers.Length; i++) 
		{
			skinnedMeshStartColor[i] = skinnedMeshRenderers [i].material.color;
			meshTargetColor [i] = meshStartColor [i];
			meshTargetColor [i].a = targetAlpha;
		}
		for(int i = 0; i < spriteRenderers.Length; i++)
		{
			spriteStartColor [i] = spriteRenderers [i].material.color;
			spriteTargetColor[i] = spriteStartColor[i];
			spriteTargetColor [i].a = targetAlpha;
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
				meshRenderers[i].material.color = Color.Lerp (meshRenderers[i].material.color, meshTargetColor[i], fadeSpeed);
			}
			for (int i = 0; i < skinnedMeshRenderers.Length; i++)
			{
				skinnedMeshRenderers[i].material.color = Color.Lerp (skinnedMeshRenderers[i].material.color, meshTargetColor[i], fadeSpeed);
			}
			for (int i = 0; i < spriteRenderers.Length; i++)
			{
				spriteRenderers[i].material.color = Color.Lerp (spriteRenderers[i].material.color, spriteTargetColor[i], fadeSpeed);
			}



			if (meshRenderers [0] != null)
			{
				currentAlpha = meshRenderers [0].material.color.a;
			}
			else if (skinnedMeshRenderers [0] != null) 
			{
				currentAlpha = skinnedMeshRenderers [0].material.color.a;
			} 
			else if (spriteRenderers [0] != null) 
			{
				currentAlpha = spriteRenderers [0].material.color.a;
			}
			else
			{
				Debug.Log ("There is no renderer to be faded.");
			}

		}

		isFading = false;
	}


}
