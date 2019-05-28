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

    //===========================================================================
    // Kebord accesebility stuff
    //===========================================================================
    [Space]
    public int selectedIndex = 0;
    public int currentIndex = -1;
    [Space]
    public Selectable[] interacrablebaseObjectsInScene = new Selectable[1];

    public Selectable[] inventorySLots = null;



    public bool inventoryMod = false;

    public bool inventoryOppen
    {
        get
        {
            inventoryMod = (inventorySLots.Length != 0);
            return inventoryMod;
        }
    }

    public GameObject selecetGamobject;
    public Selectable getCurretItem
    {
        get
        {
            Selectable temp = (inventoryOppen) ? inventorySLots[currentIndex] : interacrablebaseObjectsInScene[currentIndex];
            selecetGamobject = temp.gameObject;
            return temp;
        }

    }




    void Awake()
    {
        CustomCursorTransform = CustomCursor.GetComponent<RectTransform>();
        presistentCanvas = FindObjectOfType(typeof(Canvas)) as Canvas;

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


        if (MONO_Settings.instance.getToNextInteractable)
        {
            getAllObjectsInScene();
            getNextIndex();
            setCurentItem();

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
                RectTransform CanvasRect = presistentCanvas.GetComponent<RectTransform>();

                Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(getCurretItem.gameObject.transform.position);
                Vector2 WorldObject_ScreenPosition = new Vector2((ViewportPosition.x * CanvasRect.sizeDelta.x), (ViewportPosition.y * CanvasRect.sizeDelta.y));

                //now you can set the position of the ui element
                CustomCursorTransform.anchoredPosition = WorldObject_ScreenPosition;
            }


        }
    }
    public Vector2 temp0;
    public Vector3 temp1;
    public Vector3 temp2;
    public Vector3 temp3;
    public Vector3 temp4;
    private void getAllObjectsInScene()
    {
        interacrablebaseObjectsInScene = null;
        inventorySLots                 = null;
        List<Selectable> tempinteractablesBase = new List<Selectable>();
        List<Selectable> tempInvnetorySlots     = new List<Selectable>();
        foreach (Selectable selectableUI in Selectable.allSelectables)
        {
            if (selectableUI.gameObject.GetComponent<MONO_InteractionBase>() != null)
            {
                
                if (selectableUI.gameObject.GetComponent<MONO_InventoryItemLogic>() == null)
                {

                    tempinteractablesBase.Add(selectableUI);
                }
                else
                {
                    tempInvnetorySlots.Add(selectableUI);
                }

            }

        }
        interacrablebaseObjectsInScene = tempinteractablesBase.ToArray();
        inventorySLots                 = tempInvnetorySlots.ToArray();
    }

   
    /// <summary>
    /// gets the nex index
    /// </summary>
    private void getNextIndex()
    {
        int length = (inventoryOppen) ? inventorySLots.Length : interacrablebaseObjectsInScene.Length;
        length --;

        // selectedIndex = ((selectedIndex > length) || (selectedIndex < 0)) ? ((selectedIndex < 0) ? 0 : length) : selectedIndex;


        selectedIndex ++;
        //esentyaly a modulus, don like this just becuss..
        selectedIndex = ((selectedIndex > length) || (selectedIndex < 0)) ? ((selectedIndex < 0) ? length : 0) : selectedIndex;
    }



    

}
