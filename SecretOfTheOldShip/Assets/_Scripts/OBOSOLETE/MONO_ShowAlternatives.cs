using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MONO_ShowAlternatives : MonoBehaviour {

	public List<GameObject> 	children;			//All the options available for this object

	private MONO_PlayerMovement player;	
	private MONO_SceneManager 	sceneManager;


	void Start()
	{
		//Find the scripts needed for this script to work.
		sceneManager = FindObjectOfType<MONO_SceneManager> ();
		player 	= FindObjectOfType<MONO_PlayerMovement> ();
	}

	/// <summary>
	/// Show all interactionalternatives available for this object.
	/// </summary>
	public void ShowAlternatives()
	{
		sceneManager.getSetHandleInput = false;
		foreach (Transform child in transform) 
		{
			children.Add (child.gameObject);
		}

		foreach (GameObject child in children) 
		{
			child.SetActive (true);
		}
	}

	/// <summary>
	/// Hide all interactionalternatives available for this object.
	/// </summary>
	public void HideAlternatives()
	{
		foreach(GameObject child in children)
		{
			child.SetActive (false);
			children.Remove (gameObject);
		}
	}
}
