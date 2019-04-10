using System.Collections;
using UnityEngine;

public class MONO_CameraRotation : MonoBehaviour
{
	public bool moveCamera 		= true;                     	// Whether the camera should be moved by this script.    
	public float smoothing 		= 200f;                       	// Smoothing applied during Slerp, higher is smoother but slower.
	public float currentDist;
	public Transform playerPosition;                    		// Reference to the player's Transform to aim at.
	public GameObject firstPoint;
	public GameObject[] camPoints;

	private GameObject currentPoint;

	private IEnumerator Start()
	{
		// If the camera shouldn't move, do nothing.
		if(!moveCamera)
			yield break;

		// Wait a single frame to ensure all other Starts are called first.
		yield return null;

		//Set camera to first point
		currentPoint = firstPoint;
		currentDist = Vector3.Distance (firstPoint.transform.position, transform.position);
	}


	private void LateUpdate()
	{
		// If the camera shouldn't move, do nothing.
		if (!moveCamera)
		{
			return;
		}

		//Update distance to compare other points with 
		currentDist = Vector3.Distance (currentPoint.transform.position, playerPosition.position);

		//Find closest point by comparing distances between current point and all points.
		for(int i = 0; i < camPoints.Length; i++)
		{
			float newDist = Vector3.Distance (camPoints[i].transform.position, playerPosition.position);
			if(newDist < currentDist)
			{
				currentPoint = camPoints [i];
			}
		}

		// Find a new rotation aimed at the player's position with a given offset.
		Quaternion newRotation = Quaternion.LookRotation (currentPoint.transform.position - transform.position);

		//Move towards closest point.
		transform.rotation = Quaternion.Slerp (transform.rotation, newRotation, Time.deltaTime * smoothing);
	}
}
	