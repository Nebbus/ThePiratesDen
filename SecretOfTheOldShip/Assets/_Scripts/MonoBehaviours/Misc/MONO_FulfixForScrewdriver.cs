using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MONO_FulfixForScrewdriver : MonoBehaviour {

	public GameObject screwdriverButton;
	public Fungus.Flowchart variableFlowchart1;

	// Use this for initialization
	void Start () {
		if (variableFlowchart1.GetBooleanVariable("Screwdriver") == true) 
		{
			screwdriverButton.SetActive (true);
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
