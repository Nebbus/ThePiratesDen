using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class MONO_SlidingRingsManager : MonoBehaviour {

	public Flowchart flowChart;

	private int tempInner;
	private int tempMiddle;
	private int tempOuter;
	private MONO_SceneManager sceneManager;

	private bool finished = false;

	public void Exit()
	{
		sceneManager.ChangeScene("Scene1_outside", false, true);
	}

	private void Start()
	{
		sceneManager = FindObjectOfType<MONO_SceneManager> ();
	}

	private void Update () 
	{
		Modulo ();

		if (tempInner == 0 && tempMiddle == 0 && tempOuter == 0 && !finished) 
		{
			finished = true;
			StartCoroutine (PuzzleFinished ());
		}
	}

	private void Modulo()
	{
		tempInner =  flowChart.GetIntegerVariable("InnerCircle");
		tempMiddle =  flowChart.GetIntegerVariable("MiddleCircle");
		tempOuter =  flowChart.GetIntegerVariable("OuterCircle");

		tempInner = tempInner % 8;
		tempMiddle = tempMiddle % 8;
		tempOuter = tempOuter % 8;

		flowChart.SetIntegerVariable ("InnerCircle", tempInner);
		flowChart.SetIntegerVariable ("MiddleCircle", tempMiddle);
		flowChart.SetIntegerVariable ("OuterCircle", tempOuter);
	}

	private IEnumerator PuzzleFinished()
	{
		flowChart.ExecuteBlock ("Finished");
		yield return new WaitForSeconds (3);
		//Here's where we change the condition allowing us to use the door normally
		sceneManager.ChangeScene("Scene2_inside", false, false);

	}

}
