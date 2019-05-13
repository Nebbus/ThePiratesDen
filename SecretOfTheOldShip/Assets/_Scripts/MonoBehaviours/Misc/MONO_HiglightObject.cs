using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MONO_HiglightObject : MonoBehaviour {

    public GameObject selectedObject;
    public int        redCol;
    public int        greenCol;
    public int        blueCol;
    public bool       lookingAtObject = false;
    public bool       flashingIn      = true;
    public bool       startedFlash    = false;

    public Coroutine curentFlash;
    public int strengt = 0;
    public float time = 10f;
    private float lastTime;

    public bool showOnMouseOver = false;

    public Color originalFer;

    private void Update()
    {
       
        if (lookingAtObject)
        {
            if ((Time.realtimeSinceStartup - lastTime) >= time)
            {
                StopFlashing();
            }

            if(selectedObject.GetComponent<Renderer>() == null)
            {
                selectedObject.GetComponent<Image>().color = new Color32((byte)redCol, (byte)greenCol, (byte)blueCol, 255);
            }
            else
            {
                selectedObject.GetComponent<Renderer>().material.color = new Color32((byte)redCol, (byte)greenCol, (byte)blueCol, 255);
            }
     
        }
    }


   private void OnMouseOver()
    {
        if (showOnMouseOver)
        {
            lookingAtObject = true;
            if (startedFlash == false)
            {
                startedFlash = true;

                curentFlash = StartCoroutine(FlashObject());
            }
        }

    }
    private void OnMouseExit()
    {
        if (showOnMouseOver)
        {
            startedFlash = false;
            lookingAtObject = false;
            StopCoroutine(curentFlash);
            selectedObject.GetComponent<Renderer>().material.color = new Color32(255, 255, 255, 255);

        }

    }


    public void startFlashing()
    {
        if (selectedObject.GetComponent<Renderer>() == null)
        {
            originalFer = selectedObject.GetComponent<Image>().color ;
        }
        else
        {
            originalFer = selectedObject.GetComponent<Renderer>().material.color;
        }
        
        startedFlash    = true;
        lookingAtObject = true;
        lastTime        = Time.realtimeSinceStartup;
        curentFlash     = StartCoroutine(FlashObject());
    }
    public void StopFlashing()
    {
        startedFlash = false;
        lookingAtObject = false;
        StopCoroutine(curentFlash);

        if (selectedObject.GetComponent<Renderer>() == null)
        {
            selectedObject.GetComponent<Image>().color = originalFer;

        }
        else
        {
            selectedObject.GetComponent<Renderer>().material.color = originalFer;
        }
        strengt = 255;
        redCol = strengt;
        blueCol = strengt;
        greenCol = strengt;

    }

    
    IEnumerator FlashObject()
    {
        while (lookingAtObject)
        {
           
            yield return new WaitForSeconds(0.025f);
            if (flashingIn)
            {
                if(strengt <= 0)
                {
                    flashingIn = false;
                }
                else
                {
                    strengt     -= 25;
                    redCol      = strengt;
                    blueCol     = strengt;
                    greenCol    = strengt;
                }
            }
            
            if(flashingIn == false)
            {
                if(strengt >= 255)
                {
                    flashingIn = true;
                }
                else
                {
                    strengt += 25;
                    redCol = strengt;
                    blueCol  = strengt;
                    greenCol = strengt;
                }
            }
        }
    }

}
