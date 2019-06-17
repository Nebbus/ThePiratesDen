using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MONO_HandleInput : MonoBehaviour {




    /// <summary>
    /// Triggers the setInputHandling event ( fuktion in the SceneManager)
    /// </summary>
    /// <param name="handleInput"></param>
    public void HandleInput(bool handle)
	{

     
        //===============================================================================================================
        MONO_EventManager.EventParam param = new MONO_EventManager.EventParam();
        param.param4 = handle;
        MONO_EventManager.TriggerEvent(MONO_EventManager.setInputHandling_NAME, param);
        MONO_EventManager.TriggerEvent(MONO_EventManager.setLocalInvntoryHandelInput_NAME, param);

        //Hids inventory and sets MonoCursor to defult
        if (!handle)
        {
            param.param4 = false;
            MONO_EventManager.TriggerEvent(MONO_EventManager.setVisibilityOfInvnetory_NAME, param);


            MONO_AdventureCursor.instance.getMonoCursorSprite.setDefultCursor();
        }

        //===============================================================================================================   
        


    }


}
