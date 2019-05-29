using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MONO_EventManager : MonoBehaviour
{

    public static bool isNotWorking = true;

    //Re-usable structure/ Can be a class to. Add all parameters you need inside it
     public struct EventParam
    {
        public string   param1;
        public int      param2;
        public float    param3;
        public bool     param4;
        public Vector3  param5;
        public MONO_InteractionBase param6;

    }

    private Dictionary<string, Action<EventParam>> eventDictionary;

    private static MONO_EventManager monoEventManager;

    // All the event that exist
    public const string onInteractableEvnetManager_NAME         = "moveInteractPlayer";
    public const string onGroundEvnetManager_NAME               = "moveGround";
    public const string onHiglightAllInteractablesInScene_NAME  = "onHigligtAll";
    public const string offHiglightAllInteractablesInScene_NAME = "offHigligtAll";
    public const string setInputHandling_NAME                   = "setInputHandling";
    public const string sceneStartSetup_NAME                    = "sceneStartSetup";
    public const string sceneShutdownSetup_NAME                 = "sceneShutdownSetup";
    public const string setVisibilityOfInvnetory_NAME           = "setVisibilityOfInvnetory";
    public const string setLocalInvntoryHandelInput_NAME        = "setLocalInvntoryHandelInput";


    /// <summary>
    /// getter for the MONO_EventManager
    /// </summary>
    public static MONO_EventManager instance
    {
        get
        {
            if (!monoEventManager)
            {
                monoEventManager = FindObjectOfType(typeof(MONO_EventManager)) as MONO_EventManager;


                if (!monoEventManager)
                {
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    monoEventManager.Init();
                }
            }
            return monoEventManager;
        }
    }

    /// <summary>
    /// Inits the event dictonary
    /// </summary>
    private void Init()
    {
        if(eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, Action<EventParam>>();
        }
    }

    /// <summary>
    /// adds a listener and if nesesary cratsif 
    /// </summary>
    /// <param name="eventName"> the name of the funktion</param>
    /// <param name="listener"> the event that is suposed to </param>
    public static void StartListening(string eventName, Action<EventParam> listener)
    {
        Action<EventParam> thisEvent = null;

        if(instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            //add listener
            thisEvent += (listener);

            //Update the dictionary
            instance.eventDictionary[eventName] = thisEvent;
        }
        else
        {
       
            //Add event to the Dictionary for the first time
            // thisEvent =  UnityEvent<EventParam>();
            thisEvent += (listener);
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    /// <summary>
    /// removes a listner and if necesary creats it
    /// </summary>
    /// <param name="eventName"> the name of the event</param>
    /// <param name="listener"> the listener</param>
    public static void StopListening(string eventName, Action<EventParam> listener)
    {
        if(monoEventManager == null)
        {
            return;
        }

        Action<EventParam> thisEvent;

        if(instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            //remove event from the existing one
            thisEvent -= (listener);

            //Update the dictionary
            instance.eventDictionary[eventName] = thisEvent;
        }
    }


    /// <summary>
    /// trigger a event
    /// </summary>
    /// <param name="eventName"> name of the event that should be triggerd</param>
    /// <param name="eventParam">paramettrs</param>
    public static void TriggerEvent(string eventName, EventParam eventParam)
    {
        isNotWorking = false;
        Action<EventParam> thisEvent = null;

        if(instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            
            if((thisEvent != null))
            {
                thisEvent.Invoke(eventParam);
            }
            else
            {
                Debug.Log((thisEvent == null) ? "null: " + eventName + " dosent exist" : thisEvent.ToString());
            }

        }
        isNotWorking = true;
    }



}
