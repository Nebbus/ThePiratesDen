using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour 
{

	public int ammo = 5;
	public bool cave; 


	[FMODUnity.EventRef]

	public string caveEvent;
	FMOD.Studio.EventInstance caveSnapshot;

	void Awake ()
	{
		Debug.Log ("Jag existerar");
	}

	// Use this for initialization
	void Start () {

		caveSnapshot = FMODUnity.RuntimeManager.CreateInstance (caveEvent);
		Debug.Log ("Game start");
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.Q)) 
		
		{
			ammo--;
			Debug.Log ("Ammo left:" + ammo);
		}
			
		Debug.Log ("Helå");
	}

	void Cave (bool caveCheck)
	{
		if (cave) 
		{
			caveSnapshot.start ();
			Debug.Log ("I'm in the cave");
		} 

		else 
			
		{
			caveSnapshot.stop (FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			Debug.Log ("I'm not in the cave");
		}
	
	}

	void OnTriggerEnter (Collider other)

	{
		if (other.gameObject.tag == "Player");

		{
			Cave (cave = true);
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.gameObject.tag == "Player");

		{
			Cave (cave = false);
		}
	}
}

