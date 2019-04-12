using UnityEngine;
using System.Collections;

public class AudioMovingNPC : MonoBehaviour
{
	[FMODUnity.EventRef]
	public string movingEnemySpawn;

    [FMODUnity.EventRef]
    public string movingEnemyAttack;


	private AudioManager audioManager;

    public FMODUnity.StudioEventEmitter dEnemyFootstepManager;
    public FMODUnity.StudioEventEmitter dEnemyDamageManager;

	void Awake()
	{
		audioManager = GameObject.FindObjectOfType<AudioManager> ();
		audioManager.audioMovingNPC = this;
	}

	public void MovingNPCSpawned()
	{
		FMODUnity.RuntimeManager.PlayOneShot (movingEnemySpawn, transform.position);

		dEnemyFootstepManager.SetParameter("DEnemyDeath", 0f);
        dEnemyFootstepManager.Play();
        Debug.Log ("MovingNPCSpawned");
	}

	public void MovingNPCHit(int hitpoints)
	{

        dEnemyDamageManager.SetParameter("DamageDeath", 0f);
        dEnemyDamageManager.Play();
        Debug.Log("MovingNPCHit - HitpointsRemaining: " + hitpoints);
	}

	public void MovingNPCKilledplayer()
	{
        FMODUnity.RuntimeManager.PlayOneShot(movingEnemyAttack, transform.position);

        dEnemyDamageManager.SetParameter("DamageDeath", 1f);
        dEnemyFootstepManager.SetParameter("DEnemyDeath", 1f);
        Debug.Log("MovingNPCKilledplayer");
	}

	public void MovingNPCDied()
	{
        dEnemyDamageManager.SetParameter("DamageDeath", 1f);
        dEnemyFootstepManager.SetParameter ("DEnemyDeath", 1f);
        Debug.Log("MovingNPCDied");
	}

}
