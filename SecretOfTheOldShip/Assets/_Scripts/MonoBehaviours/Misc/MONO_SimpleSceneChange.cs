using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MONO_SimpleSceneChange : MonoBehaviour {
	public string SceneToLoad;

	void Start()
	{
		SceneManager.LoadScene (SceneToLoad);
	}
}
