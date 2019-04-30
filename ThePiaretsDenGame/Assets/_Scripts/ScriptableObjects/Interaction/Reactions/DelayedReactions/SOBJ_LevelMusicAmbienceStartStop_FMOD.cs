using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOBJ_LevelMusicAmbienceStartStop_FMOD : SOBJ_FMODreaction
{
    public enum mode { START, STOP};

    public mode onReaction = mode.START;


    protected override void ImmediateReaction()
    {
      
            MONO_LevelMusicManager.instace.StartStopSound(onReaction == mode.START);
        
        
    }
}
