using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class MONO_OnSceneStart : MonoBehaviour {


    public UnityEvent toRunOnSceneStart = new UnityEvent();

    public UnityEvent startFlowChart = new UnityEvent();

    [Space]
    public UnityEvent toRunOnSceneShutDown = new UnityEvent();

    private Action<MONO_EventManager.EventParam> sceneStartSetup;
    private Action<MONO_EventManager.EventParam> sceneShutDownSetup;


    private void Awake()
    {
        sceneStartSetup     = new Action<MONO_EventManager.EventParam>(RunThisAtStart);
        sceneShutDownSetup = new Action<MONO_EventManager.EventParam>(RunThisAtShutDown);
    }
    private void Start()
    {
        if (toRunOnSceneStart == null)
        {
            toRunOnSceneStart = new UnityEvent();
        }
        if(startFlowChart == null)
        {
            startFlowChart = new UnityEvent();
        }
        if(toRunOnSceneShutDown == null)
        {
            toRunOnSceneShutDown = new UnityEvent();
        }
    }

    private void OnEnable()
    {
        MONO_EventManager.StartListening(MONO_EventManager.sceneStartSetup_NAME, sceneStartSetup);
        MONO_EventManager.StartListening(MONO_EventManager.sceneShutdownSetup_NAME, sceneShutDownSetup);
    }
    private void OnDisable()
    {
        MONO_EventManager.StopListening(MONO_EventManager.sceneStartSetup_NAME, sceneStartSetup);
        MONO_EventManager.StopListening(MONO_EventManager.sceneShutdownSetup_NAME, sceneShutDownSetup);
    }



    /// <summary>
    /// Runs froug all the nessesary set up parts of a scene [
    /// this will be called from the scene manager after the fade is done, 
    /// and after load variables has ben done]
    /// </summary>
    /// <param name="eventParam"> not used, but needed for the MONO_EventManager</param>
    public void RunThisAtStart(MONO_EventManager.EventParam eventParam)
    {
        toRunOnSceneStart.Invoke();
        startFlowChart.Invoke();
    }
    public void RunThisAtShutDown(MONO_EventManager.EventParam eventParam)
    {
        toRunOnSceneShutDown.Invoke();
    }


}
