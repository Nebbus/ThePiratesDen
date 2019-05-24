using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//==========================================================
// this is becus of that the MONO_GlowObject 
// turns off it self all the time, so that
// the eventsystem loses the higligt funktions
// so its broken out to this
//==========================================================
public class MONO_GlowObject_higlightAll : MonoBehaviour {
   
    //EVENTS
    public Action<MONO_EventManager.EventParam> onHigligtAll;
    private Action<MONO_EventManager.EventParam> offHigligtAll;

    private MONO_GlowObject higligthScript;


    private void Awake()
    {
        onHigligtAll = new Action<MONO_EventManager.EventParam>(OnHigligtAll);
        offHigligtAll = new Action<MONO_EventManager.EventParam>(OffHigligtAll);
    }

    private void Start()
    {
        higligthScript = GetComponent<MONO_GlowObject>();
    }



    private void OnEnable()
    {
        MONO_EventManager.StartListening(MONO_EventManager.onHiglightAllInteractablesInScene_NAME, onHigligtAll);
        MONO_EventManager.StartListening(MONO_EventManager.offHiglightAllInteractablesInScene_NAME, offHigligtAll);
    }
    private void OnDisable()
    {
        MONO_EventManager.StopListening(MONO_EventManager.onHiglightAllInteractablesInScene_NAME, onHigligtAll);
        MONO_EventManager.StopListening(MONO_EventManager.offHiglightAllInteractablesInScene_NAME, offHigligtAll);
       
    }


    //==========================================================
    // Events for higligting all interactables in scene
    //==========================================================
    private void OnHigligtAll(MONO_EventManager.EventParam evntParam)
    {  
        higligthScript.HigligtON(true);
    }
    private void OffHigligtAll(MONO_EventManager.EventParam evntParam)
    {
        higligthScript.HigligtOFF(true);
    }


}
