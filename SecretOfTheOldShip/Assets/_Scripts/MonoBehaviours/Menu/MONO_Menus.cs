using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MONO_Menus : MonoBehaviour {

	[HideInInspector]
	public bool menuOpen = true;
    public GameObject menuButton;
    public GameObject hintButton;
    public MONO_SceneManager sceneManager;
	[HideInInspector]
	public MONO_IntroManager introManager;

	[Space]
	public MONO_CustomMouseCursor cursor;
	[Tooltip("The UI text object in main settings menu for cursor speed.")]
	public Text cursorSpeedMain;
	[Tooltip("The UI text object in pause settings menu for cursor speed.")]
	public Text cursorSpeedPause;

	[Space]
	public MONO_ReactionCollection mainMenuSound;
	public MONO_ReactionCollection pauseMenuSound;
	public enum menu {main, pause, settings};

	private menu latestMenu;
	[Space]
	[SerializeField]
	private GameObject mainMenu;
	[SerializeField]
	private GameObject pauseMenu;
	[SerializeField]
	private GameObject settingsMenu;
	[SerializeField]
	private GameObject inventory;
	private MONO_Fade fader;
	//private MONO_Wait waitManager;



	private float timerTime;
	private bool timerUpdating;


	void Start()
	{
		cursorSpeedMain.text = cursor.CursorSpeed.ToString ();
		cursorSpeedPause.text = cursor.CursorSpeed.ToString ();

		fader = sceneManager.gameObject.GetComponent<MONO_Fade> ();
		//waitManager = sceneManager.gameObject.GetComponent<MONO_Wait> ();

		if(latestMenu == null){
			latestMenu = menu.main; 	//main menu is the menu the game is started with
		}
	}



	public void StartGame()
	{
		float delay = sceneManager.GetComponent<MONO_Fade> ().fadeDuration;
		CloseMenu ();
		ChangeLatestMenu (pauseMenu);
		sceneManager.ChangeScene ("Scene1_outside", true, false);

		//StartCoroutine (WaitSomeTime(delay));
		mainMenu.SetActive (false);
		inventory.SetActive (true);
	}
		
	public void LoadLatestGame()
	{
		
	}


	void Update()
	{
		if (timerUpdating) 
		{
			timerTime += Time.deltaTime;
		}
	}


	public void StartIntro()
	{
		menuOpen = false;

		float delay = sceneManager.gameObject.GetComponent<MONO_Fade> ().fadeDuration;
		timerTime = 0;
		timerUpdating = true;
		//fader.Fade (1);		//fades screen to black
		StartCoroutine (FadeAndWait());

	
		//mainMenu.SetActive (false);
		//introManager.InitiateIntro ();
	}

	/// <summary>
	/// Changes the latest menu variable. Used to help the system  knowing which menu to open.
	/// </summary>
	/// <param name="menuObject">Gameobject of the current menu.</param>
	public void ChangeLatestMenu(GameObject menuObject)
	{
		if (menuObject.name == "Main") 
		{
			latestMenu = menu.main;
		} 
		else if (menuObject.name == "Pause")
		{
			latestMenu = menu.pause;	
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
        hintButton.SetActive(false);


        switch (latestMenu) {
		case menu.main:
			mainMenu.SetActive (true);
			break;

		case menu.pause:
			pauseMenu.SetActive (true);
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
        hintButton.SetActive(true);
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

		case menu.pause:
			pauseMenuSound.React ();
			break;

		default:
			break;

		}
	}

	/// <summary>
	/// Changes the cursor speed by float parameter.
	/// </summary>
	/// <param name="offset">The amount cursor speed will change.</param>
	public void ChangeCursorSpeed(float offset)
	{
		cursor.CursorSpeed = cursor.CursorSpeed + offset;
		cursorSpeedMain.text = cursor.CursorSpeed.ToString ();
		cursorSpeedPause.text = cursor.CursorSpeed.ToString ();
	}



	/// <summary>
	/// Starts a new  wait for seconds.
	/// </summary>
	/// <returns>The some time.</returns>
	/// <param name="seconds">Seconds.</param>
	IEnumerator FadeAndWait()
	{
		if (!fader.isFading) 
		{
			fader.Fade (1);
			yield return new WaitForSeconds (fader.fadeDuration);
			//mainMenu.SetActive (false);
			//introManager.InitiateIntro ();
			
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
