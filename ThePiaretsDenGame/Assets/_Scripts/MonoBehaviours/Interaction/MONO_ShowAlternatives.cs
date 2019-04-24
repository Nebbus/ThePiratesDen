using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MONO_ShowAlternatives : MonoBehaviour {

	public List<GameObject> children;

	private MONO_PlayerMovement player;
	private MONO_SceneManager sceneManager;


	void Start()
	{
		sceneManager = FindObjectOfType<MONO_SceneManager> ();
		player = FindObjectOfType<MONO_PlayerMovement> ();
	}

	/*void LateUpdate()
	{
		if (player.cancelInteraction) 
		{
			HideAlternatives ();
		}

		//if(player.OnGroundClick())
	}*/

	public void ShowAlternatives()
	{
		sceneManager.SetHandleInput (false);
		foreach (Transform child in transform) 
		{
			children.Add (child.gameObject);
		}

		foreach (GameObject child in children) 
		{
			child.SetActive (true);
		}
	}

	public void HideAlternatives()
	{
		foreach(GameObject child in children)
		{
			child.SetActive (false);
			children.Remove (gameObject);
		}
		//sceneManager.SetHandleInput (true);

	}
}
