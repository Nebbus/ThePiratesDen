using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;

public class MONO_SceneManager : MonoBehaviour {
	//[HideInInspector]
	public bool handleInput = true;                 // Whether input is currently being handled.
	public string startScene;						//The name of the starting scene as a string.
	public Canvas canvas;							//The canvas holding the black image we fade to.
	public StudioParameterTrigger musicTrigger;
	public StudioParameterTrigger ambienceTrigger;
    public MONO_SaveAndLoad saveLoad;

    public Camera loadCamera;

	private MONO_Fade fade;

	/// <summary>
	/// Get the handleinput bool.
	/// </summary>
	public bool GetHandleInput()
	{
		return handleInput;
	}

	/// <summary>
	/// Set the handleinput bool.
	/// </summary>
	/// <param name="set">If set to <c>true</c> set.</param>
	public void SetHandleInput(bool set)
	{
		handleInput = set;
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
    /// Disable input, fade out, then switch scene before fading in.
    /// </summary>
    /// <param name="sceneName">Scene to load.</param>
    /// <param name="sceneName">Sets if the start position shuld be set.</param>
    private IEnumerator FadeAndLoad(string sceneName, bool setStartPos, bool handelUnputAfterFade)
	{
		//disable input and fade out.
		handleInput = false;
		fade.Fade(1f);
		yield return new WaitForSeconds (fade.fadeDuration);

		//Unload old scene and load the new one.
		StartCoroutine(UnloadAndUnsetScene());
        loadCamera.gameObject.SetActive(true);// TEMP CAMERA=================================================================================================================
        yield return StartCoroutine(LoadAndSetScene(sceneName));
        loadCamera.gameObject.SetActive(false); // TEMP CAMERA=================================================================================================================
        if (setStartPos)
        {
            SetPlayerStartPosition();
			//SetGhostStartPosition ();
        }
        //anropa FMOD

        //fade in and enable input.
        fade.Fade (0f);
		yield return new WaitForSeconds (fade.fadeDuration);
      

        handleInput = handelUnputAfterFade;
	}

	/// <summary>
	/// Sets the player start position.
	/// </summary>
	private void SetPlayerStartPosition()
	{
		GameObject player              = GameObject.FindGameObjectWithTag ("Player");
		GameObject pos                 = GameObject.Find ("PlayerStartPosition");
		GameObject ghost               = GameObject.FindGameObjectWithTag ("Ghost");
		GameObject gpos                = GameObject.Find ("GhostStartPosition");
		player.transform.position      = pos.transform.position;
		player.transform.localRotation = pos.transform.localRotation;
		ghost.transform.position       = gpos.transform.position;
		ghost.transform.localRotation  = gpos.transform.localRotation;
		ghost.SetActive (false);
	}

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
	private void ChangeMusic(float scene)
	{
		//This needs to be done with a musician.
		musicTrigger.TriggerParameters();
		ambienceTrigger.TriggerParameters ();
	}
}
