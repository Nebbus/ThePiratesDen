using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MONO_CustomMouseCursor))]
[RequireComponent(typeof(MONO_CursorLogic))]
[RequireComponent(typeof(MONO_HeldItem))]
public class MONO_AdventureCursor : MonoBehaviour {


    public  static MONO_AdventureCursor   instance;


    private MONO_HeldItem               monoHoldedItem;
    private MONO_CursorLogic             monoPointerLogic;
    private MONO_CustomMouseCursor        monoCustimMouseCursor;

    public MONO_CursorLogic getMonoPointerLogic
    {
        get
        {
            return monoPointerLogic;
        }
    }
    public MONO_HeldItem     getMonoHoldedItem
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
            monoHoldedItem         = GetComponent<MONO_HeldItem>();
            monoPointerLogic       = GetComponent<MONO_CursorLogic>();
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
