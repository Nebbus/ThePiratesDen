using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MONO_HintButton : MonoBehaviour {


    //[SerializeField]
    //private KeyCode HiglightKey = KeyCode.Space;
    public float timeTheHiglightIsOn;

    private WaitForSeconds wait; // Storing the wait created from the delay so it doesn't need to be created each time.
    private bool timerStarted = false;
    private bool higlightedOn = false;
    private Coroutine curentTimer;

  


    private void Update()
    {


        if (MONO_Settings.instance.getHiglightAllButton)
        {
            HandleInventoryClick();
        }

        if (higlightedOn)
        {
         
            if (!timerStarted)
            {
                wait = new WaitForSeconds(timeTheHiglightIsOn);
                timerStarted = true;
                curentTimer = StartCoroutine(ReactCoroutine());
            }
        }

    }
    public void HandleInventoryClick()
    {
        if(timerStarted)
        {
            StopCoroutine(curentTimer);
            timerStarted = false;
            higligtAllOFF();
        }
        else
        {
            higligtAllON();
            higlightedOn = true;
            timerStarted = false;
        }
       
    }

    



    private void higligtAllON()
    {
       
        MONO_EventManager.EventParam param = new MONO_EventManager.EventParam();
        MONO_EventManager.TriggerEvent(MONO_EventManager.onHiglightAllInteractablesInScene_NAME, param);
     
    }

    private void higligtAllOFF()
    {
        MONO_EventManager.EventParam param = new MONO_EventManager.EventParam();
        MONO_EventManager.TriggerEvent(MONO_EventManager.offHiglightAllInteractablesInScene_NAME, param);
        higlightedOn = false;

    }

    private IEnumerator ReactCoroutine()
    {
        // Wait for the specified time.
        yield return wait;

        higligtAllOFF();
    }

}
