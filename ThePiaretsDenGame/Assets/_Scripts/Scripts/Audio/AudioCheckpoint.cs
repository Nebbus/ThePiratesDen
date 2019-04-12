using UnityEngine;
using System.Collections;

public class AudioCheckpoint : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string checkpoint;

    public void AudioCheckpointTaken()
	{
        FMODUnity.RuntimeManager.PlayOneShot(checkpoint, transform.position);
        Debug.Log("CheckpointTaken");
	}
}
