using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class MONO_PlayerFootStepPlayer : MonoBehaviour {


    [FMODUnity.EventRef]
    public string pathToFootStep;


    /// <summary>
    /// This funktion will be calld by a animations event,
    /// so it ned to be events when the players fot hits the ground.
    /// se this https://www.youtube.com/watch?v=Bnm8mzxnwP8 for how
    /// to create them.
    /// </summary>
    private void Step()
    {
        FMODUnity.RuntimeManager.PlayOneShot(pathToFootStep);
    }
}
