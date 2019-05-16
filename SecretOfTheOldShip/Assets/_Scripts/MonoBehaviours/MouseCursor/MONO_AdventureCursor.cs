using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MONO_CustomMouseCursor))]
[RequireComponent(typeof(MONO_CursorLogic))]
[RequireComponent(typeof(MONO_HeldItem))]
[RequireComponent(typeof(MONO_CursorSprite))]
public class MONO_AdventureCursor : MonoBehaviour {


    public  static MONO_AdventureCursor   instance;


    private MONO_HeldItem           monoHoldedItem;
    private MONO_CursorLogic        monoPointerLogic;
    private MONO_CustomMouseCursor  monoCustimMouseCursor;
    private MONO_CursorSprite       monoCursorSprite;

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

    public MONO_CursorSprite getMonoCursorSprite
    {
        get
        {
            return monoCursorSprite;
        }
    }

    private void Start()
    {
        if (!instance)
        {
            monoHoldedItem         = GetComponent<MONO_HeldItem>();
            monoPointerLogic       = GetComponent<MONO_CursorLogic>();
            monoCustimMouseCursor  = GetComponent<MONO_CustomMouseCursor>();
            monoCursorSprite       = GetComponent<MONO_CursorSprite>();
            instance = this;
        }
        else
        {
            Debug.Log("Try to create two custom adventur muse pointer");
            Destroy(this);
        }
    }

}
