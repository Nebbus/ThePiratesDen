using UnityEngine;
using System.Collections;

public class AudioPickup : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string pickUp;

	public void AudioPickupTaken()
	{
        FMODUnity.RuntimeManager.PlayOneShot(pickUp, transform.position);
        Debug.Log("PickupTaken");
	}
}
