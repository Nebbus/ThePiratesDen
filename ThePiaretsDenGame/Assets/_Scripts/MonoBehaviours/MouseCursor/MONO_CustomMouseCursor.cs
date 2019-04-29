using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MONO_CustomMouseCursor : MonoBehaviour {

    [HideInInspector]
    public bool       UsingKeyboard = false;
    public GameObject CustomCursor;

    public Canvas canvas;
    public RectTransform fadeImage;


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
        //Cursor.lockState = CursorLockMode.Locked;
    }

    void Update () {
        MoveVirtuelCursor();

    }


    private void MoveVirtuelCursor()
    {
        if (Camera.main != null)
        {
            // get movent of key 
            float x = UsingKeyboard ? Input.GetAxis("Horizontal") : Input.mousePosition.x;// Input.GetAxis("Mouse X");
            float y = UsingKeyboard ? Input.GetAxis("Vertical")   : Input.mousePosition.y;// Input.GetAxis("Mouse Y");
                  x = UsingKeyboard ? CustomCursorTransform.anchoredPosition.x + (x * CursorSpeed * Time.deltaTime) : x;
                  y = UsingKeyboard ? CustomCursorTransform.anchoredPosition.y +(y * CursorSpeed * Time.deltaTime) : y;

            float xTemp = (x );
            float yTemp = (y );


            Vector3 pos   = new Vector3(xTemp, yTemp, 0f);
            Vector3 image = new Vector3(CustomCursorTransform.rect.width, CustomCursorTransform.rect.height, 0f);

            // pos = Camera.main.ScreenToViewportPoint(pos);
            //image = Camera.main.ScreenToViewportPoint(image);
            //pos.x = Mathf.Clamp(pos.x, 0f, 1f + image.x);
            //pos.y = Mathf.Clamp(pos.y, 0f+ image.y, 1f + image.y);
            

            float w = CustomCursorTransform.rect.width;
            float h = CustomCursorTransform.rect.height;
            float xx = 0f;// canvas.pixelRect.xMin;//fadeImage.anchorMin.x;// screenBounds.x + w;
            float XX = Screen.width; //canvas.pixelRect.xMax;// fadeImage.anchorMax.x;// screenBounds.x * -1 - w;
            float yy = 0f;// canvas.pixelRect.yMin;//fadeImage.anchorMin.y;// screenBounds.y + h;
            float YY = Screen.height; //canvas.pixelRect.yMax;//fadeImage.anchorMax.y;// screenBounds.y * -1 - h;
            Debug.DrawLine(new Vector3(xx, yy, 0f), new Vector3(XX, yy, 0f), Color.green);
            Debug.DrawLine(new Vector3(XX, yy, 0f), new Vector3(XX, YY, 0f), Color.red);
            Debug.DrawLine(new Vector3(XX, YY, 0f), new Vector3(xx, YY, 0f), Color.blue);
            Debug.DrawLine(new Vector3(xx, YY, 0f), new Vector3(xx, yy, 0f), Color.magenta);

            pos.x = Mathf.Clamp(pos.x, xx, XX+ image.x);
            pos.y = Mathf.Clamp(pos.y, yy + image.y, YY+ image.y);
            //pos = Camera.main.ViewportToScreenPoint(pos);
            CustomCursorTransform.anchoredPosition = new Vector2(pos.x, pos.y);
 
        }

    }

}
