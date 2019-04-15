using UnityEngine;
using System;
using FMODUnity;

public class SOBJ_AudioReaction_FMOD : SOBJ_Reaction 
{
    [Serializable]
    public class EmitterRef
    {
        public StudioEventEmitter Target;
        public ParamRef[] Params;
    }

    public EmitterRef[] Emitters;

    public bool play;

    protected override void ImmediateReaction()
    {

        for (int i = 0; i < Emitters.Length; i++)
        {
            var emitterRef = Emitters[i];
            if (emitterRef.Target != null)
            {
                if (play)
                {
                    emitterRef.Target.Play();
                }
                for (int j = 0; j < Emitters[i].Params.Length; j++)
                {
                  
                    emitterRef.Target.SetParameter(Emitters[i].Params[j].Name, Emitters[i].Params[j].Value);
                }
            }
        }

    }


}





