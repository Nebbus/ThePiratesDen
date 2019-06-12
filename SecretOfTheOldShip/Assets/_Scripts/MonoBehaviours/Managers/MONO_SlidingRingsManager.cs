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

    [Space]
    [Space]

    public Transform innerRt;
    public Transform midleRt;
    public Transform outerRt;


    [Space]
    [Space]


    public TEST innerR;
    public TEST midleR;
    public TEST outerR;
    [Space]
    [Space]
   public float tempIn;
   public float tempMi;
   public float tempOu;

    [Space]
    public bool rotating = false;
    public void Exit()
	{
        //sceneManager.ChangeScene("Scene1_outside", false, true, false, true, true, true, true);
        string newScene = "Scene1_outside";
        bool handelInputAfterLoad       = true;
        bool saveDataBefforChangeGame   = true;
        bool loadDataAfterLoad          = true;
        bool loadedGame                 = false; // not used
        MONO_SceneManager.changeScenType changeType = MONO_SceneManager.changeScenType.SCENEtoSCENE;
        sceneManager.ChangeScene(newScene, loadedGame, handelInputAfterLoad, saveDataBefforChangeGame, loadDataAfterLoad, changeType);

    }

	private void Start()
	{
		sceneManager = FindObjectOfType<MONO_SceneManager> ();
	}


    private float round(float value)
    {
        float temp = value * 100f;
        temp = Mathf.Round(temp);

        return temp/100f;

    }

	private void Update () 
	{
        //Modulo ();
   
         tempIn = round(innerRt.eulerAngles.z);
         tempMi = round(midleRt.eulerAngles.z);
         tempOu = round(outerRt.eulerAngles.z);

        setTheRingsModes();

        rotating = flowChart.GetBooleanVariable("RotIng");

        /*Prevents from cheking if the rotation isent done,
         * prevents that the puzzle get finished then the
         * ring just passes the corect position
         */
        if (rotating)
        {
            return;
        }


        if (tempIn == 0f && tempMi == 0f && tempOu == 0f && !finished) 
		{
			finished = true;
			PuzzleFinished ();
		}





    }
    /// <summary>
    /// Break out to make the uppdate
    /// funktion easyer to read
    /// </summary>
    private void setTheRingsModes()
    {

        if (finished)
        {
            if (innerR.currentMod != TEST.MODE.GLITTER)
            {
                innerR.currentMod = TEST.MODE.GLITTER;
                innerR.BlockChanges = true;
            }
            if (midleR.currentMod != TEST.MODE.GLITTER)
            {
                midleR.currentMod = TEST.MODE.GLITTER;
                midleR.BlockChanges = true;

            }
            if (outerR.currentMod != TEST.MODE.GLITTER)
            {
                outerR.currentMod = TEST.MODE.GLITTER;
                outerR.BlockChanges = true;
            }

        }
        else
        {
            setRingMode(tempIn, innerR);
            setRingMode(tempMi, midleR);
            setRingMode(tempOu, outerR);
        }

    }


    /// <summary>
    /// set the ring to glitter if 
    /// it is att 0
    /// </summary>
    /// <param name="value"></param>
    /// <param name="script"></param>
    private void setRingMode(float value, TEST script)
    {
        if (value == 0f)
        {
            if (script.currentMod != TEST.MODE.GLITTER)
            {
                script.currentMod = TEST.MODE.GLITTER;
                script.BlockChanges = true;
            }
        }
        else
        {
            if (script.currentMod == TEST.MODE.GLITTER)
            {
                script.currentMod = TEST.MODE.DEFAULT;
                script.BlockChanges = false;
            }
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

	private void PuzzleFinished()
	{
		flowChart.ExecuteBlock ("Finished");
		//yield return new WaitForSeconds (3);
		//Here's where we change the condition allowing us to use the door normally
		//sceneManager.ChangeScene("Scene2_inside", false, false);

	}

}
