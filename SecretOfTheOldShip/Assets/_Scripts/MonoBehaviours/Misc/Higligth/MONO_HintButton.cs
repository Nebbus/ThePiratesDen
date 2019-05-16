using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MONO_HintButton : MonoBehaviour {


    //[SerializeField]
    //private KeyCode HiglightKey = KeyCode.Space;



    //public void Update()
    //{
    //    if(Input.ke)
    //}



    public void higligtAllON()
    {
        MONO_EventManager.EventParam param = new MONO_EventManager.EventParam();
        MONO_EventManager.TriggerEvent(MONO_EventManager.onHiglightAllInteractablesInScene_NAME, param);
    }

    public void higligtAllOFF()
    {
        MONO_EventManager.EventParam param = new MONO_EventManager.EventParam();
        MONO_EventManager.TriggerEvent(MONO_EventManager.offHiglightAllInteractablesInScene_NAME, param);

    }

}
