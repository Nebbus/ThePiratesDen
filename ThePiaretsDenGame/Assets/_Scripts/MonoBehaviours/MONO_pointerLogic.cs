using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MONO_pointerLogic : MonoBehaviour {


    public enum action { HOVER, CLICK};

    [Tooltip("Debug.Log the name of the things the raycast hits")]
    public bool debug = false;
    public MONO_PlayerMovement playerMovment;
    private GraphicRaycaster m_Graycaster;
    private PhysicsRaycaster m_Praycaster;
    private EventSystem      m_EventSystem;

    private List<RaycastResult>     resultsG;
    private List<RaycastResult>     resultsP;
    private MONO_interactionBase    interactableTarget;

    public action currentAction = action.HOVER;

    // Use this for initialization
    void Start ()
    {
        m_Praycaster    = FindObjectOfType<PhysicsRaycaster>();
        m_Graycaster    = FindObjectOfType<GraphicRaycaster>();
        m_EventSystem   = FindObjectOfType<EventSystem>();
	
    }


	private void getPlayerMomvent()
	{
		playerMovment   = FindObjectOfType<MONO_PlayerMovement>();
	}

	// Update is called once per frame
	void Update ()
	{
		if (playerMovment == null) 
		{
			getPlayerMomvent ();
		}
		
     
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            currentAction = action.CLICK;
        }
        else
        {
               currentAction = action.HOVER;
        }

		if (m_Praycaster == null) 
		{
			// temp fix to make it fin the physical raycaster i n every scene
			m_Praycaster    = FindObjectOfType<PhysicsRaycaster>();
		}


        resultsG = EXT_GraphicalRayCast.GrapphicRayCast(m_Graycaster, m_EventSystem);
        resultsP = EXT_GraphicalRayCast.PhysicalRayCast(m_Praycaster, m_EventSystem);

        if (debug)
        {
            DebugHits();
        }

       handleResult();
       
    }

    /// <summary>
    /// Debugs out all hits that hitted a MONO_interactionBase
    /// </summary>
    private void DebugHits()
    {
        int count = 0;
        foreach (RaycastResult result in resultsG)
        {
            MONO_interactionBase interactable = result.gameObject.GetComponentInParent<MONO_interactionBase>();
            if (interactable)
            {
                Debug.Log("Graphical Hit " + count + " : " + result.gameObject.name + " Has a MONO_interactionBase");
            }
            count++;
        }
        count = 0;

        foreach (RaycastResult result in resultsP)
        {
            MONO_interactionBase interactable = result.gameObject.GetComponent<MONO_interactionBase>();

            if (interactable)
            {
                Debug.Log("Physical Hit "+ count +" : " + result.gameObject.name + " Has a MONO_interactionBase");
            }
            count++;
        }
      
  
    }


    /// <summary>
    /// Controls if a monotarget was hitted
    /// </summary>
    private void handleResult()
    {
		

        interactableTarget = null;
        //Loks att the first item in the list to se if it has MONO_interactionBase
        foreach (RaycastResult result in resultsG)
        {
            interactableTarget = result.gameObject.GetComponentInParent<MONO_interactionBase>();
            if (interactableTarget)
            {
                if (currentAction == action.CLICK)
                {
                    interactableTarget.OnClick();
                }
                else
                {
                    interactableTarget.OnHovor();
                }
                return;
            }
            break;
        }
        foreach (RaycastResult result in resultsP)
        {
           
           interactableTarget = result.gameObject.GetComponent<MONO_interactionBase>();
           
            if (interactableTarget)
            {
                if (currentAction == action.CLICK)
                {
                    if (playerMovment)
                    {

                        playerMovment.OnInteractableClick((MONO_Interactable)interactableTarget);
                    }
                 
                }
                else
                {
                    interactableTarget.OnHovor();
                }

                return;
            }
            break;
        } 
    }
}
