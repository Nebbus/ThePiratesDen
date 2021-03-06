﻿using UnityEngine;

/*This script is used to reset scriptable objects
 * back to their default values.  This is useful
 * in the editor when serialized data can persist
 * between entering and exiting play mode.  It is
 * also useful for situations where the game needs
 * to reset without being closed, for example a new
 * play through.
 */
public class MONO_DataResetter : MonoBehaviour
{
    public SOBJ_ResettableScriptableObject[] SOBJ_resettableScriptableObjects;    // All of the scriptable object assets that should be reset at the start of the game.


    public void ResetAll()
    {
        // Go through all the scriptable objects and call their Reset function.
        for (int i = 0; i < SOBJ_resettableScriptableObjects.Length; i++)
        {
            SOBJ_resettableScriptableObjects[i].Reset();
        }
    }
}
