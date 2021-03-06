﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class MONO_Inventory : MonoBehaviour {


    public Fungus.SayDialog descritpionBox;

    public Image higligtImage;


    [Tooltip("The object that gas the Grid Layout Group on it, it will have all items as children")]
    public GameObject inventoryGroup;

    public MONO_Inventory_buttonLigthUpp buttonHiglight;

    [SerializeField]
    private GameObject inventoryImage;
    private Image inventoryDetectionImage; // used to sence then the mous is over the open inventory ( is trancparent)

    public Image[]      invetoryItemsImages = new Image[numberItemSlots];
    public SOBJ_Item[]  invetoryItems       = new SOBJ_Item[numberItemSlots];
    public GameObject[] inventorySlots      = new GameObject[numberItemSlots];

    public static int numberItemSlots = 0;

    // for handeling the closing of the invenory after item is draged out
    public GraphicRaycaster m_Raycaster;
    public EventSystem      m_EventSystem;

    public WaitForSeconds   wait; // Storing the wait created from the delay so it doesn't need to be created each time.
    public float waitDelay;
    public bool timerStarted = false;
    private Coroutine curentTimer;

    private bool startdFlashing = false;

    public bool localHandleInput = false;

    public Action<MONO_EventManager.EventParam> setLocalInvntoryHandelInput;
    public Action<MONO_EventManager.EventParam> setVisibilityOfInvnetory;




    private void Awake()
    {
        setVisibilityOfInvnetory = new Action<MONO_EventManager.EventParam>(SetInvnetoryVisability);
        setLocalInvntoryHandelInput = new Action<MONO_EventManager.EventParam>(SetHandleINput);
    }

    private void Start()
    {
        inventoryDetectionImage = GetComponent<Image>();
        m_Raycaster             = FindObjectOfType<GraphicRaycaster>();
        m_EventSystem           = FindObjectOfType<EventSystem>();
        wait                    = new WaitForSeconds(waitDelay);
		HideInventory ();
        higligtImage = buttonHiglight.lobingItemEffectStruct.lobedItem.GetComponent<Image>();
    }

    private void OnEnable()
    {
        MONO_EventManager.StartListening(MONO_EventManager.setLocalInvntoryHandelInput_NAME, setLocalInvntoryHandelInput);
        MONO_EventManager.StartListening(MONO_EventManager.setVisibilityOfInvnetory_NAME, setVisibilityOfInvnetory);
    }
    private void OnDisable()
    {
        MONO_EventManager.StopListening(MONO_EventManager.setLocalInvntoryHandelInput_NAME, setLocalInvntoryHandelInput);
        MONO_EventManager.StopListening(MONO_EventManager.setVisibilityOfInvnetory_NAME, setVisibilityOfInvnetory);

    }
    private void SetInvnetoryVisability(MONO_EventManager.EventParam param)
    {
        if (param.param4)
        {
            ShowInventory();
        }
        else
        {
            HideInventory();
        }
    }
    private void SetHandleINput(MONO_EventManager.EventParam param)
    {
        localHandleInput = param.param4;
    }




    private void Update()
    {
        if (MONO_Settings.instance.getInventoryButton )
        {
            HandleInventoryClick();
        }

        if (MONO_AdventureCursor.instance.getMonoHoldedItem.isHoldingItem && inventoryGroup.activeSelf)
        {
            if (wait != null)
            {
                if (!raycast() && !timerStarted)
                {
                    timerStarted = true;
                    curentTimer = StartCoroutine(ReactCoroutine());
                  

                }
                else if (raycast() && timerStarted)
                {
                    StopCoroutine(curentTimer);
                    timerStarted = false;
                }
            }
        }
        else if (timerStarted)
        {
            timerStarted = false;
        }

    }


    /// <summary>
    /// Handel clicks on the inventory
    /// </summary>
    public void HandleInventoryClick()
    {
        if (localHandleInput)
        {
            if (inventoryGroup.activeSelf)
            {
                HideInventory();
            }
            else
            {
                ShowInventory();
            }
        }

    }

    public void ShowInventory()
    {
      
       // StopPickUpReaction();
        inventoryGroup.SetActive(true);
        inventoryImage.SetActive(true);
        inventoryDetectionImage.enabled = true;
        timerStarted = false;
    }

    public void HideInventory()
    {
        inventoryGroup.SetActive(false);
        inventoryImage.SetActive(false);
        inventoryDetectionImage.enabled = false;
        timerStarted = false;
    }



    /// <summary>
    /// Sets if the inventory should handel input or not
    /// </summary>
    /// <param name="setTo"> the value the HandleInpu variabler 
    ///  is going to be set to</param>
    public void SetHandleINput(bool setTo)
    {
        localHandleInput = setTo;
    }

    /// <summary>
    /// Adds item to the inventory, returns true if the item was added, returns fals if no space was found.
    /// </summary>
    /// <param name="itemToAdd"> item to add</param>
    public bool AddItem(SOBJ_Item itemToAdd)
     {
         for (int i = 0; i < invetoryItems.Length; i++)
         {
             if (invetoryItems[i] == null)
             {
                itemToAdd.InitReaction();// ini all reactions
                invetoryItems[i]               = itemToAdd;
                invetoryItemsImages[i].sprite  = itemToAdd.sprite;

                higligtImage.sprite = itemToAdd.sprite;

                invetoryItemsImages[i].enabled = true;
                inventorySlots[i].GetComponent<MONO_InventoryItemLogic>().getSetItemsHashCode = itemToAdd.getHash;
                PickUpReaction();
                return true;
             }

         }
        return false;
        Debug.LogError("The inventory is overflowing!!!!!");
     }
    /// <summary>
    /// Removes item from inventory
    /// </summary>
    /// <param name="itemToRemove">item to be removed</param>
     public void RemoveItem(SOBJ_Item itemToRemove)
     {
         for (int i = 0; i < invetoryItems.Length; i++)
         {
             if (invetoryItems[i] == itemToRemove)
             {
                invetoryItems[i]                = null;
                invetoryItemsImages[i].sprite   = null;
                invetoryItemsImages[i].enabled  = false;
                inventorySlots[i].GetComponent<MONO_InventoryItemLogic>().getSetItemsHashCode = -1;
                 return;
             }

         }
     }


    /// <summary>
    /// t prevent duplicating of items
    /// </summary>
    public void ClerInventory()
    {
        for (int i = 0; i < invetoryItems.Length; i++)
        {
                invetoryItems[i] = null;
                invetoryItemsImages[i].sprite = null;
                invetoryItemsImages[i].enabled = false;
                inventorySlots[i].GetComponent<MONO_InventoryItemLogic>().getSetItemsHashCode = -1;
        }
    }

    /// <summary>
    /// Set item hold by the mouspinter
    /// </summary>
    /// <param name="itemIndex">index of item to grabe</param>
    public void GrabItem(int hash)
    {

        int itemIndex = -1;

        //gets the right index for the item
        for (int i = 0; i < invetoryItems.Length; i++)
        {
            // crude but works
            if (invetoryItems[i] != null)
            {
                if (invetoryItems[i].getHash == hash)
                {
                    itemIndex = i;
                    break;
                }
            }
        }

        if(itemIndex == -1)
        {
            Debug.LogError("Tryed to grab item that isent in the inventory");
            return;
        }

        if (invetoryItemsImages[itemIndex].sprite != null && itemIndex < invetoryItems.Length && itemIndex > (-1))
        {
            MONO_AdventureCursor.instance.getMonoHoldedItem.GrabbedItem(invetoryItems[itemIndex], invetoryItemsImages[itemIndex].sprite, itemIndex);
            invetoryItemsImages[itemIndex].enabled = false;
        }
    }

    /// <summary>
    /// to get this item from the inventory logic script,
    /// </summary>
    /// <param name="inventoryIndex"> index of the item to be recived</param>
    /// <param name="caller"> ust for debug to se from ther the falty call thas maide</param>
    /// <returns> a item, if null, then a error has ocured</returns>
    public SOBJ_Item GetSOBJitemFromInventory(int inventoryIndex, GameObject caller)
    {
        if (inventoryIndex < invetoryItems.Length && inventoryIndex > (-1))
        {
            return invetoryItems[inventoryIndex];
        }
        Debug.LogError("A item logic has tryd to get a time than it dosent has a item");
        return null;

    }

    /// <summary>
    /// Activates the items that was holded by the mouse pointer,
    /// allso nulls the item on the mouse
    /// </summary>
    /// <param name="itemIndex"></param>
    public void ReturnToInventory(int itemIndex)
    {
        if (itemIndex < invetoryItems.Length && itemIndex > (-1))
        {
            invetoryItemsImages[itemIndex].enabled = true;
        }
        MONO_AdventureCursor.instance.getMonoHoldedItem.ReturnItemToInventory();
    }


    /// <summary>
    /// starts the picked upp indication
    /// </summary>
    private void PickUpReaction()
    {
        HideInventory();
        buttonHiglight.startHigligtReaction();

    }

    
    private bool raycast()
    {
        RectTransform rectransform = MONO_AdventureCursor.instance.gameObject.GetComponent<RectTransform>();
        List<RaycastResult> results = EXT_RayCast.GraphicRayCast(m_Raycaster, m_EventSystem, rectransform.position);
        //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
        foreach (RaycastResult result in results)
        {

            if (result.gameObject.name == gameObject.name)
            {
                return true;
            }

        }
        return false;
    }

    private IEnumerator ReactCoroutine()
    {
        // Wait for the specified time.
        yield return wait;

        HideInventory();
    }





}
