using UnityEngine;

public class SOBJ_SceneChangeReaction : SOBJ_DelayedReaction
{
	public string sceneToBeLoaded;
    public bool setStartPosition = false;


	private GameObject sceneManager;
	private MONO_SceneManager managerScript;


	protected override void SpecificInit()
	{
		sceneManager = GameObject.FindWithTag ("SceneManager");
	}

	protected override void ImmediateReaction()
	{
		sceneManager.GetComponent<MONO_SceneManager> ().ChangeScene(sceneToBeLoaded, setStartPosition);
	}
}

