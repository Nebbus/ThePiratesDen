using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MONO_FootStepsLoop_FMOD : MonoBehaviour {

    [FMODUnity.EventRef]
    public string footStepSound;

    public float delay = 0.5f;
    public float speed = 0.1f;

    private float curretn;

    private void OnAnimatorMove()
    {
        curretn = curretn - speed * Time.deltaTime;
        if (curretn <= 0)
        {
            FMODUnity.RuntimeManager.PlayOneShot(footStepSound);
        }

    }
}
