using UnityEngine;
using System.Collections;

public class AudioEventBTrigger : MonoBehaviour
{

	public FMODUnity.StudioEventEmitter spotlight;

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "Player") 
		{
			spotlight.Play ();
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			spotlight.Stop ();
		}
	}


	public void AudioEventBTriggered()
	{
		Debug.Log("EventBTriggered");
	}

}
