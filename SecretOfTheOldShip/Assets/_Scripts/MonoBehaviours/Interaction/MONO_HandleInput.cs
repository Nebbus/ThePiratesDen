using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MONO_HandleInput : MonoBehaviour {

	private MONO_SceneManager scenemanager;

	void Start () 
	{
		scenemanager = FindObjectOfType<MONO_SceneManager> ();
	}
	
	public void HandleInput(bool handle)
	{
		scenemanager.getSetHandleInput = handle; 
	}
}
