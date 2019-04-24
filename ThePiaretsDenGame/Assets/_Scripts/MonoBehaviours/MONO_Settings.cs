using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MONO_Settings : MonoBehaviour {

	public GameObject KeyboardToggle;
	public GameObject MouseToggle;


	public void MusicVolumeChanged()
	{
		//FMOD stuff?
	}

	public void SoundVolumeChanged()
	{
		//FMOD stuff?
	}



	/// <summary>
	/// Switch between the controls being used.
	/// </summary>
	public void SwitchControls()
	{
		if (KeyboardToggle.GetComponent<Toggle> ().isOn)
		{
			ActivateKeyboard ();
			DeactivateMouse ();
		} 
		else if (MouseToggle.GetComponent<Toggle> ().isOn)
		{
			ActivateMouse ();
			DeactivateKeyboard ();
		}
	}

	private void ActivateKeyboard()
	{
		
	}

	private void DeactivateKeyboard(){
	}

	private void ActivateMouse()
	{

	}

	private void DeactivateMouse(){
		
	}

}
