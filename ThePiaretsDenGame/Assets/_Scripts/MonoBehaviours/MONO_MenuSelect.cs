using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class MONO_MenuSelect : MonoBehaviour {

	public EventSystem eventSystem;

	[Tooltip("First button on panel")]
	public GameObject firstSelectedMenuItem;	
    

	private bool buttonSelected;
    private GameObject currentSelectedMenuItem;
    private MONO_CustomMouseCursor OurCustomCursor;

    void Awake()
    {
        OurCustomCursor = FindObjectOfType<MONO_CustomMouseCursor>();

    }


	void Update () {
        if (currentSelectedMenuItem != eventSystem.currentSelectedGameObject)
        {
            currentSelectedMenuItem = eventSystem.currentSelectedGameObject;
        }
        if (OurCustomCursor.UsingKeyboard)
        {
            SelectWithKeys();
            Vector2 tempPos = new Vector2(currentSelectedMenuItem.GetComponent<RectTransform>().position.x - 500,
                                          currentSelectedMenuItem.GetComponent<RectTransform>().position.y);

            OurCustomCursor.CustomCursor.GetComponent<RectTransform>().position = tempPos;
        }
        else
        {
            eventSystem.SetSelectedGameObject(null);
        }
	}

    private void SelectWithKeys()
    {
        
        if (Input.GetAxisRaw("Vertical") != 0 && !buttonSelected)
        {
            eventSystem.SetSelectedGameObject(firstSelectedMenuItem);
            currentSelectedMenuItem = eventSystem.currentSelectedGameObject;

            buttonSelected = true;
        }
    }


	private void OnDisable()
	{
		eventSystem.SetSelectedGameObject (null);
		buttonSelected = false;		//resets the menu so that no button is selected
	}


}
