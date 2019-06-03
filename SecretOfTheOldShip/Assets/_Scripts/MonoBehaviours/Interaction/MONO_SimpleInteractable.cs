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

    private MONO_SceneManager monoSceneManager;
  


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

    [Tooltip(" if this shuld have get a Selectabe added to it so it can be selected then using keybord")]
    public bool canMoveMouseOver = false;

    [Tooltip("if this simple intaraction shuld obay the handle input OBS: must have the MONO_SceneManager in sen if true")]
    public bool respectHandleInput = false;



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

        if (canMoveMouseOver && gameObject.GetComponent(typeof(UnityEngine.UI.Selectable)) == null)
        {
            gameObject.AddComponent(typeof(UnityEngine.UI.Selectable));
        }
        monoSceneManager = FindObjectOfType(typeof(MONO_SceneManager)) as MONO_SceneManager;
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
        if (respectHandleInput)
        {

            if (monoSceneManager.getSetHandleInput)
            {
                myClickEvent.Invoke();
            }
           
        }
        else
        {
            myClickEvent.Invoke();
        }

    }

    public override void OnHoverEnterd()
    {
        if (respectHandleInput)
        {

            if (monoSceneManager.getSetHandleInput)
            {
                myHoverEnterdEvent.Invoke();
            }

        }
        else
        {
            myHoverEnterdEvent.Invoke();
        }

    }

    public override void OnHover()
    {
        if (respectHandleInput)
        {

            if (monoSceneManager.getSetHandleInput)
            {
                myHoverEvent.Invoke();
            }

        }
        else
        {
            myHoverEvent.Invoke();
        }

    }

    public override void OnHoverExit()
    {
        if (respectHandleInput)
        {

            if (monoSceneManager.getSetHandleInput)
            {
                myHoverExitEvent.Invoke();
            }

        }
        else
        {
            myHoverExitEvent.Invoke();
        }

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
