using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MONO_Settings : MonoBehaviour {

    public bool usingKeybodInput = false;

    public static MONO_Settings instance;


	public GameObject KeyboardToggle;
	public GameObject MouseToggle;


    //===========================================================================
    // controlls
    //===========================================================================

    [SerializeField]
    private KeyCode mouseKey = KeyCode.Mouse0;

    [SerializeField]
    private KeyCode keabordKey = KeyCode.Space;
    [SerializeField]
    private KeyCode toNextInteractable = KeyCode.Tab;
    [SerializeField]
    private KeyCode higligthAllButton = KeyCode.Q;
    [SerializeField]
    private KeyCode inventoryButton = KeyCode.W;

    [SerializeField]
    private KeyCode pauseButtonKey = KeyCode.Escape;

    public bool getClickKey
    {
        get
        {
            return Input.GetKeyDown((usingKeybodInput) ? keabordKey : mouseKey);
        }
    }

    public bool getPauseButtonKey
    {
        get
        {
            return Input.GetKeyDown(pauseButtonKey);
        }
    }


    public bool getHiglightAllButton
    {
        get
        {
            return Input.GetKeyDown(higligthAllButton);
        }
    }
    public bool getToNextInteractable
    {
        get
        {
            return Input.GetKeyDown(toNextInteractable);
        }
    }
    public bool getInventoryButton
    {
        get
        {
            return Input.GetKeyDown(inventoryButton);
        }
    }



    public float getKeybordHorizontal
    {
        get
        {
            return Input.GetAxis("Horizontal");
        }
    }
    public float getKeybordVertical
    {
        get
        {
            return Input.GetAxis("Vertical");
        }
    }

    public float getMouseHorizontal
    {
        get
        {
            return Input.GetAxis("Mouse X");
        }
    }
    public float getMouyseVertical
    {
        get
        {
            return Input.GetAxis("Mouse Y");
        }
    }



    public void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError(this.ToString() + " tryed to creat two setings");
        }
    }


    public void UsingMouse()
    {
        usingKeybodInput = false;

    }
    public void UsingKeyBord()
    {
        usingKeybodInput = true;

    }

    /// <summary>
    /// Switch between the controls being used.
    /// </summary>
    public void SwitchControls()
	{
		if (KeyboardToggle.GetComponent<Toggle> ().isOn)
		{
            usingKeybodInput = true;

        } 
		else if (MouseToggle.GetComponent<Toggle> ().isOn)
		{
            usingKeybodInput = false;
        }
	}




	

}
