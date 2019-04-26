using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MONO_pointerLogic : MonoBehaviour {


    public enum action { HOVER, CLICK};

    [Tooltip("Debug.Log the name of the things the raycast hits")]
    public bool debug = false;
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

	// Update is called once per frame
	void Update ()
	{
	    
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
        if(m_Praycaster != null)
        {
            resultsP = EXT_GraphicalRayCast.PhysicalRayCast(m_Praycaster, m_EventSystem);
        }
        else
        {
            Debug.Log("ERROR: ingen PhysicsRaycaster hittat bugg undviken med quc fix");
        }
     

        if (debug)
        {
            DebugHits();
        }
        if (m_Praycaster != null)
        {
            handleResult();
        }
        else
        {
            Debug.Log("ERROR: ingen PhysicsRaycaster hittat bugg undviken med quc fix");
        }

        
       
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
            else if (tag == "GROUND")
            {

                Debug.Log("Physical Hit G " + count + " : " + result.gameObject.name + " Has a MONO_interactionBase");
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
                simpleInteract();
                return;
            }
            break;
        }
        foreach (RaycastResult result in resultsP)
        {
            interactableTarget = result.gameObject.GetComponent<MONO_interactionBase>();

       
            if (interactableTarget)
            {
                if (Interactable(result, interactableTarget))
                {
                    return;
                } 
                else 
                {
                    simpleInteract();
                }
            }
            else
            {
                GroundClick(result);
            }
           


            break;
        } 
    }

    /// <summary>
    /// The interactable in the world 
    /// </summary>
    /// <param name="result">the curent click result</param>
    /// <returns>tru if the click was on a interactblee , oter wise false</returns>
    private bool Interactable(RaycastResult result, MONO_interactionBase interacTarget)
    {
       
        MONO_Interactable test = (MONO_Interactable)interacTarget;
        if (test)
        {
            if (currentAction == action.CLICK)
            {
                MONO_EventManager.EventParam param = new MONO_EventManager.EventParam();
                param.param6                       = interacTarget;
                MONO_EventManager.TriggerEvent(MONO_EventManager.onInteractableEvnetManager_NAME, param);

            }
            else
            {
                interactableTarget.OnHovor();
            }

            return true;
        }
        return false;
    }

    /// <summary>
    /// the click on the grund
    /// </summary>
    /// <param name="result">the curent click result</param>
    /// <returns>tru if the click was on teh ground, oter wise false</returns>
    private bool GroundClick(RaycastResult result)
    {
        if (result.gameObject.tag == "GROUND" && currentAction == action.CLICK)
        {

            MONO_EventManager.EventParam param = new MONO_EventManager.EventParam();
            param.param5 = result.worldPosition;
            MONO_EventManager.TriggerEvent(MONO_EventManager.onGroundEvnetManager_NAME, param);
            return true;
        }
        return false;
    }


    /// <summary>
    /// simple interaction, for gui buttons and alike
    /// </summary>
    private void simpleInteract()
    {


        if (currentAction == action.CLICK)
        {
            interactableTarget.OnClick();
        }
        else
        {
            interactableTarget.OnHovor();
        }

    }
}
