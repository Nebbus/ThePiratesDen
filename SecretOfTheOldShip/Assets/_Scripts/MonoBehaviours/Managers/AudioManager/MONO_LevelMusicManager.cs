using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class MONO_LevelMusicManager : MonoBehaviour {

    public static MONO_LevelMusicManager instace;


    public StudioEventEmitter AmbienceTarget;
    public StudioEventEmitter MusicTarget;


    public static string pathToMasterVCA =  "vca:/Master";
    public static string pathToMusicVCA = "vca:/Music";
	public static string pathToSFXVCA = "vca:/Sounds";
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
            Debug.LogError("Must only be one level music manager");
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
		float originalVolume, currentVolume, percentageVolume;
		musicVolumeController.getVolume (out percentageVolume, out originalVolume);

		currentVolume = originalVolume + changeFactor;
		currentVolume *= 100f;
		currentVolume = Mathf.Round (currentVolume);
		percentageVolume = currentVolume;
		currentVolume *= 0.01f;

		//if you are trying to change the volume outside the percentage scale
		if (percentageVolume < 0 || percentageVolume > 100) 
		{
			originalVolume = Mathf.Round (originalVolume * 100f);
			return originalVolume.ToString();
		}

		musicVolumeController.setVolume (currentVolume);
		return percentageVolume.ToString();
	}


	public string changeSFXVolume(float changeFactor)
	{
		float originalVolume, currentVolume, percentageVolume;
		sfxVolumeController.getVolume (out percentageVolume, out originalVolume);

		currentVolume = originalVolume + changeFactor;
		currentVolume *= 100f;
		currentVolume = Mathf.Round (currentVolume);
		percentageVolume = currentVolume;
		currentVolume *= 0.01f;

		//if you are trying to change the volume outside the percentage scale
		if (percentageVolume < 0 || percentageVolume > 100) 
		{
			originalVolume = Mathf.Round (originalVolume * 100f);
			return originalVolume.ToString();
		}

		sfxVolumeController.setVolume (currentVolume);
		return percentageVolume.ToString();
	}


	public string GetVolume(Audio whichVolume)
	{
		float currentVolume, percentageVolume;

		switch (whichVolume) {
		case Audio.Master:		//not implemented yet
			goto default;

		case Audio.Music:
			musicVolumeController.getVolume (out percentageVolume, out currentVolume);
			break;

		case Audio.SFX:
			sfxVolumeController.getVolume (out percentageVolume, out currentVolume);
			break;

		case Audio.Voices:		//not implemented yet
			goto default;

		default:
			Debug.LogError ("Couldn't get the volume type");
			return "";
		}


		percentageVolume = Mathf.Round (currentVolume * 100f);
		return percentageVolume.ToString();
	}

	public enum Audio {Master, Music, SFX, Voices};
	public Audio audioTypeMaster = Audio.Master;
	public Audio audioTypeMusic = Audio.Music;
	public Audio audioTypeSFX = Audio.SFX;
	public Audio audioTypeVoices = Audio.Voices;

}
