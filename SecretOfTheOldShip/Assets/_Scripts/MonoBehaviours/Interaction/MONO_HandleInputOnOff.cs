using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MONO_HandleInputOnOff : MonoBehaviour {

	

    /// <summary>
    /// Triggers the setInputHandling event ( fuktion in the SceneManager)
    /// </summary>
    /// <param name="handleInput"></param>
    public void HandleInputOn(bool handleInput)
    {
        //===============================================================================================================
        MONO_EventManager.EventParam param = new MONO_EventManager.EventParam();
        param.param4 = handleInput;
        MONO_EventManager.TriggerEvent(MONO_EventManager.setInputHandling_NAME, param);
        //===============================================================================================================     
    }
}
