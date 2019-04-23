using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class MONO_MenuSelect : MonoBehaviour {

	public EventSystem eventSystem;

	[Tooltip("First button on panel")]
	public GameObject selectedObject;	

	private bool buttonSelected;



	void Update () {
		//selects a button depending on the up and down arrow
		if (Input.GetAxisRaw ("Vertical") != 0 && !buttonSelected) 
		{
			eventSystem.SetSelectedGameObject (selectedObject);
			buttonSelected = true;
		}
		else if(eventSystem.IsPointerOverGameObject ())
		{
			eventSystem.SetSelectedGameObject (null);
			buttonSelected = false;
		}
	}


	private void OnDisable()
	{
		eventSystem.SetSelectedGameObject (null);
		buttonSelected = false;		//resets the menu so that no button is selected
	}


}
