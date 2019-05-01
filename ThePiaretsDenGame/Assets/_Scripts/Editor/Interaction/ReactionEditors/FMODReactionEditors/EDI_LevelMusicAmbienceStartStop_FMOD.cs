using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SOBJ_LevelMusicAmbienceStartStop_FMOD))]
public class EDI_LevelMusicAmbienceStartStop_FMOD : EDI_Reaction
{

    protected override string GetFoldoutLabel()
    {
        return "SOBJ_LevelMusicAmbienceStartStop_FMOD";
    }
}
