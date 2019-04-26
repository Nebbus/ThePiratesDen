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
		} 
		else if (MouseToggle.GetComponent<Toggle> ().isOn)
		{
			ActivateMouse ();
		}
	}

	private void ActivateKeyboard()
	{
	//Debug.Log ("activating keyboard");
        MONO_AdventureCursor.instance.getMonoCustimMouseCursor.UsingKeyboard = true;
        MONO_AdventureCursor.instance.getMonoPointerLogic.setKeyBordMode();
    }


	private void ActivateMouse()
	{
        MONO_AdventureCursor.instance.getMonoCustimMouseCursor.UsingKeyboard = false;
        MONO_AdventureCursor.instance.getMonoPointerLogic.setMouseMode();
    }

}
