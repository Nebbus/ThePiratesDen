using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MONO_Menus : MonoBehaviour {

	[HideInInspector]
	public bool menuOpen = true;
    public GameObject menuButton;
	public MONO_SceneManager sceneManager;
	public MONO_ReactionCollection mainMenuSound;
	public MONO_ReactionCollection pausMenuSound;


	public enum menu {main, paus, settings};


	private menu latestMenu;
	[SerializeField]
	private GameObject mainMenu;
	[SerializeField]
	private GameObject pausMenu;
	[SerializeField]
	private GameObject settingsMenu;
	[SerializeField]
	private GameObject inventory;

	void Start()
	{
		if(latestMenu == null){
			latestMenu = menu.paus; 	//paus menu is the default menu
		}
	}


	public void StartNewGame()
	{
		CloseMenu ();
		ChangeLatestMenu (pausMenu);
		sceneManager.ChangeScene ("Scene1_outside", true);
		inventory.SetActive (true);
	}

	public void LoadLatestGame()
	{
		
	}

	public void ChangeLatestMenu(GameObject menuObject)
	{
		if (menuObject.name == "Main") 
		{
			latestMenu = menu.main;
		} 
		else if (menuObject.name == "Paus")
		{
			latestMenu = menu.paus;	
		}

	}

	/// <summary>
	/// Opens the menu. 
	/// This is based on which menu that was opened most recently, so 
	/// that you can go back to whatever menu you came from when going 
	/// back from, ex, the settings menu. The latest menu is set to be the 
	/// paus menu by default, since this is the menu you open from in game.
	/// </summary>
	public void OpenMenu()
	{
        menuOpen = true;
		menuButton.SetActive (false);

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
    /// Doesn't really close the menu, that is done by setting the specified 
    /// menu object to non-active with the help of an on-click-event. 
    /// This function just sets the boolean variable to false.
    /// </summary>
    public void CloseMenu()
    {
        menuOpen = false;
        menuButton.SetActive(true);
    }

	/// <summary>
	/// Plays the click sound depending on whether you're in the main menu or the paus menu
	/// </summary>
	public void PlayClickSound()
	{
		switch (latestMenu) 
		{
		case menu.main:
			mainMenuSound.React ();
			break;

		case menu.paus:
			pausMenuSound.React ();
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
