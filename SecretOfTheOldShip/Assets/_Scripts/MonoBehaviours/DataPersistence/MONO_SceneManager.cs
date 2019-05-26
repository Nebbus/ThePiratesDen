using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;

public class MONO_SceneManager : MonoBehaviour {

	public string startScene;						//The name of the starting scene as a string.
	public Canvas canvas;							//The canvas holding the black image we fade to.
	//public StudioParameterTrigger musicTrigger;
	//public StudioParameterTrigger ambienceTrigger;
    public MONO_SaveAndLoad saveLoad;

    public Camera loadCamera;

	private MONO_Fade fade;

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
    // Setup and turn of stuff
    //==========================================================
    private void Awake()
    {
        setInputHandling = new Action<MONO_EventManager.EventParam>(SetHandleInput);
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

    private IEnumerator Start () 
	{
		//Find the instance holding the code for fading.
		fade = FindObjectOfType<MONO_Fade>();

        loadCamera.gameObject.SetActive(true);

        //Load first scene, set start position for player and fade in.
        yield return StartCoroutine( LoadAndSetScene (startScene));

        loadCamera.gameObject.SetActive(false);

        //SetPlayerStartPosition ();
        fade.Fade (0f);
       // Debug.log("Afsef");
    }
   
    
    /// <summary>
    /// Changes the scene.
    /// </summary>
    /// <param name="sceneName">Scene to load.</param>
    /// <param name="sceneName">Sets if the start position shuld be set.</param>
    public void ChangeScene(string sceneName, bool setStartPos, bool handelUnputAfterFade)
	{
        StartCoroutine (FadeAndLoad (sceneName, setStartPos, handelUnputAfterFade));
    }


    /// <summary>
    /// Disable input, fade out, then switch scene before fading in.
    /// </summary>
    /// <param name="sceneName">Scene to load.</param>
    /// <param name="setStartPos">Sets if the start position shuld be set(onlyUsed then loading.</param>
    /// <param name="handelUnputAfterFade">Sets if the start position shuld be set.</param>
    private IEnumerator FadeAndLoad(string sceneName, bool setStartPos, bool handelUnputAfterFade)
    {
        //disable input and fade out.
        handleInput = false;
        fade.Fade(1f);
        yield return new WaitForSeconds(fade.fadeDuration);
        MONO_SaveAndLoad.SaveData data = saveLoad.GetData;

       saveLoad.handleSave(true, sceneName);

        //Unload old scene and load the new one.
        StartCoroutine(UnloadAndUnsetScene());
        loadCamera.gameObject.SetActive(true);// TEMP CAMERA=================================================================================================================
        yield return StartCoroutine(LoadAndSetScene(sceneName));
        loadCamera.gameObject.SetActive(false); // TEMP CAMERA=================================================================================================================

        saveLoad.handLoad(true);

        // runs the start upp in the new scene
        MONO_EventManager.EventParam paramFiller = new MONO_EventManager.EventParam();
        MONO_EventManager.TriggerEvent(MONO_EventManager.sceneStartSetup_NAME, paramFiller);

       

        //fade in and enable input.
        fade.Fade(0f);

        if (setStartPos)
        {
            GameObject.FindGameObjectWithTag("Player").transform.position = data.playerPosData.getPos;// = new Vector3(0f,1f,2f);
            GameObject.FindGameObjectWithTag("Player").transform.rotation = data.playerPosData.getRotation;
        }


        yield return new WaitForSeconds(fade.fadeDuration);

       
        
        handleInput = handelUnputAfterFade;
    }



    /// <summary>
    /// Load in the new scene ontop of the persistent scene and activate.
    /// </summary>
    /// <param name="sceneName">Scene to load.</param>
    private IEnumerator LoadAndSetScene(string sceneName)
	{
		yield return SceneManager.LoadSceneAsync (sceneName, LoadSceneMode.Additive);
		Scene scene = SceneManager.GetSceneAt (SceneManager.sceneCount - 1);
		SceneManager.SetActiveScene (scene);
    }

	/// <summary>
	/// Unloads the previous scene.
	/// </summary>
	private IEnumerator UnloadAndUnsetScene()
	{
		yield return SceneManager.UnloadSceneAsync (SceneManager.GetActiveScene ().buildIndex);
	}

  




	/// <summary>
	/// Sets the player start position.
	/// </summary>
	//private void SetPlayerStartPosition()
	//{
	//	GameObject player              = GameObject.FindGameObjectWithTag ("Player");
	//	GameObject pos                 = GameObject.Find ("PlayerStartPosition");
	//	GameObject ghost               = GameObject.FindGameObjectWithTag ("Ghost");
	//	GameObject gpos                = GameObject.Find ("GhostStartPosition");
	//	player.transform.position      = pos.transform.position;
	//	player.transform.localRotation = pos.transform.localRotation;
	//	ghost.transform.position       = gpos.transform.position;
	//	ghost.transform.localRotation  = gpos.transform.localRotation;
	//	ghost.SetActive (false);
	//}

	/// <summary>
	/// Sets the ghost start position.
	/// </summary>
	/*private void SetGhostStartPosition()
	{
		GameObject ghost              = GameObject.FindGameObjectWithTag ("Ghost");
		GameObject gpos                 = GameObject.Find ("GhostStartPosition");
		ghost.transform.position      = gpos.transform.position;
		ghost.transform.localRotation = gpos.transform.localRotation;
	}*/

	/// <summary>
	/// Changes the music.
	/// </summary>
	//private void ChangeMusic(float scene)
	//{
	//	//This needs to be done with a musician.
	//	musicTrigger.TriggerParameters();
	//	ambienceTrigger.TriggerParameters ();
	//}
}
