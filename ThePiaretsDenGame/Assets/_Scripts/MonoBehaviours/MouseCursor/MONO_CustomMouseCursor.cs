using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MONO_CustomMouseCursor : MonoBehaviour {

    [HideInInspector]
    public bool       UsingKeyboard = false;
    public GameObject CustomCursor;

    public Canvas canvas;

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
        if (Camera.main != null)
        {
            // get movent of key 
            float x = UsingKeyboard ? Input.GetAxis("Horizontal") : Input.GetAxis("Mouse X");
            float y = UsingKeyboard ? Input.GetAxis("Vertical")   : Input.GetAxis("Mouse Y");

            float xTemp = CustomCursorTransform.anchoredPosition.x + (x * CursorSpeed * Time.deltaTime);
            float yTemp = CustomCursorTransform.anchoredPosition.y + (y * CursorSpeed * Time.deltaTime);


            Vector3 pos = new Vector3(xTemp, yTemp, 0f);
        
            pos = Camera.main.ScreenToViewportPoint(pos);
            pos.x = Mathf.Clamp(pos.x, 0f, 1f);
            pos.y = Mathf.Clamp(pos.y, 0f, 1f);

            pos = Camera.main.ViewportToScreenPoint(pos);
            CustomCursorTransform.anchoredPosition = new Vector2(pos.x, pos.y);

        }

    }

    private void OnDrawGizmos()
    {
        if(Camera.main != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(new Vector3(0.0f, 0.0f, 0.0f), Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.0f, 0.0f)));
            Gizmos.DrawLine(new Vector3(0.0f, 0.0f, 0.0f), Camera.main.ViewportToScreenPoint(new Vector3(0.0f, 0.5f, 0.0f)));
            Gizmos.DrawLine(new Vector3(0.0f, 0.0f, 0.0f), Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.0f, 0.0f)));
            Gizmos.DrawLine(new Vector3(0.0f, 0.0f, 0.0f), Camera.main.ViewportToScreenPoint(new Vector3(0.0f, 0.5f, 0.0f)));
        }

    }

}
