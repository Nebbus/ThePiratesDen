using UnityEngine;
using System;
using FMODUnity;

public class SOBJ_AudioParameterReaction_FMOD : SOBJ_Reaction
{
    [FMODUnity.EventRef]
    FMOD.Studio.EventInstance musicEvent;

    [EventRef]
    public String Event = "";

    public ParamRef[] Params = new ParamRef[0];
    


    protected FMOD.Studio.EventDescription eventDescription;
    public FMOD.Studio.EventDescription EventDescription
    {
        get
        {
            return eventDescription;
        }
    }

    protected FMOD.Studio.EventInstance instance;
    public FMOD.Studio.EventInstance EventInstance
    {
        get
        {
            return instance;
        }
    }




    protected override void SpecificInit()
    {
        RuntimeUtils.EnforceLibraryOrder();
    }

    protected override void ImmediateReaction()
    {
        if (instance.isValid())
        {
            foreach (var param in Params)
            {
                instance.setParameterValue(param.Name, param.Value);
            }
        }
    }


}





