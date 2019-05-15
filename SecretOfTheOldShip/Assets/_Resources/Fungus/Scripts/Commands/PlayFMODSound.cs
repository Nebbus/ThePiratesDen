using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using FMOD;

[CommandInfo("FMOD", "Play FMOD Sound", "Plays an FMOD sound")]
public class PlayFMODSound : Command 
{
	[FMODUnity.EventRef]
	public string pathToFMODSound;

	public override void OnEnter()
	{
		FMODUnity.RuntimeManager.PlayOneShot(pathToFMODSound);
		Continue();
	}

	public override string GetSummary()
	{
		return "FMODSound";
	}

	public override Color GetButtonColor ()
	{
		return Color.yellow;
	}
}
