using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MONO_Menus : MonoBehaviour {

	public enum menu {main, paus, settings};

	private menu latestMenu;
	[SerializeField]
	private GameObject mainMenu;
	[SerializeField]
	private GameObject pausMenu;
	[SerializeField]
	private GameObject settingsMenu;

	void Start()
	{
		latestMenu = menu.paus; 	//paus menu is the default menu
	}

	public void StartNewGame()
	{
		
	}


	public void OpenMenu()
	{
		switch (latestMenu) {
		case menu.main:
			mainMenu.SetActive (true);
			break;

		case menu.paus:
			pausMenu.SetActive (true);
			break;

		default:
			break;
		}
	}

	/// <summary>
	/// Quit game.
	/// If we're in the unity editor, exit play mode.
	/// If we're running the application, quit the application.
	/// </summary>
	public void Quit()
	{
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#else
			Application.Quit();
		#endif
	}

}
