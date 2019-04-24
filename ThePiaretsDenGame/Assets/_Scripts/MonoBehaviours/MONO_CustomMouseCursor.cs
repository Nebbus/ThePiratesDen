using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MONO_CustomMouseCursor : MonoBehaviour {

	public GameObject Cursor;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Input.GetAxis ("Horizontal");
	}
}
