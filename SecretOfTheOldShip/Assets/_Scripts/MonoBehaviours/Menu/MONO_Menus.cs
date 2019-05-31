using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MONO_Menus : MonoBehaviour {

    public string sceneToLoadOnStartNewGame = "Scene1_outside";
	[Space]
	public MONO_SceneManager   		sceneManager;
	public MONO_LevelMusicManager 	audioManager;
	[HideInInspector]
	public MONO_SaveAndLoad     	monoSaveAndLoad;
	[HideInInspector]
	public MONO_IntroManager 		introManager;


    [HideInInspector]
	public bool menuOpen = true;
	[Space]
    public Button loadButton;
    public MONO_Inventory       monoInventory;



	[Space]
	public MONO_CustomMouseCursor cursor;
	[Tooltip("The UI text object in main settings menu for cursor speed.")]
	public Text cursorSpeedMain;
	[Tooltip("The UI text object in pause settings menu for cursor speed.")]
	public Text cursorSpeedPause;
	public float cursorSpeedMinValue = 0;
	public float cursorSpeedMaxValue = 3000;


	[Space]
	public MONO_ReactionCollection mainMenuSound;
	public MONO_ReactionCollection pauseMenuSound;

	[Space]
	public Text musicVolumeMain;
	public Text musicVolumePaus;
	public Text soundVolumeMain;
	public Text soundVolumePaus;
//	public Text voiceVolumeMain;		//Activate in case we use volume controls for voices
//	public Text voiceVolumePaus;		//Activate in case we use volume controls for voices


	public enum menu {main, pause, settings};

	private menu latestMenu;
	[Space]
	[SerializeField]
	private GameObject mainMenu;
	[SerializeField]
	private GameObject pauseMenu;
	[SerializeField]
	private GameObject settingsMenu;
	//[SerializeField]
	//private GameObject inventory;
	private MONO_Fade fader;
	//private MONO_Wait waitManager;




	void Start()
	{
		SetTextComponents ();

		fader = sceneManager.gameObject.GetComponent<MONO_Fade> ();
		monoSaveAndLoad = sceneManager.gameObject.GetComponent<MONO_SaveAndLoad> ();

        latestMenu = menu.main; 	//main menu is the menu the game is started with

        // Sets if the load button shuld be usavle
        MONO_SaveAndLoad.SaveData data  = monoSaveAndLoad.GetData;
        loadButton.interactable         = data.hasSAveData;

    }

	//--------------------------------------------------------------------------------
	// Methods started on button clicks
	//--------------------------------------------------------------------------------

	public void StartNewGame()
	{
		//float delay = sceneManager.GetComponent<MONO_Fade> ().fadeDuration;
		CloseMenu ();
		ChangeLatestMenu (pauseMenu);
       
		sceneManager.ChangeScene (sceneToLoadOnStartNewGame, false, false, false, true);

		//StartCoroutine (WaitSomeTime(delay));
		mainMenu.SetActive (false);
		//inventory.SetActive (true);
	}

	/// <summary>
	/// Loads the last game.
	/// </summary>
    public void LoadLastGame()
    {
        MONO_SaveAndLoad.SaveData data = monoSaveAndLoad.GetData;
        CloseMenu();
        ChangeLatestMenu(pauseMenu);

        SOBJ_Item[] items = monoSaveAndLoad.ReconstructInventoryItems(data.itemsInInentory);
        // gets all the inventory items from last game
        for(int i = 0; i < data.itemsInInentory.Length; i++)
        {
            monoInventory.AddItem(items[i]);
        }

        //Update all condition
        data.conditions.uppdatAllCondition();

        sceneManager.ChangeScene(data.currentScene, true, true, false, true);

        mainMenu.SetActive(false);
    }

	/// <summary>
	/// Start intro
	/// </summary>
	public void StartIntro()
	{
		MONO_SaveAndLoad.SaveData data = monoSaveAndLoad.GetData;
		CloseMenu();
		ChangeLatestMenu(pauseMenu);

		SOBJ_Item[] items = monoSaveAndLoad.ReconstructInventoryItems(data.itemsInInentory);
		// gets all the inventory items from last game
		for(int i = 0; i < data.itemsInInentory.Length; i++)
		{
			monoInventory.AddItem(items[i]);
		}

		//Update all condition
		data.conditions.uppdatAllCondition();

		sceneManager.ChangeScene("Intro", true, true, false, true);

		mainMenu.SetActive(false);
	}


	//--------------------------------------------------------------------------------
	//	Methods used when opening and closing menues
	//--------------------------------------------------------------------------------

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
	/// pause menu by default, since this is the menu you open from in game.
	/// </summary>
	public void OpenMenu()
	{
        menuOpen = true;

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
    }


	/// <summary>
	/// Opens the main menu.
	/// </summary>
	public void OpenMainMenu()
	{
		sceneManager.ChangeScene ("MainMenu", false, false, true, true);
		pauseMenu.SetActive (false);
		mainMenu.SetActive (true);
	}

	//--------------------------------------------------------------------------------
	//	Audio Stuff
	//--------------------------------------------------------------------------------

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


	public void ChangeMusicVolume(float offset)
	{
		Text tempText = musicVolumeMain;
		tempText.text = audioManager.changeMusicVolume (offset);
		musicVolumeMain.text = tempText.text;
		musicVolumePaus.text = tempText.text;
	}

	public void ChangeSFXVolume(float offset)
	{
		Text tempText = musicVolumeMain;
		tempText.text = audioManager.changeSFXVolume (offset);
		soundVolumeMain.text = tempText.text;
		soundVolumePaus.text = tempText.text;
	}


	//--------------------------------------------------------------------------------
	// Helpful Methods
	//--------------------------------------------------------------------------------

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
	/// This is practically only for the bouns menu
	/// </summary>
	/// <param name="newScene"></param>
	public void fromMenuSetUpp(string newScene)
	{
		//float delay = sceneManager.GetComponent<MONO_Fade> ().fadeDuration;
		CloseMenu();
		ChangeLatestMenu(pauseMenu);
		sceneManager.ChangeScene(newScene, false, true, true, false);
		//StartCoroutine (WaitSomeTime(delay));
		mainMenu.SetActive(false);
		//inventory.SetActive (true);
	}

	/// <summary>
	/// Sets the text components.
	/// </summary>
	private void SetTextComponents()
	{
		float speed = Mathf.InverseLerp (cursorSpeedMinValue, cursorSpeedMaxValue, cursor.CursorSpeed);
		speed = Mathf.Round (speed * 10f) / 10f;
		cursor.CursorSpeed = Mathf.Lerp (cursorSpeedMinValue, cursorSpeedMaxValue, speed);
		speed *= 100f;
		cursorSpeedMain.text = speed.ToString();
		cursorSpeedPause.text = speed.ToString();

		musicVolumeMain.text = audioManager.GetVolume (audioManager.audioTypeMusic);
		musicVolumePaus.text = audioManager.GetVolume (audioManager.audioTypeMusic);
		soundVolumeMain.text = audioManager.GetVolume (audioManager.audioTypeSFX);
		soundVolumePaus.text = audioManager.GetVolume (audioManager.audioTypeSFX);
	/*	voiceVolumeMain;
		voiceVolumePaus;*/
	}

	/// <summary>
	/// Changes the cursor speed by float parameter.
	/// </summary>
	/// <param name="offset">The amount cursor speed will change.</param>
	public void ChangeCursorSpeed(float offset)
	{
		float speedPercentage = Mathf.InverseLerp (cursorSpeedMinValue, cursorSpeedMaxValue, cursor.CursorSpeed);
		speedPercentage *= 100f;
		speedPercentage = Mathf.Round (speedPercentage);
		speedPercentage += offset;

		if (speedPercentage <  1|| speedPercentage > 100) 
		{
			return;
		}

		cursorSpeedMain.text = speedPercentage.ToString ();
		cursorSpeedPause.text = speedPercentage.ToString ();

		speedPercentage *= 0.01f;
		cursor.CursorSpeed = Mathf.Lerp(cursorSpeedMinValue, cursorSpeedMaxValue, speedPercentage);
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
