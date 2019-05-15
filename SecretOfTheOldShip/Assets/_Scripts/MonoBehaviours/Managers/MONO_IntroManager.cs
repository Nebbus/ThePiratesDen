using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MONO_IntroManager : MonoBehaviour {
	
	public GameObject introBook;
	public GameObject[] introDrawings;

	private MONO_SceneManager sceneManager;
	private MONO_Menus menuManager;
	private GameObject mainMenu;

	void Awake()
	{
		sceneManager = FindObjectOfType<MONO_SceneManager> ();
		menuManager = FindObjectOfType <MONO_Menus> ();

	}

	void Start()
	{
		menuManager.introManager = gameObject.GetComponent<MONO_IntroManager>();
	}


	public void StartIntro()
	{
		introBook.SetActive (true);
		introBook.GetComponent<MONO_ReactionCollection> ().React();
	}


	public void StartGame()
	{
		menuManager.StartGame ();
	}
}
