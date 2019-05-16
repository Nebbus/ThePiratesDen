using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MONO_Wait : MonoBehaviour {
	

	public void Wait(float secondsToWait)
	{
		StartCoroutine (WaitSomeTime(secondsToWait));
	}

	/// <summary>
	/// Starts a new  wait for seconds.
	/// </summary>
	/// <returns>The some time.</returns>
	/// <param name="seconds">Seconds.</param>
	IEnumerator WaitSomeTime(float seconds)
	{
		Debug.Log ("Waiting");
		yield return new WaitForSeconds (seconds);
	}
}
