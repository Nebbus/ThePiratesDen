using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MONO_CustomMouseCursor : MonoBehaviour {

    [HideInInspector]
    public bool UsingKeyboard = false;
    public GameObject CustomCursor;
    

    [SerializeField]
    private readonly float CursorSpeed = 4;
    private RectTransform CustomCursorTransform;
    private MONO_Menus Menus;
   

    void Awake()
    {
        CustomCursorTransform = CustomCursor.GetComponent<RectTransform>();
        Menus = FindObjectOfType<MONO_Menus>();
    }


	void Update () {

        if (UsingKeyboard)
        {
            if (!Menus.menuOpen)
            {
                MoveWithKeys();
            }
        }
        else
        {
            MoveWithCursor();
        }
    }


    private void MoveWithKeys()
    {
        float xTemp = CustomCursorTransform.anchoredPosition.x + (Input.GetAxis("Horizontal") * CursorSpeed);
        float yTemp = CustomCursorTransform.anchoredPosition.y + (Input.GetAxis("Vertical") * CursorSpeed);
        CustomCursorTransform.anchoredPosition = new Vector2(xTemp, CustomCursorTransform.anchoredPosition.y);
        CustomCursorTransform.anchoredPosition = new Vector2(CustomCursorTransform.anchoredPosition.x, yTemp);
    }

    private void ClickWithKeys()
    {
        Input.GetKeyDown(KeyCode.Space); //ask designer which button
        //do the clicking
    }

    private void MoveWithCursor()
    {
        CustomCursorTransform.anchoredPosition = Input.mousePosition;
    }

    private void ClickWithMouse()
    {
        Input.GetMouseButtonDown(0); //primary mouse button
        //do the clicking
    }
}
