using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class MONO_PlayerFootStepPllayer : MonoBehaviour {


    [FMODUnity.EventRef]
    public string pathToFootStep;


    /// <summary>
    /// This funktion will be calld by a animations event,
    /// so it ned to be events when the players fot hits the ground.
    /// </summary>
    private void Step()
    {
        FMODUnity.RuntimeManager.PlayOneShot(pathToFootStep);
    }
}
