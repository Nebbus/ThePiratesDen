using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

/// <summary>
/// simple class for on click 
/// </summary>
public class MONO_SimpleInteractable : MONO_interactionBase {

    [Serializable]
    public struct otherReactions
    {
        public UnityEvent myOnStartEvent;

        public UnityEvent myOnAwaykeEvent;

        public UnityEvent myOnScriptEnableEvent;

        public UnityEvent myOnScriptDisableEvent;

        public void init()
        {
            if (myOnStartEvent == null)
            {
                myOnStartEvent = new UnityEvent();
            }
            if (myOnAwaykeEvent == null)
            {
                myOnAwaykeEvent = new UnityEvent();
            }
            if (myOnScriptEnableEvent == null)
            {
                myOnScriptEnableEvent = new UnityEvent();
            }
            if (myOnScriptDisableEvent == null)
            {
                myOnScriptDisableEvent = new UnityEvent();
            }
        }
    }

    public UnityEvent myClickEvent;

    public UnityEvent myHoverkEvent;

    public otherReactions otherReactionsTriggers;
    

    

    

    

    private void Awake()
    {
        if (myClickEvent == null)
        {
            myClickEvent = new UnityEvent();
        }
        if (myHoverkEvent == null)
        {
            myHoverkEvent = new UnityEvent();
        }
        otherReactionsTriggers.init();
        otherReactionsTriggers.myOnAwaykeEvent.Invoke();
    }


    void OnEnable()
    {
        otherReactionsTriggers.myOnScriptEnableEvent.Invoke();
    }

    void OnDisable()
    {
        otherReactionsTriggers.myOnScriptDisableEvent.Invoke();
    }

    public override void OnClick()
    {
        myClickEvent.Invoke();
    }

    public override void OnHovor()
    {
        myHoverkEvent.Invoke();
    }
}
