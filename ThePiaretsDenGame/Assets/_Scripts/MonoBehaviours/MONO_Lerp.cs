using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MONO_Lerp : MonoBehaviour {

	public Transform startPosition;
	public Transform endPosition;
	public float lerpSpeed = 1.0f;
	public bool alwaysLerping;
	public bool turn;


	private bool lerping;
	private float lerpStartTime;
	private float journeyLength;
	private Vector3 startMarker;
	private Vector3 endMarker;


	// Use this for initialization
	void Start () 
	{
		startMarker = startPosition.position;
		endMarker = endPosition.position;
		if (alwaysLerping) 
		{
			StartLerp (false);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{

		if (lerping)
		{
			float distCovered = (Time.time - lerpStartTime) * lerpSpeed;
			float fracJourney = distCovered / journeyLength;
			transform.position = Vector3.Lerp (startMarker, endMarker, fracJourney);

			if (turn) 
			{
				transform.rotation = Quaternion.Slerp (startPosition.rotation, endPosition.rotation, fracJourney);
			}

			if (Mathf.Approximately (transform.position.x, endMarker.x)
				&& Mathf.Approximately (transform.position.z, endMarker.z)) 
			{
				lerping = false;
				if (!turn) {
					transform.Rotate (new Vector3 (0, 180, 0));
				}
			}


		} 
		else if(alwaysLerping)
		{
			Vector3 temp = startMarker;
			startMarker = endMarker;
			endMarker = temp;
			StartLerp (false);
		}


	}


	public void StartLerp(bool keepLerping)
	{
		lerping = true;
		lerpStartTime = Time.time;
		journeyLength = Vector3.Distance (startMarker, endMarker);

		if (keepLerping) 
		{
			alwaysLerping = true;
		}
	}

}
