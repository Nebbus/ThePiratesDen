using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SOBJ_LevelMusicAmbientEmiters : ScriptableObject {

    private static SOBJ_LevelMusicAmbientEmiters instance;

    private const string loadPath = "SOBJ_LevelMusicAmbientEmiters";

    
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





    public static SOBJ_LevelMusicAmbientEmiters Instance                // The public accessor for the singleton instance.
    {
        get
        {
            // If the instance is currently null, try to find an SOBJ_LevelMusicAmbientEmiters instance already in memory.
            if (!instance)
            {
                instance = FindObjectOfType<SOBJ_LevelMusicAmbientEmiters>();
            }
            // If the instance is still null, try to load it from the Resources folder.
            if (!instance)
            {
                instance = Resources.Load<SOBJ_LevelMusicAmbientEmiters>(loadPath);
            }

            // If the instance is still null, report that it has not been created yet.
            if (!instance)
            {
                Debug.LogError("SOBJ_LevelMusicAmbientEmiters has not been created yet.  Go to Assets > Create > SOBJ_LevelMusicAmbientEmiters.");
            }

            return instance;
        }
        set { instance = value; }
    }


}
