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


	void LateUpdate () 
	{
		//Scale the size of interactionalternatives to always look the same size no matter the distance from camera.
		camDist = Vector3.Distance(this.transform.position, camera.transform.position);
		this.transform.localScale = new Vector3(camDist*0.3f,camDist*0.3f,camDist*0.3f);
	}
}
