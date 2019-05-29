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
	private FMOD.Studio.VCA masterVolumeController;
    private FMOD.Studio.VCA musicVolumeController;
	private FMOD.Studio.VCA sfxVolumeController;
    public string debug;
 

    private void Awake()
    {
        if (instace == null)
        {
            instace = this;
            masterVolumeController = FMODUnity.RuntimeManager.GetVCA(pathToMasterVCA);
			musicVolumeController = FMODUnity.RuntimeManager.GetVCA(pathToMusicVCA);
			sfxVolumeController = FMODUnity.RuntimeManager.GetVCA(pathToSFXVCA);
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
    public void StartStopSound(bool start, bool startAmbience, bool startMusic)
    {
        if (start)
        {
            if (startMusic)
            {
                if (!MusicTarget.IsPlaying())
                {
                    MusicTarget.Play();
                }

            }
            if (startAmbience)
            {
                if (!AmbienceTarget.IsPlaying())
                {
                    AmbienceTarget.Play();
                }

            }

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
    public void changeMasterVolume(float changeFactor)
    {

        float oldVolume;
        float dummy;
        masterVolumeController.getVolume(out dummy, out oldVolume);
        masterVolumeController.setVolume(oldVolume + changeFactor);
    }


	public string changeMusicVolume(float changeFactor)
	{
		float currentVolume, percentageVolume;
		musicVolumeController.getVolume (out percentageVolume, out currentVolume);

		currentVolume = currentVolume + changeFactor;
		currentVolume *= 100f;
		currentVolume = Mathf.Round (currentVolume);
		percentageVolume = currentVolume;
		currentVolume *= 0.01f;

		musicVolumeController.setVolume (currentVolume);

		return percentageVolume.ToString();
	}

	public void changeSFXVolume(float changeFactor, UnityEngine.UI.Text text)
	{

	}

	public string GetMusicVolume()
	{
		float currentVolume, percentageVolume;
		musicVolumeController.getVolume (out percentageVolume, out currentVolume);

		percentageVolume = Mathf.Round (currentVolume * 100f);

		return percentageVolume.ToString();
	}

}
