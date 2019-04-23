using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MONO_RenderInteractOptions : MonoBehaviour {

	private Camera camera;
	private float camDist;

	void Start()
	{
		camera = FindObjectOfType<Camera> ();
	}

	// Update is called once per frame
	void LateUpdate () 
	{
		camDist = Vector3.Distance(this.transform.position, camera.transform.position);
		this.transform.localScale = new Vector3(camDist*0.3f,camDist*0.3f,camDist*0.3f);
	}
}
