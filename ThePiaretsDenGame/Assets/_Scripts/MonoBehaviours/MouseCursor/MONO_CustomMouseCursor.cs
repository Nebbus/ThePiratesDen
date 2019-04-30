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
    [SerializeField]
    [Tooltip("Pixels distans the muse points out on the right side and on the bottom")]
    private float pointerSafeDist = 5;

    private RectTransform   CustomCursorTransform;
    private MONO_Menus      Menus;
    public bool useMosePosition = true;

  
    

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
            if (Cursor.lockState != CursorLockMode.Locked)
            {
            Cursor.lockState = CursorLockMode.None; // herd this culd prevent editor bugg
            Cursor.lockState = CursorLockMode.Locked;
            }

            // Gets the movment direction
            float x = UsingKeyboard ? Input.GetAxis("Horizontal") : Input.GetAxis("Mouse X");
            float y = UsingKeyboard ? Input.GetAxis("Vertical")   : Input.GetAxis("Mouse Y");


            // Adds the momvent
            float xTemp = CustomCursorTransform.anchoredPosition.x + (x * CursorSpeed * Time.deltaTime);
            float yTemp = CustomCursorTransform.anchoredPosition.y + (y * CursorSpeed * Time.deltaTime);


            // Bounds code Vigfus
            float refHeight = 1080;// the referens hight
            float refWidth  = ((float)Screen.width / (float)Screen.height) * refHeight;// the referens whidt that works with diffrent higts


            float minX = 0f;
            float minY = 0f       + pointerSafeDist;
            float maxX = refWidth - pointerSafeDist; 
            float maxY = refHeight;

            xTemp = Mathf.Clamp(xTemp, minX, maxX );
            yTemp = Mathf.Clamp(yTemp, minY , maxY);

            CustomCursorTransform.anchoredPosition = new Vector2(xTemp, yTemp);

    }

}
