using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class MONO_CustomMouseCursor : MonoBehaviour {

    public bool lockkurso = true;
    public GameObject CustomCursor;
    public  float  CursorSpeed = 4;

    [SerializeField]
    [Tooltip("Pixel distance the mouse points out on the right side and on the bottom")]
    private float pointerSafeDist = 5;

    private RectTransform   CustomCursorTransform;
    private Canvas presistentCanvas; // must be presisten canvas to work
    private RectTransform CanvasRect;
   
    
    //===========================================================================
    // Kebord accesebility stuff
    //===========================================================================
    [Space]
    public int selectedIndex = 0;
    public int currentIndex = -1;
    [Space]
    public Selectable[] interacrablebaseObjectsInScene = new Selectable[1];
    public Selectable[] inventorySLots = null;

    public Selectable closestInteractable;
    [Space]
    public GameObject DebugselecetGamobject;

    [SerializeField]
    private Transform currentTransform;


    public bool inventoryMod = false;
    public bool inventoryOppen
    {
        get
        {
        
           return inventorySLots.Length != 0;
        }
    }

    public Selectable getCurretItem
    {
        get
        {
            Selectable temp = (inventoryMod) ? inventorySLots[currentIndex] : interacrablebaseObjectsInScene[currentIndex];
            DebugselecetGamobject = (temp == null) ? null : temp.gameObject;
            return temp;
        }

    }

    public enum kebordMovmentMod { SCROLL, CLOSESTTARGET };

    public kebordMovmentMod curretnKebordMod = kebordMovmentMod.CLOSESTTARGET;


    public MONO_PlayerMovement player;
    private Vector3 playerLastPos;

    private bool getPlayerMoved
    {
        get
        {
            if (getPlayerExsist)
            {
                float minDistans = 0.05f;
                Vector3 current = getPlayerPos;
                if (Vector3.Distance(current, playerLastPos) > minDistans)
                {

                    playerLastPos = current;
                    return true;
                }
           
                return false;
            }
            else
            {
                return false;
            }
            
        }
    }
    private bool getPlayerExsist
    {
        get
        {
            if (player == null)
            {
                player = FindObjectOfType(typeof(MONO_PlayerMovement)) as MONO_PlayerMovement;
            }

            return player != null;
        }
    }
    private Vector3 getPlayerPos
    {
        get
        {
            if (player == null)
            {
                player = FindObjectOfType(typeof(MONO_PlayerMovement)) as MONO_PlayerMovement;
            }

            return player.gameObject.transform.position;
        }
    }



    void Awake()
    {
        CustomCursorTransform = CustomCursor.GetComponent<RectTransform>();
       

    }

    private void Start()
    {
        presistentCanvas = FindObjectOfType(typeof(Canvas)) as Canvas;
        CanvasRect       = presistentCanvas.GetComponent<RectTransform>();
    }


    void Update () {

        MoveVirtuelCursor();

    }

    private void MoveVirtuelCursor()
    {
        //Locks the cursor 
        if (Cursor.lockState != CursorLockMode.Locked && lockkurso)
        {
            Cursor.lockState = CursorLockMode.None; // herd this culd prevent editor bugg
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (MONO_Settings.instance.usingKeybodInput)
        {
 
            KeybordMovment();
        }
        else
        {
            MouseMovment();
        }
      
    }

    private void MouseMovment()
    {
        float x = MONO_Settings.instance.getMouseHorizontal;// Input.GetAxis("Mouse X");
        float y = MONO_Settings.instance.getMouyseVertical; //Input.GetAxis("Mouse Y");



        // Adds the momvent
        float xTemp = CustomCursorTransform.anchoredPosition.x + (x * CursorSpeed * Time.deltaTime);
        float yTemp = CustomCursorTransform.anchoredPosition.y + (y * CursorSpeed * Time.deltaTime);


        // Bounds code Vigfus
        float refHeight = 1080;// the referens hight
        float refWidth = ((float)Screen.width / (float)Screen.height) * refHeight;// the referens whidt that works with diffrent higts


        float minX = 0f;
        float minY = 0f + pointerSafeDist;
        float maxX = refWidth - pointerSafeDist;
        float maxY = refHeight;

        xTemp = Mathf.Clamp(xTemp, minX, maxX);
        yTemp = Mathf.Clamp(yTemp, minY, maxY);

        CustomCursorTransform.anchoredPosition = new Vector2(xTemp, yTemp);
    }

    private void KeybordMovment()
    {
        bool scroll           = MONO_Settings.instance.getToNextInteractable;
        bool moved            = getPlayerMoved;
        bool inventoryIsOppen = inventoryOppen;

        curretnKebordMod = (moved) ? kebordMovmentMod.CLOSESTTARGET : curretnKebordMod;
      
        if (scroll || curretnKebordMod == kebordMovmentMod.SCROLL || inventoryIsOppen)
        {
            getAllObjectsInScene();
            curretnKebordMod = kebordMovmentMod.SCROLL;
            /* controlls if the invntory was oppend, 
             * then select the first item slot.
             * else if the invntory was closed
             * slect the closest interactabl
             */
            if (!inventoryMod && inventoryIsOppen)
            {
                inventoryMod = true;
                selectedIndex = 0;
                setCurentItem();
          

            }
            else if (inventoryMod && !inventoryIsOppen)
            {
                inventoryMod = false;
                getClosestInteractable();
                return;
            }
     
            if (scroll)
            {
                getAllObjectsInScene();
                getNextIndex();
                setCurentItem();
            }
        }
        else if(moved)
        {
            curretnKebordMod = kebordMovmentMod.CLOSESTTARGET;
                getClosestInteractable();
        }
        else
        {
            // cep over rigt pos
            CustomCursorTransform.anchoredPosition = geUIpos(currentTransform.position);
        }
       
    }
  
    private void setCurentItem()
    {
        if (currentIndex != selectedIndex)
        {
            currentIndex = selectedIndex;
            if (inventoryOppen)
            {
                //now you can set the position of the ui element
                CustomCursorTransform.position = getCurretItem.gameObject.GetComponent<RectTransform>().position;
                CustomCursorTransform.position = new Vector3(CustomCursorTransform.position.x, CustomCursorTransform.position.y, 1f);
            }
            else
            {

                if(getCurretItem != null)
                {
                    currentTransform = getCurretItem.gameObject.transform;
                    CustomCursorTransform.anchoredPosition = geUIpos(currentTransform.position);

                }


               
            }


        }
    }

    private Vector2 geUIpos( Vector3 worldPos)
    {
        Vector2 ViewportPosition           = Camera.main.WorldToViewportPoint(worldPos);
        Vector2 WorldObject_ScreenPosition = new Vector2((ViewportPosition.x * CanvasRect.sizeDelta.x), (ViewportPosition.y * CanvasRect.sizeDelta.y));
        return WorldObject_ScreenPosition;
    }
  

    private void getAllObjectsInScene()
    {

        interacrablebaseObjectsInScene = null;
        inventorySLots                 = null;
        List<Selectable> tempinteractablesBase = new List<Selectable>();
        List<Selectable> tempInvnetorySlots     = new List<Selectable>();
        foreach (Selectable selectableUI in Selectable.allSelectables)
        {
            if (selectableUI.interactable)
            {
                if (selectableUI.gameObject.GetComponent<MONO_InteractionBase>() != null)
                {

                    if (selectableUI.gameObject.GetComponent<MONO_InventoryItemLogic>() == null)
                    {
                        Vector3 temptest = Camera.main.WorldToViewportPoint(selectableUI.gameObject.transform.position);
                        // not beutiful but it gets the jobb don.
                        if(temptest.x > 0 && temptest.x<1 && temptest.y > 0 && temptest.y < 1)
                        {
                            tempinteractablesBase.Add(selectableUI);
                        }
                             
                    }
                    else
                    {
                        tempInvnetorySlots.Add(selectableUI);
                    }

                }
            }

        }
        interacrablebaseObjectsInScene = tempinteractablesBase.ToArray();
        inventorySLots                 = tempInvnetorySlots.ToArray();

    }

    /// <summary>
    /// locates the closest interactable 
    /// to the player, probely not efficent
    /// ore especely buteful. but like mutch 
    /// her in the crunch, it wors thats all
    /// that counts.
    /// </summary>
    private void getClosestInteractable()
    {
        getAllObjectsInScene();

        Selectable bestTarget   = null;
        Vector2 pos              = new Vector2(0,0);
        Transform tempShosedTarget = null;
        int indexFinal = 0;
        closestInteractable     = null;
        float closestDistanceSqr = Mathf.Infinity;


        int i = 0;
        Vector2 currentPosition = geUIpos(getPlayerPos);
        foreach (Selectable selectableUI in interacrablebaseObjectsInScene)
        {
            i++;
            Vector2 tempPos              = geUIpos(selectableUI.gameObject.transform.position);
            Vector2 directionToTarget    = tempPos - currentPosition;
            float dSqrToTarget           = directionToTarget.sqrMagnitude;

            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget         = selectableUI;
                pos                = tempPos;
                indexFinal         = i;
                tempShosedTarget = selectableUI.gameObject.transform;
            }
        }
        currentTransform                       = tempShosedTarget;
        closestInteractable                    = bestTarget;
        CustomCursorTransform.anchoredPosition = pos;
        currentIndex                           = indexFinal;
        selectedIndex                          = indexFinal;
    }
   


    /// <summary>
    /// gets the nex index
    /// </summary>
    private void getNextIndex()
    {
        int length = (inventoryMod) ? inventorySLots.Length : interacrablebaseObjectsInScene.Length;
        length --;
        selectedIndex++;

        //esentyaly a modulus, don like this just becuss..
        selectedIndex = ((selectedIndex > length) || (selectedIndex < 0)) ? ((selectedIndex < 0) ? length : 0) : selectedIndex;
    }



    

}
