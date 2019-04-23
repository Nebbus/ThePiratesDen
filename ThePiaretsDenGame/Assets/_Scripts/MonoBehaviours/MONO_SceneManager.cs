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

	private MONO_Fade fade;

	private IEnumerator Start () 
	{
		//Find the instance holding the code for fading.
		fade = FindObjectOfType<MONO_Fade>();

		//Load first scene, set start position for player and fade in.
		yield return StartCoroutine( LoadAndSetScene (startScene));
		SetPlayerStartPosition ();
		fade.Fade (0f);

	}

	/// <summary>
	/// Changes the scene.
	/// </summary>
	/// <param name="sceneName">Scene to load.</param>
	public void ChangeScene(string sceneName)
	{
		StartCoroutine (FadeAndLoad (sceneName));
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
	private IEnumerator FadeAndLoad(string sceneName)
	{
		//disable input and fade out.
		handleInput = false;
		fade.Fade(1f);
		yield return new WaitForSeconds (fade.fadeDuration);

		//Unload old scene and load the new one.
		StartCoroutine(UnloadAndUnsetScene());
		StartCoroutine(LoadAndSetScene(sceneName));	

		//anropa FMOD

		//fade in and enable input.
		fade.Fade (0f);
		yield return new WaitForSeconds (fade.fadeDuration);
		handleInput = true;
	}

	/// <summary>
	/// Sets the player start position.
	/// </summary>
	private void SetPlayerStartPosition()
	{
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		GameObject pos = GameObject.Find ("StartPosition");
		player.transform.position = pos.transform.position;
		player.transform.localRotation = pos.transform.localRotation;
	}

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
