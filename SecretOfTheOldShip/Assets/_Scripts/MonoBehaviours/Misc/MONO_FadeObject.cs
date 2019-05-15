using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MONO_FadeObject : MonoBehaviour {
	[Tooltip("Is the object visible when first activated?")]
	public bool visible;
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


	void Awake()
	{
		//If the object isn't meant to be visible at first, all renderers alpha has to be set to 0.
		if(!visible)
		{
			for (int i = 0; i < meshRenderers.Length; i++) 
			{
				meshRenderers [i].material.color = new Color(meshRenderers [i].material.color.r, 
															meshRenderers [i].material.color.g,
															meshRenderers [i].material.color.b, 
															0);
			}
			for (int i = 0; i < skinnedMeshRenderers.Length; i++) 
			{
				skinnedMeshRenderers [i].material.color =  new Color(skinnedMeshRenderers [i].material.color.r, 
																		skinnedMeshRenderers [i].material.color.g,
																		skinnedMeshRenderers [i].material.color.b, 
																		0);
			}
			for(int i = 0; i < spriteRenderers.Length; i++)
			{
				spriteRenderers [i].material.color =   new Color(spriteRenderers [i].material.color.r, 
																	spriteRenderers [i].material.color.g,
																	spriteRenderers [i].material.color.b, 
																	0);
			}
		}
	}


	void Start()
	{
		//create new startColor arrays of the same size as the corresponing renderer array
		meshStartColor = new Color[meshRenderers.Length];
		skinnedMeshStartColor = new Color[skinnedMeshRenderers.Length];
		spriteStartColor = new Color[spriteRenderers.Length];
	}

	/// <summary>
	/// Initiate the fading method.
	/// </summary>
	/// <param name="newAlpha">The final alpha of the fade. 
	/// If NewAlpha is 0, the object will fade out of the scene. 
	/// If NewAlpha is 1, the object will fade in to the scene.</param>
	public void StartFade(float newAlpha)
	{
		targetAlpha = newAlpha;
		currentAlpha = (newAlpha + 1) % 1;	//should not be like this, but it is
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

		// While the renderers hasn't reached the final alpha
		while (!Mathf.Approximately (currentAlpha, targetAlpha))
		{
			//Lerp the alpha of each object
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
				spriteRenderers[i].color = new Color (1.0f, 1.0f, 1.0f, Mathf.Lerp(currentAlpha,targetAlpha,fadeSpeed));
				//spriteRenderers[i].color = Color.Lerp (spriteRenderers[i].material.color, spriteTargetColor[i], fadeSpeed);
				//spriteRenderers[i].material.color = Color.Lerp (spriteRenderers[i].material.color, spriteTargetColor[i], fadeSpeed);
			}


			//Update current alpha
			if (meshRenderers.Length > 0)
			{
				currentAlpha = meshRenderers [0].material.color.a;
			}
			else if (skinnedMeshRenderers.Length > 0) 
			{
				currentAlpha = skinnedMeshRenderers [0].material.color.a;
			} 
			else if (spriteRenderers.Length > 0) 
			{
				currentAlpha = spriteRenderers [0].material.color.a;
			}
			else
			{
				Debug.Log ("There is no renderer to be faded.");
				currentAlpha = targetAlpha;
			}
		}

		isFading = false;
	}


}
