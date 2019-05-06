using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MONO_pointerLogic : MonoBehaviour {


    public enum action { HOVER, CLICK};




//===========================================================================
// DEBUG
//===========================================================================

    public string overCurentObject;

    public bool debugAll;
    public string[] allGraphicalHitsDebug = new string[1];
    public string[] allPhysicalHitsDebug  = new string[1];


//===========================================================================
// Raycaster stuff
//===========================================================================
    public GraphicRaycaster mainCameraGraycaster;
    public PhysicsRaycaster presistentCanvansPraycaster;
    private EventSystem     presistentSeneEventSystem;

    private List<RaycastResult> resultsG;
    private List<RaycastResult> resultsP;

    private RectTransform thisTransformer;

    private PhysicsRaycaster getPraycaster
    {
        get
        {
            if(presistentCanvansPraycaster == null || !presistentCanvansPraycaster.gameObject.activeSelf)
            {
                presistentCanvansPraycaster = FindObjectOfType<PhysicsRaycaster>();
            }

            return presistentCanvansPraycaster;
        }
    }

    private GraphicRaycaster getGraycaster
    {
        get
        {
            if (mainCameraGraycaster == null || !mainCameraGraycaster.gameObject.activeSelf)
            {
                mainCameraGraycaster = FindObjectOfType<GraphicRaycaster>();
            }

            return mainCameraGraycaster;
        }
    }


    //===========================================================================
    // detection and action stuff (desidig that to do)
    //===========================================================================
    private MONO_interactionBase interactableTarget;
    private Button               buttonTarger;
    public action currentAction = action.HOVER;


    public double lastClickTime = 0f;
    public double timeThreshold = 0.1f;

    public double timedelta = 0.1f;


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

        presistentCanvansPraycaster    = FindObjectOfType<PhysicsRaycaster>();
        mainCameraGraycaster           = FindObjectOfType<GraphicRaycaster>();
        presistentSeneEventSystem      = FindObjectOfType<EventSystem>();
    }

    // Update is called once per frame
    void Update()
    {


        currentAction = Input.GetKeyDown(usedClickKey) ? action.CLICK : action.HOVER;
        if(currentAction == action.CLICK)
        {
       ;
            timedelta        = Time.time - lastClickTime;
            lastClickTime    = Time.time;
            if ( timedelta <= timeThreshold)
            {
                currentAction = action.HOVER;
            }
     
        }



        resultsP = EXT_GraphicalRayCast.PhysicalRayCast(getPraycaster, presistentSeneEventSystem, thisTransformer.position);
        resultsG = EXT_GraphicalRayCast.GrapphicRayCast(getGraycaster, presistentSeneEventSystem, thisTransformer.position);

        if (debugAll)
        {
            debugAllG();
            debugAllP(); 
        }
    
        handleResult();
    }

  
    private void debugAllG()
    {
       
        int count = 0;
        foreach (RaycastResult result in resultsG)
        {
            count++;
        }
        allGraphicalHitsDebug = new string[count];
        count = 0;
        foreach (RaycastResult result in resultsG)
        {

            allGraphicalHitsDebug[count] = result.gameObject.name;
            count++;
        }
    }
    private void debugAllP()
    {

        int count = 0;
        foreach (RaycastResult result in resultsP)
        {
            count++;
        }
        allPhysicalHitsDebug = new string[count];
        count = 0;
        foreach (RaycastResult result in resultsP)
        {

            allPhysicalHitsDebug[count] = result.gameObject.name;
            count++;
        }
    }
    
    
    /// <summary>
    /// Controls if a monotarget was hitted
    /// </summary>
    private void handleResult()
    {	
        interactableTarget = null;
        buttonTarger       = null;

        //==================================================================================
        // Handel Graphical interactions
        //==================================================================================
        foreach (RaycastResult result in resultsG)
        {
            interactableTarget = result.gameObject.GetComponentInParent<MONO_interactionBase>();
            buttonTarger       = result.gameObject.GetComponentInParent<Button>();
           
            if (interactableTarget)
            {
                overCurentObject = result.gameObject.name;
                simpleInteract();
                return;
            }
            else if (buttonTarger)
            {
                overCurentObject = "Button: "+ buttonTarger.gameObject.name;
                ButtonInteraction();
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

    /// <summary>
    /// for handeling GUI buttons
    /// </summary>
    private void ButtonInteraction()
    {
        if (currentAction == action.CLICK)
        {
            buttonTarger.onClick.Invoke();
        }
        else
        {
            
        }

    }


}
