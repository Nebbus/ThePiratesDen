using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class MONO_LevelMusicManager : MonoBehaviour {

    public static MONO_LevelMusicManager instace;


    public StudioEventEmitter AmbienceTarget;
    public StudioEventEmitter MusicTarget;


    public static string pathToMasterVCA =  "vca:/Music";
    public static string pathToMusicVCA = "vca:/Music";
    public static string pathToSFXVCA = "vca:/Music";
    FMOD.Studio.VCA masterVolumController;
    FMOD.Studio.VCA musicVolumController;
    FMOD.Studio.VCA sfxVolumController;
    public string debug;
 

    private void Awake()
    {
        if (instace == null)
        {
            instace = this;
            masterVolumController = FMODUnity.RuntimeManager.GetVCA(pathToMasterVCA);
        }
        else
        {
            Debug.LogError("Must only be one leve music manager");
        }
    }

    /// <summary>
    /// set the paramters of the fmodEvent
    /// </summary>
    /// <param name="musicParams"> new valus of parameters</param>
    public void setMusicParamters(ParamRef[] musicParams)
    {
        for (int j = 0; j < musicParams.Length; j++)
        {
            
            MusicTarget.SetParameter(musicParams[j].Name, musicParams[j].Value);
        }
    }


    /// <summary>
    /// set the paramters of the fmodEvent
    /// </summary>
    /// <param name="ambientParams"> new valus of parameters</param>
    public void setAmbientParamters(ParamRef[] ambientParams)
    {
        for (int j = 0; j < ambientParams.Length; j++)
        {

            AmbienceTarget.SetParameter(ambientParams[j].Name, ambientParams[j].Value);
        }
    }


    /// <summary>
    /// to start and stop the emiters
    /// </summary>
    /// <param name="start">fals mans stoping the emiters and true starting them.</param>
    public void StartStopSound(bool start)
    {
        if (start)
        {
            MusicTarget.Play();
            AmbienceTarget.Play();
        }
        else
        {
            MusicTarget.Stop();
            AmbienceTarget.Stop();
        }
       
    }




    /// <summary>
    /// Shange the overal volume
    /// </summary>
    /// <param name="changeFactor"> how muthce the sounds changes</param>
    public void changeVolume(float changeFactor)
    {

        float oldVolume;
        float dummy;
        masterVolumController.getVolume(out dummy, out oldVolume);
        masterVolumController.setVolume(oldVolume + changeFactor);
    }





}
