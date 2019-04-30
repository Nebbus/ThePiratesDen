using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MONO_IntroManager : MonoBehaviour {
	private MONO_SceneManager sceneManager;
	private MONO_Menus menuManager;
	public GameObject mainMenu;

	private MONO_MenuSelect temp;

	void Awake()
	{
		sceneManager = FindObjectOfType<MONO_SceneManager> ();
		menuManager = FindObjectOfType <MONO_Menus> ();
	}


	void Start () {
		
	}

	void LateStart()
	{
		menuManager.ChangeLatestMenu (mainMenu);
		//menuManager.OpenMenu ();
	}
}
