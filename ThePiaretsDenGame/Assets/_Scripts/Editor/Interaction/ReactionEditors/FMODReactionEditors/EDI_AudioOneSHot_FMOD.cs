using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SOBJ_AudioOneSHot_FMOD))]
public class EDI_AudioOneSHot_FMOD : EDI_Reaction
{
    protected override string GetFoldoutLabel()
    {
        return "SOBJ_AudioOneSHot_FMOD";
    }

}
