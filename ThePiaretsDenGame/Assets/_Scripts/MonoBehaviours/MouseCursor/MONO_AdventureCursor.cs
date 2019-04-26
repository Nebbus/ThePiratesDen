using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MONO_CustomMouseCursor))]
[RequireComponent(typeof(MONO_pointerLogic))]
[RequireComponent(typeof(MONO_HoldedItem))]
public class MONO_AdventureCursor : MonoBehaviour {


    public  static MONO_AdventureCursor   instance;

    private MONO_HoldedItem               monoHoldedItem;
    private MONO_pointerLogic             monoPointerLogic;
    private MONO_CustomMouseCursor        monoCustimMouseCursor;

    public MONO_pointerLogic getMonoPointerLogic
    {
        get
        {
            return monoPointerLogic;
        }
    }
    public MONO_HoldedItem     getMonoHoldedItem
    {
        get
        {
            return monoHoldedItem;
        }
    }
    public MONO_CustomMouseCursor getMonoCustimMouseCursor
    {
        get
        {
            return monoCustimMouseCursor;
        }
    }


    private void Start()
    {
        if (!instance)
        {
            monoHoldedItem         = GetComponent<MONO_HoldedItem>();
            monoPointerLogic       = GetComponent<MONO_pointerLogic>();
            monoCustimMouseCursor  = GetComponent<MONO_CustomMouseCursor>();
            instance = this;
        }
        else
        {
            Debug.Log("Try to create two custom adventur muse pointer");
            Destroy(this);
        }
    }

}
