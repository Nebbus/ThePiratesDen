using UnityEngine;

public class SOBJ_SceneChangeReaction : SOBJ_Reaction
{
	public string sceneToBeLoaded;

	private GameObject sceneManager;
	private MONO_SceneManager managerScript;


	protected override void SpecificInit()
	{
		sceneManager = GameObject.FindWithTag ("SceneManager");
	}

	protected override void ImmediateReaction()
	{
		sceneManager.GetComponent<MONO_SceneManager> ().ChangeScene(sceneToBeLoaded);
	}
}

