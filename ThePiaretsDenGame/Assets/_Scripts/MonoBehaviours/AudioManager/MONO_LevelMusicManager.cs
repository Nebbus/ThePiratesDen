using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class MONO_LevelMusicManager : MonoBehaviour {

    public static MONO_LevelMusicManager instace;
    //LjudControll
    //settParametes
    //level music och ambines

    public StudioEventEmitter AmbienceTarget;
    public StudioEventEmitter MusicTarget;


    public static StudioEventEmitter getAmbienceTarget
    {
        get
        {
            if(instace == null)
            {
                instace = FindObjectOfType<MONO_LevelMusicManager>();
            }



            return instace.AmbienceTarget;
        }
    }

    public static StudioEventEmitter getMusicTarget
    {
        get
        {
            if (instace == null)
            {
                instace = FindObjectOfType<MONO_LevelMusicManager>();
            }

            return instace.MusicTarget;
        }
    }

    private void Awake()
    {
        if (instace == null)
        {
            instace = this;
        }
        else
        {
            Debug.LogError("Must only be one leve music manager");
        }
    }

    /// <summary>
    /// set the paramters of the fmodEvent
    /// </summary>
    /// <param name="parametersName"> name of the parameter</param>
    /// <param name="parametersvalues"> new valus of parameters</param>
    public void setMusicParamters(string[] parametersName, float[] parametersValues)
    {
        for (int j = 0; j < MusicTarget.Params.Length; j++)
        {

            MusicTarget.SetParameter(parametersName[j], parametersValues[j]);
        }
    }
    /// <summary>
    /// set the paramters of the fmodEvent
    /// </summary>
    /// <param name="parametersName"> name of the parameter</param>
    /// <param name="parametersvalues"> new valus of parameters</param>
    public void setAmbientParamters(string[] parametersName, float[] parametersValues)
    {
        for (int j = 0; j < MusicTarget.Params.Length; j++)
        {

            AmbienceTarget.SetParameter(parametersName[j], parametersValues[j]);
        }
    }





    /// <summary>
    /// Shange the overal volume
    /// </summary>
    /// <param name="changeFactor"> how muthce the sounds changes</param>
    public void changeVolume(float changeFactor)
    {

    }





}
