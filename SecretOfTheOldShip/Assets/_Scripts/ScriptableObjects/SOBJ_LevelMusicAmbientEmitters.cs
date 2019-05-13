using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SOBJ_LevelMusicAmbientEmitters : ScriptableObject {

    private static SOBJ_LevelMusicAmbientEmitters instance;

    private const string loadPath = "SOBJ_LevelMusicAmbientEmitters";

    
    public StudioEventEmitter AmbienceTarget;
    public StudioEventEmitter MusicTarget;

    public StudioEventEmitter getAmbienceTarget
    {
        get
        {
            return AmbienceTarget;
        }
    }
    public  StudioEventEmitter getMusicTarget
    {
        get
        {
            return MusicTarget;
        }
    }





    public static SOBJ_LevelMusicAmbientEmitters Instance                // The public accessor for the singleton instance.
    {
        get
        {
            // If the instance is currently null, try to find an SOBJ_LevelMusicAmbientEmiters instance already in memory.
            if (!instance)
            {
                instance = FindObjectOfType<SOBJ_LevelMusicAmbientEmitters>();
            }
            // If the instance is still null, try to load it from the Resources folder.
            if (!instance)
            {
                instance = Resources.Load<SOBJ_LevelMusicAmbientEmitters>(loadPath);
            }

            // If the instance is still null, report that it has not been created yet.
            if (!instance)
            {
                Debug.LogError("SOBJ_LevelMusicAmbientEmitters has not been created yet.  Go to Assets > Create > SOBJ_LevelMusicAmbientEmitters.");
            }

            return instance;
        }
        set { instance = value; }
    }


}
