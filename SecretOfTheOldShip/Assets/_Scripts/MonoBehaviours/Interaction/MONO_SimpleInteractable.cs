using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

/// <summary>
/// simple class for on click 
/// </summary>
public class MONO_SimpleInteractable : MONO_InteractionBase, Fungus.IWriterListener
{


  


    [Serializable]
    public struct otherReactions
    {
        public UnityEvent myOnStartEvent;

        public UnityEvent myOnAwaykeEvent;

        public UnityEvent myOnScriptEnableEvent;

        public UnityEvent myOnScriptDisableEvent;

        public UnityEvent myOnScriptGamobjectDisabeld;

        public UnityEvent myOnScriptGamobjectEnable;

        public GameObject trackedObject;

        public bool gamobjectEnalde ;

        public void init()
        {
            if (trackedObject)
            {
                gamobjectEnalde = trackedObject.activeSelf;
            }


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
            if (myOnScriptGamobjectDisabeld == null)
            {
                myOnScriptGamobjectDisabeld = new UnityEvent();
            }
            if (myOnScriptGamobjectEnable == null)
            {
                myOnScriptGamobjectEnable = new UnityEvent();
            }
        }
    }

    [Serializable]
    public struct FungusReactions
    {
        public UnityEvent FungusOnStart;

        public UnityEvent FungusOnEnd;

        public UnityEvent FungusOnGlyph;

        public UnityEvent FungusOnInput;

        public UnityEvent FungusOnResume;

        public UnityEvent FungusOnPause;

        public UnityEvent FungusOnVoiceover;

        public void init()
        {
            if (FungusOnStart == null)
            {
                FungusOnStart = new UnityEvent();
            }
            if (FungusOnEnd == null)
            {
                FungusOnEnd = new UnityEvent();
            }
            if (FungusOnGlyph == null)
            {
                FungusOnGlyph = new UnityEvent();
            }
            if (FungusOnInput == null)
            {
                FungusOnInput = new UnityEvent();
            }
            if (FungusOnPause == null)
            {
                FungusOnPause = new UnityEvent();
            }
            if (FungusOnVoiceover == null)
            {
                FungusOnVoiceover = new UnityEvent();
            }
            if(FungusOnResume == null)
            {
                FungusOnResume = new UnityEvent();
            }
        }
    }

    public bool canMoveMouseOver = false;

    [Space]
    [Space]
    public UnityEvent myClickEvent;

    public UnityEvent myHoverEnterdEvent;

    public UnityEvent myHoverEvent;

    public UnityEvent myHoverExitEvent;

    public otherReactions otherReactionsTriggers;

    public FungusReactions fungusRactionsTriggers;

    //===============================================================================================
    //  Other reaction triggers
    //===============================================================================================


    public void Start()
    {
        {
            if (canMoveMouseOver && gameObject.GetComponent(typeof(UnityEngine.UI.Selectable)) == null)
            {
                gameObject.AddComponent(typeof(UnityEngine.UI.Selectable));
     
            }
        }
    }

        private void Update()
    {
        

        if (otherReactionsTriggers.trackedObject && otherReactionsTriggers.trackedObject.activeSelf)
        {
            if (!otherReactionsTriggers.gamobjectEnalde)
            {
                otherReactionsTriggers.gamobjectEnalde = true;
                otherReactionsTriggers.myOnScriptGamobjectEnable.Invoke();
            }
        }
        else
        {
            if (otherReactionsTriggers.gamobjectEnalde)
            {
                otherReactionsTriggers.gamobjectEnalde = false;
                otherReactionsTriggers.myOnScriptGamobjectDisabeld.Invoke();
            }
        }
    }

    private void Awake()
    {
        if (myClickEvent == null)
        {
            myClickEvent = new UnityEvent();
        }
        if (myHoverEvent == null)
        {
            myHoverEvent = new UnityEvent();
        }
        fungusRactionsTriggers.init();
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

//===============================================================================================
//  Main reaction triggers
//===============================================================================================

    public override void OnClick()
    {
        myClickEvent.Invoke();
    }

    public override void OnHoverEnterd()
    {
        myHoverEnterdEvent.Invoke();
    }

    public override void OnHover()
    {
        myHoverEvent.Invoke();
    }

    public override void OnHoverExit()
    {
        myHoverExitEvent.Invoke();
    }



//===============================================================================================
// Fungus Events
//===============================================================================================
  
    public void OnEnd(bool stopAudio)
    {
        fungusRactionsTriggers.FungusOnEnd.Invoke();
    }

    public void OnGlyph()
    {
        fungusRactionsTriggers.FungusOnGlyph.Invoke();
    }

    public void OnInput()
    {
        fungusRactionsTriggers.FungusOnInput.Invoke();
    }

    public void OnPause()
    {
        fungusRactionsTriggers.FungusOnPause.Invoke();
    }

    public void OnResume()
    {
        fungusRactionsTriggers.FungusOnResume.Invoke();
    }

    public void OnStart(AudioClip audioClip)
    {
        fungusRactionsTriggers.FungusOnStart.Invoke(); ;
    }

    public void OnVoiceover(AudioClip voiceOverClip)
    {
        fungusRactionsTriggers.FungusOnVoiceover.Invoke();
    }
}
