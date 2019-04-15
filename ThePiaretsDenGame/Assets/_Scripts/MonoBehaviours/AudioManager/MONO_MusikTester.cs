using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

/// <summary>
/// This script is only for testing of the 
/// muski an ambiens
/// </summary>
public class MONO_MusikTester : MonoBehaviour {

    public StudioEventEmitter musiken;
    public StudioEventEmitter ambiens;

    public float area;
    public float ambien;


    void Start ()
    {
        area   = 0;
        ambien = 0;

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F))
        {
         
            if (area == 0f)
            {
                Debug.Log("0");
                musiken.SetParameter("Area", 1.0f);           
                ambiens.SetParameter("AMB_Switch", 1.0f);
                area   = 1f;
                ambien = 1f;
            }
            else if (area == 1f)
            { musiken.SetParameter("Area", 0.0f);

                ambiens.SetParameter("AMB_Switch", 0.0f);
                area   = 0f;
                ambien = 0f;
            }
        }
	}
}
