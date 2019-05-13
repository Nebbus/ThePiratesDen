using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SOBJ_AudioOneShot_FMOD))]
public class EDI_AudioOneShot_FMOD : EDI_Reaction
{
    protected override string GetFoldoutLabel()
    {
        return "SOBJ_AudioOneShot_FMOD";
    }

}
