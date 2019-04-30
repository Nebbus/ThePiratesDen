using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MONO_pointerLogic : MonoBehaviour {


    public enum action { HOVER, CLICK};

    //[Tooltip("Debug.Log the name of the things the raycast hits")]
    //public bool debug             = false;
    //public bool debugOnlyFirstHit = true;

    [Tooltip("The name of the interactable object (graphical or fysikal) the raycast hits")]
    public string overCurentObject;
    [Space]

//===========================================================================
// Raycaster stuff
//===========================================================================
    private GraphicRaycaster m_Graycaster;
    private PhysicsRaycaster m_Praycaster;
    private EventSystem      m_EventSystem;

    private List<RaycastResult> resultsG;
    private List<RaycastResult> resultsP;

    private RectTransform thisTransformer;

    private PhysicsRaycaster getPraycaster
    {
        get
        {
            if(m_Praycaster == null || !m_Praycaster.gameObject.activeSelf)
            {
                m_Praycaster = FindObjectOfType<PhysicsRaycaster>();
            }

            return m_Praycaster;
        }
    }

//===========================================================================
// detection and action stuff (desidig that to do)
//===========================================================================

    private MONO_interactionBase interactableTarget;
    public action currentAction = action.HOVER;


//===========================================================================
// Ínput stuff
//===========================================================================

    //temporär lösnign
    [SerializeField]
    private KeyCode usedClickKey = KeyCode.Mouse0;
    [SerializeField]
    private KeyCode mouseKey    = KeyCode.Mouse0;
    [SerializeField]
    private KeyCode keabordKey   = KeyCode.Space;

    public void setKeyBordMode()
    {
        usedClickKey = keabordKey;
    }
    public void setMouseMode()
    {
        usedClickKey = mouseKey;
    }








    // Use this for initialization
    void Start ()
    {
        thisTransformer = GetComponent<RectTransform>();

        m_Praycaster    = FindObjectOfType<PhysicsRaycaster>();
        m_Graycaster    = FindObjectOfType<GraphicRaycaster>();
        m_EventSystem   = FindObjectOfType<EventSystem>();
     


    }

    // Update is called once per frame
    void Update()
    {
        currentAction = Input.GetKeyDown(usedClickKey) ? action.CLICK : action.HOVER;

        resultsP = EXT_GraphicalRayCast.PhysicalRayCast(getPraycaster, m_EventSystem, thisTransformer.position);
        resultsG = EXT_GraphicalRayCast.GrapphicRayCast(m_Graycaster , m_EventSystem, thisTransformer.position);
        
        //if (debug)
        //{
        //    DebugHits();
        //}
        handleResult();
    }

    ///// <summary>
    ///// Debugs out all hits that hitted a MONO_interactionBase
    ///// </summary>
    //private void DebugHits()
    //{
    //    //Debugs ut
    //    int count = 0;
    //    foreach (RaycastResult result in resultsG)
    //    {
    //        MONO_interactionBase interactable = result.gameObject.GetComponentInParent<MONO_interactionBase>();
    //        if (interactable)
    //        {
    //            Debug.Log("Graphical Hit " + count + " : " + result.gameObject.name + " Has a MONO_interactionBase");
    //        }
    //        if (debugOnlyFirstHit)
    //        {
    //            break;
    //        }
    //        count++;
    //    }
    //    count = 0;

    //    foreach (RaycastResult result in resultsP)
    //    {
    //        MONO_interactionBase interactable = result.gameObject.GetComponent<MONO_interactionBase>();

    //        MONO_Interactable temp = interactable as MONO_Interactable;

    //        if (temp)
    //        {
    //            Debug.Log("Physical Hit " + count + " : " + result.gameObject.name + " Has a MONO_Interactable");
    //        }
    //        else if (interactable)
    //        {
    //            Debug.Log("Physical Hit " + count + " : " + result.gameObject.name + " Has a MONO_interactionBase");

    //        }
    //        else if (tag == "GROUND")
    //        {

    //            Debug.Log("Physical Hit " + count + " : " + result.gameObject.name + " is Ground");
    //        }
    //        if (debugOnlyFirstHit)
    //        {
    //            break;
    //        }
    //        count++;
    //    }
      
  
    //}

    /// <summary>
    /// Controls if a monotarget was hitted
    /// </summary>
    private void handleResult()
    {	
        interactableTarget = null;


        //==================================================================================
        // Handel Graphical interactions
        //==================================================================================
        foreach (RaycastResult result in resultsG)
        {
            interactableTarget = result.gameObject.GetComponentInParent<MONO_interactionBase>();
            if (interactableTarget)
            {
                overCurentObject = result.gameObject.name;
                simpleInteract();
                return;
            }
            overCurentObject = "---------";
            break;        //Only considerts the first hit
        }

        //==================================================================================
        // Handel physical interactions
        //==================================================================================
        foreach (RaycastResult result in resultsP)
        {
            interactableTarget = result.gameObject.GetComponent<MONO_interactionBase>();

            if (interactableTarget)
            {
                /* If it wasent a interactable so 
                 * was it a simpleInteraction*/
                if (!Interactable(result, interactableTarget))
                {
                    simpleInteract();
                }
                overCurentObject = result.gameObject.name;
                return;
            }
            else if(result.gameObject.tag == "GROUND")
            {
                GroundClick(result);
                overCurentObject = result.gameObject.name;
                return;
            }
            overCurentObject = "---------";
            break;
        }

        overCurentObject = "---------";
    }


    /// <summary>
    /// The interactable in the world 
    /// </summary>
    /// <param name="result">the curent click result</param>
    /// <returns>tru if the click was on a interactblee , oter wise false</returns>
    private bool Interactable(RaycastResult result, MONO_interactionBase interacTarget)
    {
        MONO_Interactable interactable = interacTarget as MONO_Interactable;// result.gameObject.GetComponent<MONO_Interactable>();

        if (interactable)
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
        if (currentAction == action.CLICK)
        {

            MONO_EventManager.EventParam param = new MONO_EventManager.EventParam();
            param.param5                       = result.worldPosition;
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
