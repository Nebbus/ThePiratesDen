using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SOBJ_AudioOneSHot_FMOD : SOBJ_Reaction
{
    [FMODUnity.EventRef]
    public string onShotEvent;

    [Tooltip("Transform/position of the sounds origin")]
    public Transform theSorceOftheOneShot = null;

    protected override void ImmediateReaction()
    {
        if(theSorceOftheOneShot == null)
        {
            FMODUnity.RuntimeManager.PlayOneShot(onShotEvent);
        }
        else
        {
            FMODUnity.RuntimeManager.PlayOneShot(onShotEvent, theSorceOftheOneShot.position);
        }

    }
}
