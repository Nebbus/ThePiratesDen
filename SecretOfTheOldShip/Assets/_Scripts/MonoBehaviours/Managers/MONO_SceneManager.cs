using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;

public class MONO_SceneManager : MonoBehaviour {

    public enum changeScenType {SCENEtoSCENE, MENUtoSCENE, SCENEtoMENU};


	public string startScene = "MainMenu";						//The name of the starting scene as a string.
	public Canvas canvas;							//The canvas holding the black image we fade to.
	//public StudioParameterTrigger musicTrigger;
	//public StudioParameterTrigger ambienceTrigger;
    public MONO_SaveAndLoad saveLoad;

    public Camera loadCamera;

	public Fungus.Flowchart fadeFlowchart;
	[HideInInspector]
	public float fadeDuration;

	[Space]
	public GameObject openMenuButton;
	public GameObject hintButton;
	public GameObject inventory;
	[HideInInspector]

    public bool testarDeta = false;


//==========================================================
// Everything that has with handle inupt to do
//==========================================================
    [SerializeField]
    private bool handleInput = true;                 // Whether input is currently being handled.

    /// <summary>
    /// Get set for if input should be handled
    /// </summary>
    public bool getSetHandleInput
    {
        get
        {
           

            return handleInput;
        }
        set
        {
          

            handleInput = value;
        }
    }

    //EVENTS
    private Action<MONO_EventManager.EventParam> setInputHandling;

    //==========================================================
    // Setup and turn off stuff
    //==========================================================
    private void Awake()
    {
        setInputHandling = new Action<MONO_EventManager.EventParam>(SetHandleInput);
		fadeDuration    = fadeFlowchart.GetFloatVariable ("fadeDuration");
    }
    private void OnEnable()
    {
        MONO_EventManager.StartListening(MONO_EventManager.setInputHandling_NAME, setInputHandling);
    }
    private void OnDisable()
    {
        MONO_EventManager.StopListening(MONO_EventManager.setInputHandling_NAME, setInputHandling);

    }

    private void SetHandleInput(MONO_EventManager.EventParam evntParam)
    {
        handleInput = evntParam.param4;
    }

	private void Start () 
	{
        
   	    loadCamera.gameObject.SetActive(true);

    	 //Load first scene, set start position for player and fade in.
        //yield return StartCoroutine(LoadAndSetScene(startScene));

        //yield return StartCoroutine(OneSceneStartUpp());
		StartCoroutine(LoadAndFadeMainMenu());

        loadCamera.gameObject.SetActive(false);
		fadeFlowchart.ExecuteBlock ("FadeFromBlack");
	}

	private IEnumerator LoadAndFadeMainMenu()
	{
		yield return StartCoroutine(LoadAndSetScene(startScene));

		yield return StartCoroutine(OneSceneStartUpp());
	}


    /// <summary>
    /// Changes the scene.
    /// </summary>
    /// <param name="sceneName">Scene to load.</param>
    /// <param name="sceneName">Sets if the start position shuld be set.</param>
    //public void ChangeScene(string sceneName, bool setStartPos, bool handelnputAfterFade, bool goingToMainMenu, bool save, bool activateMenuButton = true, bool activateHintButton = true, bool activateInventory = true)
    //{
    //	StartCoroutine (FadeAndLoad (sceneName, setStartPos, handelnputAfterFade, goingToMainMenu, save, activateMenuButton, activateHintButton, activateInventory));
    //   }

    /// <summary>
    /// Disable input, fade out, then switch scene before fading in.
    /// </summary>
    /// <param name="sceneName">Scene to load.</param>
    /// <param name="setStartPos">Sets if the start position shuld be set(onlyUsed then loading.</param>
    /// <param name="handelUnputAfterFade">Sets if the start position shuld be set.</param>
    //private IEnumerator FadeAndLoad(string sceneName, bool setStartPos, bool handelInputAfterFade, bool goingToMainMenu, bool save, bool activateMenuButton = true, bool activateHintButton = true, bool activateInventory = true)
    //{
    //    //disable input and fade out.
    //    handleInput = false;
    //    fadeFlowchart.ExecuteBlock("FadeToBlack");
    //    yield return new WaitForSeconds(fadeDuration);
    //    MONO_SaveAndLoad.SaveData data = saveLoad.GetData;

    //    if (save)
    //    {
    //        if (goingToMainMenu)
    //        {
    //            saveLoad.handleSave(true);
    //        }
    //        else
    //        {
    //            saveLoad.handleSave(true, sceneName);
    //        }
    //    }

    //    saveLoad.SaveInGame();

    //    yield return StartCoroutine(OneSceneShutdown());


    //    //Unload old scene and load the new one.
    //    StartCoroutine(UnloadAndUnsetScene());
    //    loadCamera.gameObject.SetActive(true);// TEMP CAMERA=================================================================================================================
    //    yield return StartCoroutine(LoadAndSetScene(sceneName));
    //    loadCamera.gameObject.SetActive(false); // TEMP CAMERA=================================================================================================================

    //    saveLoad.handLoad(true);
    //    saveLoad.SaveInGame();


    //    if (activateMenuButton)
    //    {
    //        openMenuButton.SetActive(true);
    //    }
    //    if (activateHintButton)
    //    {
    //        hintButton.SetActive(true);
    //    }
    //    if (activateInventory)
    //    {
    //        inventory.SetActive(true);
    //    }


    //    yield return StartCoroutine(OneSceneStartUpp());

    //    //fade in and enable input.
    //    fadeFlowchart.ExecuteBlock("FadeFromBlack");



    //    //setsStartPosition, then loading from menu 
    //    if (setStartPos)
    //    {

    //        GameObject player = GameObject.FindGameObjectWithTag("Player");

    //        if (player != null)
    //        {
    //            player.transform.position = data.playerPosData.getPos;// = new Vector3(0f,1f,2f);
    //            player.transform.rotation = data.playerPosData.getRotation;
    //        }
    //    }


    //    yield return new WaitForSeconds(fadeDuration);



    //    handleInput = handelInputAfterFade;
    //}



    public void ChangeScene(string sceneName, bool loadedGame, bool handelnputAfterFade, bool saveDataBefforChangeGame, bool loadDataAfterLoad, changeScenType typeOffFade)
    {

        switch (typeOffFade)
        {
            case changeScenType.SCENEtoSCENE:
                StartCoroutine(FadeSceneToScene(sceneName, handelnputAfterFade, saveDataBefforChangeGame, loadDataAfterLoad));
                break;
            case changeScenType.MENUtoSCENE:
                StartCoroutine(FadeMenuToScene(sceneName, loadedGame, handelnputAfterFade));
                break;
            case changeScenType.SCENEtoMENU:
                StartCoroutine(FadeSceneToMenu(sceneName, saveDataBefforChangeGame, loadDataAfterLoad));
                break;
            default:
                break;
        } 
    }


    private IEnumerator FadeMenuToScene(string sceneName, bool loadedGame, bool handelInputAfterFade)
    {
        //disable input and fade out.
        handleInput = false;
        fadeFlowchart.ExecuteBlock("FadeToBlack");
        yield return new WaitForSeconds(fadeDuration);


        MONO_SaveAndLoad.SaveData data = saveLoad.GetData;

        yield return StartCoroutine(OneSceneShutdown());

        StartCoroutine(UnloadAndUnsetScene());

        yield return StartCoroutine(LoadAndSetScene(sceneName));

        saveLoad.handLoad(true);

        //setsStartPosition, then loading from menu 
        if (loadedGame)
        {

            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                player.transform.position = data.playerPosData.getPos;// = new Vector3(0f,1f,2f);
                player.transform.rotation = data.playerPosData.getRotation;
            }
        }
        saveLoad.SaveInGame();

        yield return StartCoroutine(OneSceneStartUpp());

        //fade in and enable input.
        fadeFlowchart.ExecuteBlock("FadeFromBlack");



        yield return new WaitForSeconds(fadeDuration);


        handleInput = handelInputAfterFade;
    }


    private IEnumerator FadeSceneToScene(string sceneName, bool handelInputAfterFade, bool saveDataBefforChangeGame, bool loadDataAfterLoad)
    {
        //disable input and fade out.
        handleInput = false;
        fadeFlowchart.ExecuteBlock("FadeToBlack");
        yield return new WaitForSeconds(fadeDuration);


        MONO_SaveAndLoad.SaveData data = saveLoad.GetData;
        if (saveDataBefforChangeGame)
        {
            saveLoad.SaveInGame();
        }


        yield return StartCoroutine(OneSceneShutdown());


        StartCoroutine(UnloadAndUnsetScene());


        yield return StartCoroutine(LoadAndSetScene(sceneName));

        if (loadDataAfterLoad)
        {
            saveLoad.handLoad(true);
            saveLoad.SaveInGame();
        }


        yield return StartCoroutine(OneSceneStartUpp());

        //fade in and enable input.
        fadeFlowchart.ExecuteBlock("FadeFromBlack");

        yield return new WaitForSeconds(fadeDuration);

        handleInput = handelInputAfterFade;
    }


    private IEnumerator FadeSceneToMenu(string sceneName, bool saveDataBefforChangeGame, bool loadDataAfterLoad)
    {
        //disable input and fade out.
        handleInput = false;
        fadeFlowchart.ExecuteBlock("FadeToBlack");
        yield return new WaitForSeconds(fadeDuration);

        if (saveDataBefforChangeGame)
        {
            saveLoad.SaveInGame();
        }

        yield return StartCoroutine(OneSceneShutdown());

        //Unload old scene and load the new one.
        StartCoroutine(UnloadAndUnsetScene());

        yield return StartCoroutine(LoadAndSetScene(sceneName));

        if (loadDataAfterLoad)
        {
            saveLoad.handLoad(true);
        }

        yield return StartCoroutine(OneSceneStartUpp());

        //fade in and enable input.
        fadeFlowchart.ExecuteBlock("FadeFromBlack");

        yield return new WaitForSeconds(fadeDuration);

    }



    /// <summary>
    /// Load in the new scene ontop of the persistent scene and activate.
    /// </summary>
    /// <param name="sceneName">Scene to load.</param>
    private IEnumerator LoadAndSetScene(string sceneName)
	{
        loadCamera.gameObject.SetActive(true);

        yield return SceneManager.LoadSceneAsync (sceneName, LoadSceneMode.Additive);
		Scene scene = SceneManager.GetSceneAt (SceneManager.sceneCount - 1);
		SceneManager.SetActiveScene (scene);

        loadCamera.gameObject.SetActive(false);
    }

	/// <summary>
	/// Unloads the previous scene.
	/// </summary>
	private IEnumerator UnloadAndUnsetScene()
	{

      yield return SceneManager.UnloadSceneAsync (SceneManager.GetActiveScene ().buildIndex);
	}


    /// <summary>
    /// 
    /// </summary>
    private IEnumerator OneSceneShutdown()
    {
        // runs the shut down in the old scene
        MONO_EventManager.EventParam paramFiller = new MONO_EventManager.EventParam();
        MONO_EventManager.TriggerEvent(MONO_EventManager.sceneShutdownSetup_NAME, paramFiller);

        yield return !MONO_EventManager.isNotWorking ;
    }

    /// <summary>
    ///
    /// </summary>
    private IEnumerator OneSceneStartUpp()
    {
        // runs the shut down in the old scene
        MONO_EventManager.EventParam paramFiller = new MONO_EventManager.EventParam();
        MONO_EventManager.TriggerEvent(MONO_EventManager.sceneStartSetup_NAME, paramFiller);

        yield return !MONO_EventManager.isNotWorking;
    }


}
