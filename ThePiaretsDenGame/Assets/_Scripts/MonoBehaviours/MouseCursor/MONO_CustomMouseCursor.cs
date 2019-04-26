using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MONO_CustomMouseCursor : MonoBehaviour {

    [HideInInspector]
    public bool       UsingKeyboard = false;
    public GameObject CustomCursor;
    

    [SerializeField]
    private  float  CursorSpeed = 4;
    private RectTransform   CustomCursorTransform;
    private MONO_Menus      Menus;
   

    void Awake()
    {
        CustomCursorTransform = CustomCursor.GetComponent<RectTransform>();
        Menus                = FindObjectOfType<MONO_Menus>();

    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update () {
        MoveVirtuelCursor();

    }

    private void MoveVirtuelCursor()
    {

        // get movent of key 
        float x = UsingKeyboard ? Input.GetAxis("Horizontal") : Input.GetAxis("Mouse X");
        float y = UsingKeyboard ? Input.GetAxis("Vertical")   : Input.GetAxis("Mouse Y") ;
     
        float xTemp                            = CustomCursorTransform.anchoredPosition.x + (x * CursorSpeed*Time.deltaTime);
        float yTemp                            = CustomCursorTransform.anchoredPosition.y + (y * CursorSpeed * Time.deltaTime);

        //Vector3 pos = Camera.main.WorldToViewportPoint(new Vector3(xTemp, yTemp,0));
        //bool stopVertical = (1.0 > pos.y && pos.y > 0.0);
        //bool stopHorizontal = (1.0 > pos.x && pos.x > 0.0);

        //Camera.main.ed

        //yTemp = stopVertical ? CustomCursorTransform.anchoredPosition.y : 0;
        //xTemp = stopHorizontal ? CustomCursorTransform.anchoredPosition.x : 0;

        CustomCursorTransform.anchoredPosition = new Vector2(xTemp, CustomCursorTransform.anchoredPosition.y);
        CustomCursorTransform.anchoredPosition = new Vector2(CustomCursorTransform.anchoredPosition.x, yTemp);
    }
    
}
