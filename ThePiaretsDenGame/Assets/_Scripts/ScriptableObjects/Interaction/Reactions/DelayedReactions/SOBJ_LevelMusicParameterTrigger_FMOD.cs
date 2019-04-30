using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FMODUnity;

public class SOBJ_LevelMusicParameterTrigger_FMOD : SOBJ_FMODreaction {

    [Serializable]
    public class EmitterRef
    {
        public StudioEventEmitter Target;
        public ParamRef[] Params;
    }

    
    public EmitterRef[] musicEmittor;

    public EmitterRef[] ambienceEmittor;

    public float[] valuesAmb;
    public float[] valuesMus;

    public string[] namesAmb;
    public string[] namesMus;

    protected override void SpecificInit()
    {
        int ambLenght = ambienceEmittor[0].Params.Length;
        int musLenght = musicEmittor[0].Params.Length;
        valuesAmb = new float[ambLenght];
        valuesMus = new float[musLenght];

        namesAmb = new string[ambLenght];
        namesMus = new string[musLenght];

        for (int i = 0; i < ambLenght; i++)
        {
            namesAmb[i]  = ambienceEmittor[0].Params[i].Name;
            valuesAmb[i] = ambienceEmittor[0].Params[i].Value;
        }
        for (int i = 0; i < musLenght; i++)
        {
            namesMus[i]  = musicEmittor[0].Params[i].Name;
            valuesMus[i] = musicEmittor[0].Params[i].Value;
        }



    }

    protected override void ImmediateReaction()
    {
        MONO_LevelMusicManager.instace.setMusicParamters(namesMus, valuesMus);
        MONO_LevelMusicManager.instace.setAmbientParamters(namesAmb, valuesAmb);
    }

  
}
