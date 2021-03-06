﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MONO_IntroManager : MonoBehaviour {
	
	public GameObject introBook;
	public GameObject[] introDrawings;

	private MONO_SceneManager sceneManager;
	private MONO_Menus menuManager;
//	private MONO_Wait waitManager;
	private GameObject mainMenu;



	void Awake()
	{
		sceneManager = FindObjectOfType<MONO_SceneManager> ();
		menuManager = FindObjectOfType<MONO_Menus> ();
//		waitManager = FindObjectOfType<MONO_Wait> ();
	}

	void Start()
	{
		menuManager.introManager = gameObject.GetComponent<MONO_IntroManager>();
	}


	public void InitiateIntro()
	{
		Debug.Log ("Initiating intro");
		//sceneManager.GetComponent<MONO_Fade> ().Fade (0);


		float delay = sceneManager.GetComponent<MONO_Fade> ().fadeDuration;
		StartCoroutine (FadeAndWait (delay));
		this.gameObject.GetComponent<MONO_ReactionCollection> ().React ();
	}


	public void StartGame()
	{
		menuManager.StartNewGame();
	}



	/// <summary>
	/// Starts a new  wait for seconds.
	/// </summary>
	/// <returns>The some time.</returns>
	/// <param name="seconds">Seconds.</param>
	IEnumerator FadeAndWait(float seconds)
	{
		//sceneManager.GetComponent<MONO_Fade> ().Fade (1);
		introBook.SetActive (true);
		sceneManager.GetComponent<MONO_Fade> ().Fade (0);
		yield return new WaitForSeconds (seconds);
	}
}
