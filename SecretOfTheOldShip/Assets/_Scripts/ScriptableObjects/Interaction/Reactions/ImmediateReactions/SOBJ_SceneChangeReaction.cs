using UnityEngine;

public class SOBJ_SceneChangeReaction : SOBJ_DelayedReaction
{
	//

 //   public bool setStartPosition    = false;// only used from the load game button in the menu
 //   public bool handelInputAfterFade = true;
	//public bool changingToMainMenu = false;
 //   public bool saveOnChange = true;

    public MONO_SceneManager.changeScenType changeType = MONO_SceneManager.changeScenType.SCENEtoSCENE;
    public string sceneToBeLoaded;

    [Tooltip("Used if the changeType = MenuToScene or SceneToScene")]
    public bool handelInputAfterLoad     = true;// not used her

    [Tooltip("Used if the changeType = SceneToMenue or SceneToScene")]
    public bool saveDataBefforChangeGame = true;

    [Tooltip("Used if the changeType = SceneToMenue or SceneToScene")]
    public bool loadDataAfterLoad        = true;

    [Tooltip("Used if changeType = MenuToScene, if true: sets the players position to the position loaded from data")]
    public bool loadedGame           = true;// not used her

    private GameObject sceneManager;
	private MONO_SceneManager managerScript;


	protected override void SpecificInit()
	{
		sceneManager = GameObject.FindWithTag ("SceneManager");
	}

	protected override void ImmediateReaction()
	{


        sceneManager.GetComponent<MONO_SceneManager>().ChangeScene(sceneToBeLoaded, loadedGame, handelInputAfterLoad, saveDataBefforChangeGame, loadDataAfterLoad, changeType);


        //sceneManager.GetComponent<MONO_SceneManager> ().ChangeScene(sceneToBeLoaded, setStartPosition, handelInputAfterFade, changingToMainMenu, saveOnChange, activateMenuButton, activateHintButton, activateInventory);
	}
}

