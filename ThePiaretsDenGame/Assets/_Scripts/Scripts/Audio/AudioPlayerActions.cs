using UnityEngine;
using System.Collections;

public class AudioPlayerActions : MonoBehaviour
{
    public float myFloat = 0f;

    public GameObject deathCamera;
   // public FirstPersonController playerController;
    //public Player player;

    [FMODUnity.EventRef]
	public string playerSpawn;

	[FMODUnity.EventRef]
	public string playerDiedRespawn;

   /* [FMODUnity.EventRef]
    public string playerRunEvent;
	FMOD.Studio.EventInstance playerRun;
	*/

    [FMODUnity.EventRef]
	public string playerShoot;

	[FMODUnity.EventRef]
	public string playerJump;

	[FMODUnity.EventRef]
	public string playerFootstepEvent;
	FMOD.Studio.EventInstance playerFootstep;

	public bool showFootstepDebugMessages = false;

	/*void Start()
	{
		playerRun = FMODUnity.RuntimeManager.CreateInstance (playerRunEvent);
	}
*/
	public void PlayFootstep(int material)

	{
		playerFootstep = FMODUnity.RuntimeManager.CreateInstance (playerFootstepEvent);

		//Switchsats. 
		switch (material) 
		{

        case 0:
            playerFootstep.setParameterValue ("Surface", 0f);
            break;
		case 1:
			playerFootstep.setParameterValue ("Surface", 1f);
			break;

		case 2: playerFootstep.setParameterValue ("Surface", 2f);
			break;

		case 3: playerFootstep.setParameterValue ("Surface", 3f);
			break;

		case 4: playerFootstep.setParameterValue ("Surface", 4f);
			break;
		}

		playerFootstep.start ();

		if (!showFootstepDebugMessages)
		{
			return;
		}
		else
		{
			Debug.Log ("PlayerFootstep - Material: " + material);
		}
	}
    
    public void PlayLand (int material)
	{
		
		Debug.Log ("PlayerLanded - Material: " + material);
	}

	public void PlayJump (int material)
	{
		FMODUnity.RuntimeManager.PlayOneShot (playerJump, transform.position);
		Debug.Log ("PlayerJumped - Material: " + material);
	}

	public void PlaySpawn()
	{
		FMODUnity.RuntimeManager.PlayOneShot (playerSpawn, transform.position);
		Debug.Log ("PlayerSpawned");
	}

    /*void Update()
   {

		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
        	playerRun.start();
        }
        else
        {
			playerRun.triggerCue();
        }
	}*/
    
    public void PlayShoot()
	{
		FMODUnity.RuntimeManager.PlayOneShot (playerShoot, transform.position);
		Debug.Log ("PlayerShoot");
	}

	public void PlayerDied ()
	{
        //FMODUnity.RuntimeManager.PlayOneShot(playerDiedRespawn, transform.position);
        //StartCoroutine(WaitForSpawn());
		Debug.Log ("PlayerDied");
	}

    public IEnumerator WaitForSpawn ()
       {
           FMODUnity.RuntimeManager.PlayOneShot(playerDiedRespawn, transform.position);
           deathCamera.SetActive(true);
        /*    playerController.enabled = false;
         player.enabled = false;*/
        yield return new WaitForSeconds(myFloat);
        deathCamera.SetActive(false);
        /*   playerController.enabled = true;
        player.enabled = true;*/
    }

 


    public void PlayerWon(float waitTime)
	{
		Debug.Log ("PlayerWon - Waiting " + waitTime + " seconds");
	}
}

