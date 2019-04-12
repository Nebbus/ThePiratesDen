using UnityEngine;
using System.Collections;

public class AudioEventATrigger : MonoBehaviour
{

	public FMODUnity.StudioEventEmitter bedroomAmbience;

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "Player") 
		{
			bedroomAmbience.Play ();
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			bedroomAmbience.Stop ();
		}
	}

	[FMODUnity.EventRef]
	public string EventA;

	public void AudioEventATriggered()
	{
		FMODUnity.RuntimeManager.PlayOneShot (EventA, transform.position);
		Debug.Log("EventATriggered");
	}

}
