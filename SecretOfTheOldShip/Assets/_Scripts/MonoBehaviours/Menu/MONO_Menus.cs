using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MONO_Menus : MonoBehaviour {

	public string mainMenuSceneName = "MainMenu";
    public string startSceneName = "Scene1_outside";
	public string introSceneName = "Intro";
	public string achievementsSceneName = "Achievements";
	[Space]
	public MONO_SceneManager   		sceneManager;
	public MONO_LevelMusicManager 	audioManager;
	[HideInInspector]
	public MONO_SaveAndLoad     	monoSaveAndLoad;
	[HideInInspector]
	public MONO_IntroManager 		introManager;

	[Space]
	public Fungus.Flowchart fadeFlowchart;
	private float fadeDuration;

    [HideInInspector]
	public bool menuOpen = true;
	[Space]
    public Button 			loadButton;
    public MONO_Inventory 	monoInventory;


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
	private GameObject mainSettingsMenu;
	[SerializeField]
	private GameObject pauseSettingsMenu;
	[SerializeField]
	private GameObject bonusMenu;



	void Start()
	{
		SetTextComponents ();
		monoSaveAndLoad = sceneManager.gameObject.GetComponent<MONO_SaveAndLoad> ();

        latestMenu = menu.main; 	//main menu is the menu the game is started with

        // Sets if the load button shuld be usable
        MONO_SaveAndLoad.SaveData data  = monoSaveAndLoad.GetData;
        loadButton.interactable         = data.hasSAveData;

		fadeDuration = sceneManager.fadeDuration;
    }


    public void StartNewGame()
    {
		GameObject[] objectsToActivate = { };
		GameObject[] objectsToDeactivate = { mainMenu };
        CloseMenu();
        ChangeLatestMenu(pauseMenu);

        monoSaveAndLoad.StartNewGAme();
        string newScene                             = startSceneName;
        bool handelInputAfterLoad                   = true;
        bool saveDataBefforChangeGame               = true;// not used her
        bool loadDataAfterLoad                      = true;// not used her
        bool loadedGame                             = false;
        MONO_SceneManager.changeScenType changeType = MONO_SceneManager.changeScenType.MENUtoSCENE;
        sceneManager.ChangeScene (newScene, loadedGame, handelInputAfterLoad, saveDataBefforChangeGame, loadDataAfterLoad, changeType);
		StartCoroutine (WaitAndActivate (fadeDuration, objectsToActivate, objectsToDeactivate));
    }

    /// <summary>
    /// Loads the last game.
    /// </summary>
    public void LoadLastGame()
    {
		GameObject[] objectsToActivate = { };
		GameObject[] objectsToDeactivate = { mainMenu };

        MONO_SaveAndLoad.SaveData data = monoSaveAndLoad.GetData;
        CloseMenu();
        ChangeLatestMenu(pauseMenu);

        monoInventory.ClerInventory();

        SOBJ_Item[] items = monoSaveAndLoad.ReconstructInventoryItems(data.itemsInInentory);

        // gets all the inventory items from last game
        for (int i = 0; i < data.itemsInInentory.Length; i++)
        {
            monoInventory.AddItem(items[i]);
        }

        //Update all condition
        data.conditions.uppdatAllCondition();


        string newScene               = data.currentScene;
        bool handelInputAfterLoad     = true;
        bool saveDataBefforChangeGame = true;// not used her
        bool loadDataAfterLoad        = true;// not used her
        bool loadedGame               = true;
        MONO_SceneManager.changeScenType changeType = MONO_SceneManager.changeScenType.MENUtoSCENE;
        sceneManager.ChangeScene(newScene, loadedGame, handelInputAfterLoad, saveDataBefforChangeGame, loadDataAfterLoad, changeType);
		StartCoroutine (WaitAndActivate (fadeDuration, objectsToActivate, objectsToDeactivate));
    }

	/// <summary>
	/// Start intro
	/// </summary>
	public void StartIntro()
	{
		MONO_SaveAndLoad.SaveData data = monoSaveAndLoad.GetData;
		CloseMenu();
		ChangeLatestMenu(pauseMenu);

		GameObject[] objectsToActivate = { };
		GameObject[] objectsToDeactivate = { mainMenu };


        string newScene               = introSceneName;
        bool handelInputAfterLoad     = false; 
        bool saveDataBefforChangeGame = false;// not used her
        bool loadDataAfterLoad        = false;// not used her
        bool loadedGame               = false;// not used her
        MONO_SceneManager.changeScenType changeType = MONO_SceneManager.changeScenType.MENUtoSCENE;
        sceneManager.ChangeScene(newScene, loadedGame, handelInputAfterLoad, saveDataBefforChangeGame, loadDataAfterLoad, changeType);

		StartCoroutine (WaitAndActivate (fadeDuration, objectsToActivate, objectsToDeactivate));

    }

	public void GoToAchievements()
	{
		
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
        ChangeLatestMenu(mainMenu);
		GameObject[] objectsToActivate = { mainMenu};
		GameObject[] objectsToDeactivate = { pauseMenu };

        string newScene               = mainMenuSceneName;
        bool handelInputAfterLoad     = true;// not used her
        bool saveDataBefforChangeGame = true;
        bool loadDataAfterLoad        = true;
        bool loadedGame               = true;// not used her
        MONO_SceneManager.changeScenType changeType = MONO_SceneManager.changeScenType.SCENEtoMENU;
        sceneManager.ChangeScene(newScene, loadedGame, handelInputAfterLoad, saveDataBefforChangeGame, loadDataAfterLoad, changeType);

		StartCoroutine (WaitAndActivate (fadeDuration, objectsToActivate, objectsToDeactivate));
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
		Text tempText = soundVolumeMain;
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
	IEnumerator WaitAndActivate(float fadeDuration, GameObject[] objectsToActivate, GameObject[] objectsToDeactivate
							/*, string newScene, bool loadedGame, bool handelInputAfterLoad, bool saveDataBefforChangeGame, bool loadDataAfterLoad, MONO_SceneManager.changeScenType changeType*/)
	{
		yield return new WaitForSeconds (fadeDuration);

		if (objectsToDeactivate != null) {
			for (int i = 0; i < objectsToDeactivate.Length; i++)
			{
				objectsToDeactivate [i].SetActive (false);
			}
		}

		if (objectsToActivate != null) {
			for (int i = 0; i < objectsToActivate.Length; i++) 
			{
				objectsToActivate [i].SetActive (true);
			}
		}

		//sceneManager.ChangeScene(newScene, loadedGame, handelInputAfterLoad, saveDataBefforChangeGame, loadDataAfterLoad, changeType);
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
