using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MONO_SimpleSceneChange : MonoBehaviour {
	public string SceneToLoad;
  
	void Start()
	{
	    //SceneManager.LoadScene (SceneToLoad);

        StartCoroutine(LoadAndSetScene());

    }


    /// <summary>
    /// Load in the new scene ontop of the persistent scene and activate.
    /// </summary>
    /// <param name="sceneName">Scene to load.</param>
    private IEnumerator LoadAndSetScene()
    {


        //yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
         SceneManager.LoadSceneAsync(SceneToLoad, LoadSceneMode.Additive);
        yield return SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Additive);

        Scene scene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        yield return StartCoroutine(OneSceneStartUpp());
        SceneManager.SetActiveScene(scene);
        SceneManager.UnloadSceneAsync(0);
  
    }






    /// <summary>
    /// 
    /// </summary>
    private IEnumerator OneSceneShutdown()
    {
        // runs the shut down in the old scene
        MONO_EventManager.EventParam paramFiller = new MONO_EventManager.EventParam();
        MONO_EventManager.TriggerEvent(MONO_EventManager.sceneShutdownSetup_NAME, paramFiller);

        yield return !MONO_EventManager.isNotWorking;
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
