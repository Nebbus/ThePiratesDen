using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveScript : MonoBehaviour 

{


	public FMODUnity.StudioEventEmitter cave;
	public FMODUnity.StudioEventEmitter droplets;

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "Player") 
		{
			cave.Play ();
			droplets.Play ();
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			cave.Stop ();
			droplets.Stop ();
		}
	}
}

