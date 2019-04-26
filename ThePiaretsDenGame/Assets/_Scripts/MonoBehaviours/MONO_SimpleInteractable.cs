using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// simple class for on click 
/// </summary>
public class MONO_SimpleInteractable : MONO_interactionBase {


    public UnityEvent myClickEvent;

    public UnityEvent myHoverkEvent;

    private void Awake()
    {
        if (myClickEvent == null)
        {
            myClickEvent = new UnityEvent();
        }
        if (myHoverkEvent == null)
        {
            myHoverkEvent = new UnityEvent();
        }
    }



    public override void OnClick()
    {
        myClickEvent.Invoke();
    }

    public override void OnHovor()
    {
        myHoverkEvent.Invoke();
    }
}
