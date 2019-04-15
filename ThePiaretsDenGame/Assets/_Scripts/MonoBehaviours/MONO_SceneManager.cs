using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MONO_SceneManager : MonoBehaviour {

	public string startScene;
	public Canvas canvas;
	public int fadeDelay;

	private MONO_Fade fade;

	IEnumerator Start () 
	{
		fade = FindObjectOfType<MONO_Fade>();
		yield return StartCoroutine( LoadAndSetScene (startScene));
		SetPlayerStartPosition ();
		fade.Fade (0f);

	}


	public IEnumerator LoadAndSetScene(string sceneName)
	{
		yield return SceneManager.LoadSceneAsync (sceneName, LoadSceneMode.Additive);
		Scene scene = SceneManager.GetSceneAt (SceneManager.sceneCount - 1);
		SceneManager.SetActiveScene (scene);
	}

	public IEnumerator UnloadAndUnsetScene()
	{
		yield return SceneManager.UnloadSceneAsync (SceneManager.GetActiveScene ().buildIndex);
	}

	public void FadeAndLoad(string sceneName)
	{
		fade.Fade(1f);
		//FMOD stuuuuffs
		StartCoroutine(UnloadAndUnsetScene());
		StartCoroutine(LoadAndSetScene(sceneName));
		fade.Fade (0f);
	}

	private void SetPlayerStartPosition()
	{
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		GameObject pos = GameObject.Find ("StartPosition");
		player.transform.position = pos.transform.position;
		player.transform.rotation = pos.transform.rotation;
	}
	

	void Update () 
	{
		
	}
}
