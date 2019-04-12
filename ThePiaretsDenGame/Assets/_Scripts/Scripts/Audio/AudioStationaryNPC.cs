using UnityEngine;
using System.Collections;

public class AudioStationaryNPC : MonoBehaviour
{
	public FMODUnity.StudioEventEmitter turretRotation; 
	public FMODUnity.StudioEventEmitter turretDeath;

	[FMODUnity.EventRef]
	public string turretShootEvent; 
	FMOD.Studio.EventInstance turretShoot;

	//private Sentry turretClass;

	void Start ()
	{
		//TTturretClass = GetComponent <Sentry> ();
		//turretRotation = FMODUnity.RuntimeManager.CreateInstance (turretRotation);
	}

	public void AudioStationaryNPCActivate()

	{
		Debug.Log ("StationaryNPCActivated");
	}

	public void AudioStationaryNPCDeactivate()
	{
		Debug.Log ("StationaryNPCDeactivated");
	}

	public void AudioStationaryNPCShoot()
	{
		turretShoot = FMODUnity.RuntimeManager.CreateInstance (turretShootEvent);
		//FMODUnity.RuntimeManager.AttachInstanceToGameObject (turretShoot, turretClass.muzzleTransform, GetComponent <Rigidbody> ());
		turretShoot.start ();
		Debug.Log ("StationaryNPCShoot");
	}

	public void AudioStationaryNPCRotationStart()
	{
		turretRotation.SetParameter ("RotationCheck", 0f);
		turretRotation.Play ();
		Debug.Log ("StationaryNPCRotationStarted");
	}

	public void AudioStationaryNPCRotationStop()
	{
		turretRotation.SetParameter ("RotationCheck", 1f);
		Debug.Log ("StationaryNPCRotationStopped");
	}

	public void AudioStationaryNPCDie()
	{
		turretRotation.Stop ();
		turretDeath.Play ();
		Debug.Log("StationaryNPCDied");
	}
}
