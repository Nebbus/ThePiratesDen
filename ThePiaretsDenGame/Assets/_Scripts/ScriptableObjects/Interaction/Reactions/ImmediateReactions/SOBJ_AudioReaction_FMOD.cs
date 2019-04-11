using UnityEngine;
using System;
using FMODUnity;
// if oneShot, fadeout, 3D sound(is3D) or override attenuation is needed as parameters se StudioEventEmitterEditor.
public class SOBJ_AudioReaction_FMOD : SOBJ_Reaction
{
    [FMODUnity.EventRef]
    FMOD.Studio.EventInstance musicEvent;

    [EventRef]
    public String Event = "";

    public ParamRef[] Params = new ParamRef[0];


    protected FMOD.Studio.EventDescription eventDescription;
    public    FMOD.Studio.EventDescription EventDescription
    {
        get
        {
            return eventDescription;
        }
    }

    public bool Preload = false;

    protected FMOD.Studio.EventInstance instance;
    public    FMOD.Studio.EventInstance EventInstance
    {
        get
        {
            return instance;
        }
    }

    private bool hasTriggered = false;
    private bool isQuitting   = false;


    protected override void SpecificInit()
    {
        RuntimeUtils.EnforceLibraryOrder();
        if (Preload)
        {
            Lookup();
            eventDescription.loadSampleData();
            RuntimeManager.StudioSystem.update();
            FMOD.Studio.LOADING_STATE loadingState;
            eventDescription.getSampleLoadingState(out loadingState);
            while (loadingState == FMOD.Studio.LOADING_STATE.LOADING)
            {
#if WINDOWS_UWP
                    System.Threading.Tasks.Task.Delay(1).Wait();
#else
                System.Threading.Thread.Sleep(1);
#endif
                eventDescription.getSampleLoadingState(out loadingState);
            }
        }
    }

    protected override void ImmediateReaction()
    {
        Play();
    }
    
    protected override void SpecificOnAppQuit()
    {
        isQuitting = true;
    }

    void OnDestroy()
    {
        Debug.Log("LjudFörstörs kanke");
        if (!isQuitting)
        {
            Stop();
            if (instance.isValid())
            {
                RuntimeManager.DetachInstanceFromGameObject(instance);
            }

            if (Preload)
            {
                eventDescription.unloadSampleData();
            }
        }
    }
    void Lookup()
    {
        eventDescription = RuntimeManager.GetEventDescription(Event);
    }

    public void Play()
    {
        if (hasTriggered)
        {
            return;
        }

        if (String.IsNullOrEmpty(Event))
        {
            return;
        }

        if (!eventDescription.isValid())
        {
            Lookup();
        }

        bool isOneshot = true;
        if (!Event.StartsWith("snapshot", StringComparison.CurrentCultureIgnoreCase))
        {
            eventDescription.isOneshot(out isOneshot);
        }

        if (!instance.isValid())
        {
            instance.clearHandle();
        }

        // Let previous oneshot instances play out
        if (isOneshot && instance.isValid())
        {
            instance.release();
            instance.clearHandle();
        }

        if (!instance.isValid())
        {
            eventDescription.createInstance(out instance);
        }

        foreach (var param in Params)
        {
            instance.setParameterValue(param.Name, param.Value);
        }

        instance.start();

        hasTriggered = true;

    }

    public void Stop()
    {
        if (instance.isValid())
        {
            instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            instance.release();
            instance.clearHandle();
        }
    }

    public void SetParameter(string name, float value)
    {
        if (instance.isValid())
        {
            instance.setParameterValue(name, value);
        }
    }

    public bool IsPlaying()
    {
        if (instance.isValid() && instance.isValid())
        {
            FMOD.Studio.PLAYBACK_STATE playbackState;
            instance.getPlaybackState(out playbackState);
            return (playbackState != FMOD.Studio.PLAYBACK_STATE.STOPPED);
        }
        return false;
    }
}






