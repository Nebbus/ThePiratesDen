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
    private MONO_Menus      Menus;

    //===========================================================================
    // Kebord accesebility stuff
    //===========================================================================

    public Selectable[] interacrablebaseObjectsInScene = new Selectable[1];

    public Selectable[] inventorySLots = new Selectable[1];

    private int selectedItem = 0;
    private bool inventoryMod = false;


    private Selectable selected;

    public Selectable curretItem
    {
        get
        {
            return (inventorySLots != null) ? inventorySLots[selectedItem] : interacrablebaseObjectsInScene[selectedItem];
        }

    }




    void Awake()
    {
        CustomCursorTransform = CustomCursor.GetComponent<RectTransform>();
        Menus                 = FindObjectOfType<MONO_Menus>();

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

        if (MONO_Settings.instance.getInventoryButton)
        {
            getAllObjectsInScene();
        }

        if (MONO_Settings.instance.getToNextInteractable)
        {
            getAllObjectsInScene();
            getNextIndex();
            if(selected != curretItem)
            {
                CustomCursorTransform.anchoredPosition = Camera.current.WorldToScreenPoint(curretItem.gameObject.transform.position);
                selected = curretItem;
            }

        }
    }




    private void getAllObjectsInScene()
    {
        interacrablebaseObjectsInScene = null;
        inventorySLots = null;
        List<Selectable> tempinteractablesBase = new List<Selectable>();
        List<Selectable> tempInvnetorySlots = new List<Selectable>();
        foreach (Selectable selectableUI in Selectable.allSelectables)
        {
            if (selectableUI.gameObject.GetComponent<MONO_InteractionBase>() != null)
            {

                if (selectableUI.gameObject.GetComponent<MONO_InventoryItemLogic>() != null)
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
        inventorySLots = tempInvnetorySlots.ToArray();
    }

   
    /// <summary>
    /// gets the nex index
    /// </summary>
    private void getNextIndex()
    {
        int length = (inventorySLots != null) ? inventorySLots.Length : interacrablebaseObjectsInScene.Length;

        if (inventorySLots != null && !inventoryMod)
        {
            selectedItem = 0;
            inventoryMod = true;
        }
        else
        {
            inventoryMod = false;
            //esentyaly a clamp, don like this just becuss..
            selectedItem = ((selectedItem > length) || (selectedItem < 0)) ? ((selectedItem < 0) ? 0 : length) : selectedItem;
        }

        selectedItem += 1;
        //esentyaly a modulus, don like this just becuss..
        selectedItem = ((selectedItem > length) || (selectedItem < 0)) ? ((selectedItem < 0) ? length : 0) : selectedItem;
    }



    

}
