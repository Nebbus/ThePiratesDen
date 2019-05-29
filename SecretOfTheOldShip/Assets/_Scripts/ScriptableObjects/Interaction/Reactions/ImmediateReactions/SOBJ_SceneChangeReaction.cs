using UnityEngine;

public class SOBJ_SceneChangeReaction : SOBJ_DelayedReaction
{
	public string sceneToBeLoaded;

    private bool setStartPosition = false;// only used from the load game button in the menu
    public bool handelInputAfterFade = true;
	public bool changingToMainMenu = false;

    private GameObject sceneManager;
	private MONO_SceneManager managerScript;


	protected override void SpecificInit()
	{
		sceneManager = GameObject.FindWithTag ("SceneManager");
	}

	protected override void ImmediateReaction()
	{
		sceneManager.GetComponent<MONO_SceneManager> ().ChangeScene(sceneToBeLoaded, setStartPosition, handelInputAfterFade, changingToMainMenu);
	}
}

