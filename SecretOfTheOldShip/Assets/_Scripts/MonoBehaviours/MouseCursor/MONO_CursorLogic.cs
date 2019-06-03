using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class MONO_CursorLogic : MonoBehaviour
{
    public enum action { HOVER_ENTERD, HOVER_OVER, HOVER_EXIT, HOVER, CLICK };

	//===========================================================================
	// Cursor sprite
	//===========================================================================
	private MONO_CursorSprite cursorSprite;

    //===========================================================================
    // DEBUG
    //===========================================================================

    public string overCurentObject;


    public bool debugAll;
    public string[] allGraphicalHitsDebug = new string[1];
    public string[] allPhysicalHitsDebug = new string[1];


    //===========================================================================
    // Raycaster stuff
    //===========================================================================
    public  GraphicRaycaster presistentCanvansPraycaster;
    public  PhysicsRaycaster mainCameraGraycaster;
    private EventSystem      presistentSeneEventSystem;
    private MONO_SceneManager monoSceneManager;


    private List<RaycastResult> resultsG;
    private List<RaycastResult> resultsP;

    private RectTransform thisTransformer;

    private GraphicRaycaster getGraycaster
    {
        get
        {
            if (presistentCanvansPraycaster == null || !presistentCanvansPraycaster.gameObject.activeSelf)
            {
                presistentCanvansPraycaster = FindObjectOfType<GraphicRaycaster>();
            }

            return presistentCanvansPraycaster;
        }
    }

    private PhysicsRaycaster getPraycaster  
    {
        get
        {
            if (mainCameraGraycaster == null || !mainCameraGraycaster.gameObject.activeSelf)
            {
                mainCameraGraycaster = FindObjectOfType<PhysicsRaycaster>();
            }

            return mainCameraGraycaster;
        }
    }


    //===========================================================================
    // detection and action stuff (desidig that to do)
    //===========================================================================

    public GameObject           currentHoverOver           = null;
    public MONO_InteractionBase currentInteractableTarget  = null;
    public Selectable           currentUIelemt             = null;

    public GameObject           lastHoverOver              = null;
    public MONO_InteractionBase lastInteractableTarget     = null;
    public Selectable           lastUIelemt                = null;


    public action currentAction = action.HOVER;


    private GameObject lastHoverdOver = null;
    private bool enterdHover = false;


    public double lastClickTime = 0f;
    public double timeThreshold = 0.1f;
    public double timedelta = 0.1f;


    //===========================================================================
    // Ínput stuff
    //===========================================================================

   

    public Vector3 getPointerPosition
    {
        get
        {
            if (thisTransformer != null)
            {
                thisTransformer = GetComponent<RectTransform>();
            }
            return thisTransformer.position;
        }
       
    }

  
    void Start()
    {
        thisTransformer = GetComponent<RectTransform>();
		cursorSprite = GetComponent<MONO_CursorSprite> ();

        presistentCanvansPraycaster = FindObjectOfType<GraphicRaycaster>();
        mainCameraGraycaster        = FindObjectOfType<PhysicsRaycaster>();
        presistentSeneEventSystem   = FindObjectOfType<EventSystem>();
        monoSceneManager            = FindObjectOfType<MONO_SceneManager>();
    }

    void Update()
    {
        getCurentAction();

        resultsP = EXT_RayCast.PhysicalRayCast(getPraycaster, presistentSeneEventSystem, thisTransformer.position);
        resultsG = EXT_RayCast.GraphicRayCast(getGraycaster, presistentSeneEventSystem, thisTransformer.position);

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
    /// Gets the current action,
    /// </summary>
    private void getCurentAction()
    {

        currentAction = action.HOVER; 

        if (MONO_Settings.instance.getClickKey)
        {
            //Prevents dubble click from causing truble
            timedelta      = Time.time - lastClickTime;
            lastClickTime  = Time.time;
            if (timedelta >= timeThreshold)
            {
                currentAction = action.CLICK;
            }
        }


    }



    /// <summary>
    /// Controls if a monotarget was hitted
    /// </summary>
    private void handleResult()
    {
        overCurentObject            = "---------";
        currentHoverOver            = null;
        currentInteractableTarget   = null;



        //RaycastResult theLa
        //==================================================================================
        // Handel Graphical interactions
        //==================================================================================
        foreach (RaycastResult result in resultsG)
        {
            // get the object and all that is attached to it
            currentHoverOver            = result.gameObject;
            currentInteractableTarget   = currentHoverOver.GetComponentInParent<MONO_InteractionBase>();
            currentUIelemt = currentHoverOver.GetComponentInParent<Selectable>();

            if (currentInteractableTarget && monoSceneManager.getSetHandleInput)
            {
                overCurentObject = currentHoverOver.name;
                HandleSimpleInteract();
            }
            else if (currentUIelemt)
            {
                overCurentObject = "UI: " +currentHoverOver.name;
                HandleSelectabeUIinteraction();
            }
            InteractableHover();
            SelactableUIhover();
            return;// Only considerts the first hit
        }

        /* dot read inputs (exept buttons to make pause
         * pause button work, as well as fungus
         * menu dialog
         */ 
        if (!monoSceneManager.getSetHandleInput)
        {
            return;
        }
    


        //==================================================================================
        // Handel physical interactions
        //==================================================================================
        foreach (RaycastResult result in resultsP)
        {
            currentHoverOver = result.gameObject;
            currentInteractableTarget = currentHoverOver.GetComponentInParent<MONO_InteractionBase>();


            if (currentInteractableTarget)
            {
                /* If it wasent a interactable so 
                 * was it a simpleInteraction*/
                if (!HandleInteractable())
                {
                    HandleSimpleInteract();
                }
                overCurentObject = currentHoverOver.name;
            }
            else if (currentHoverOver.tag == "GROUND")
            {
                HandleGroundClick(result);
                overCurentObject = "GROUND: " + currentHoverOver.name;
            }
            InteractableHover();
            SelactableUIhover();
            return;// Only considerts the first hit
        }

        if (lastInteractableTarget != null)
        {
            OnHoverExitRaper();
        }
    }


   
    /// <summary>
    /// the click on the grund
    /// </summary>
    /// <param name="result">the curent click result</param>
    private void HandleGroundClick(RaycastResult result)
    {
        if (currentAction == action.CLICK)
        {
            MONO_EventManager.EventParam param = new MONO_EventManager.EventParam();
            param.param5 = result.worldPosition;
            MONO_EventManager.TriggerEvent(MONO_EventManager.onGroundEvnetManager_NAME, param);
    
        }

    }

   
    /// <summary>
    /// for handeling GUI buttons annd toggles
    /// </summary>
    private void HandleSelectabeUIinteraction()
    {


        if (currentAction == action.CLICK && currentUIelemt.interactable)
        {
           
            Button buttonTarget = currentUIelemt as Button;
            Toggle toggleTarget = currentUIelemt as Toggle;
           // Slider sliderTarget = currentUIelemt as Slider;

            if (buttonTarget)
            {
                buttonTarget.onClick.Invoke();

            }
            if (toggleTarget)
            {
                toggleTarget.isOn = !toggleTarget.isOn; 
            }



        }

    }

    /// <summary>
    /// The interactable in the world 
    /// </summary>
    /// <param name="result">the curent click result</param>
    /// <returns>tru if the click was on a interactblee , oter wise false</returns>
    private bool HandleInteractable()
    {
        MONO_Interactable interactable = currentInteractableTarget as MONO_Interactable;

        if (interactable)
        {
            if (currentAction == action.CLICK)
            {
                MONO_EventManager.EventParam param = new MONO_EventManager.EventParam();
                param.param6                       = currentInteractableTarget;
                MONO_EventManager.TriggerEvent(MONO_EventManager.onInteractableEvnetManager_NAME, param);
            }

            return true;
        }
        return false;
    }

    /// <summary>
    /// simple interaction, for gui buttons and alike
    /// </summary>
    private void HandleSimpleInteract()
    {
        if (currentAction == action.CLICK)
        {
            currentInteractableTarget.OnClick();
        }


    }


   private void SelactableUIhover()
    {
        if (lastUIelemt != currentUIelemt)
        {
            lastUIelemt = currentUIelemt;
   
            if (currentUIelemt)
            {
                currentUIelemt.Select();
                presistentSeneEventSystem.SetSelectedGameObject(currentUIelemt.gameObject);

            }
            else
            {
                presistentSeneEventSystem.SetSelectedGameObject(null);

            }
        }
      
    }


    /// <summary>
    /// handle the hover reaction, 
    /// it calls the on enterd/ented/leaft
    /// </summary>
    private void InteractableHover()
    {
        if(lastInteractableTarget != currentInteractableTarget)
        {
            if (lastInteractableTarget)
            {
                OnHoverExitRaper();
            }

            if (currentInteractableTarget)
            {
                OnHoverEnterRaper();          
            }

            lastInteractableTarget = currentInteractableTarget;
        }
        else
        {
            if (currentInteractableTarget)
            {
                currentInteractableTarget.OnHover();

            }
        }
     
    }


    private void OnHoverEnterRaper()
    {
        currentInteractableTarget.OnHoverEnterd();
    }

    private void OnHoverExitRaper()
    {
        lastInteractableTarget.OnHoverExit();
        lastInteractableTarget = null;
    }








}



