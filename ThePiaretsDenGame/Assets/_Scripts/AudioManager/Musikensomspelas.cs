using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Musikensomspelas : MonoBehaviour {

    public StudioEventEmitter musiken;
    public StudioEventEmitter ambiens;
    public float miljö;
    public float ambien;
    // Use this for initialization
    void Start () {
        miljö = 0;
        ambien = 0;

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F))
        {
         
            if (miljö == 0f)
            {
                Debug.Log("0");
                musiken.SetParameter("Area", 1.0f);
                miljö = 1f;
                ambiens.SetParameter("AMB_Switch", 1.0f);
                ambien = 1f;
            }
            else if (miljö == 1f)
            { musiken.SetParameter("Area", 0.0f);
                miljö = 0f;
                ambiens.SetParameter("AMB_Switch", 0.0f);
                ambien = 0f;
                Debug.Log("1");
            }
        }
	}
}
