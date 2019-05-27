using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;

public class MONO_Inventory : MonoBehaviour {


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
    private GraphicRaycaster m_Raycaster;
    private EventSystem      m_EventSystem;

    private WaitForSeconds   wait; // Storing the wait created from the delay so it doesn't need to be created each time.
    public float waitDelay;
    private bool timerStarted = false;
    private Coroutine curentTimer;

    private bool startdFlashing = false;

    private bool HandleInput = true;


    private void Start()
    {
        inventoryDetectionImage = GetComponent<Image>();
        m_Raycaster             = FindObjectOfType<GraphicRaycaster>();
        m_EventSystem           = FindObjectOfType<EventSystem>();
        wait                    = new WaitForSeconds(waitDelay);
		HideInventory ();
        higligtImage = buttonHiglight.higlightEffect.itemImageObject.GetComponent<Image>();
    }

    /// <summary>
    /// Handel clicks on the inventory
    /// </summary>
    public void HandleInventoryClick()
    {
        if (HandleInput)
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
        StopPickUpReaction();
        inventoryGroup.SetActive(true);
        inventoryImage.SetActive(true);
        inventoryDetectionImage.enabled = true;
    }

    public void HideInventory()
    {
        inventoryGroup.SetActive(false);
        inventoryImage.SetActive(false);
        inventoryDetectionImage.enabled = false;
    }

 
    /// <summary>
    /// Sets if the inventory should handel input or not
    /// </summary>
    /// <param name="setTo"> the value the HandleInpu variabler 
    ///  is going to be set to</param>
    public void SetHandleINput( bool setTo)
    {
        HandleInput = setTo;
    }

    /// <summary>
    /// Adds item to the inventory
    /// </summary>
    /// <param name="itemToAdd"> item to add</param>
    public void AddItem(SOBJ_Item itemToAdd)
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
                return;
             }

         }
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
    /// Removes item from inventory
    /// </summary>
    /// <param name="itemToRemove">item to be removed</param>
    public void RemoveItem(int itemToRemoveIndex)
    {
        if (itemToRemoveIndex < invetoryItems.Length && itemToRemoveIndex > (-1))
        {
            invetoryItems[itemToRemoveIndex]               = null;
            invetoryItemsImages[itemToRemoveIndex].sprite  = null;
            invetoryItemsImages[itemToRemoveIndex].enabled = false;
            inventorySlots[itemToRemoveIndex].GetComponent<MONO_InventoryItemLogic>().getSetItemsHashCode = -1;
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
           // Debug.Log((invetoryItems[i] != null && invetoryItems[i].getHash == hash));
            if (invetoryItems[i]!=null && invetoryItems[i].getHash == hash)
            {
                itemIndex = i;
                break;
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
    /// gets a item form the inventory
    /// </summary>
    /// <param name="hash"> the hash of the itenm</param>
    /// <returns> returns null if the item isent in the inventory</returns>
    public SOBJ_Item GetItem(int hash)
    {
       
        int itemIndex = -1;

        //gets the right index for the item
        for (int i = 0; i < invetoryItems.Length; i++)
        {
            if (invetoryItems[i].getHash == hash)
            {
                itemIndex = i;
                break;
            }
        }

        if (itemIndex == -1)
        {
            Debug.LogError("Tryed to grab item that isent in the inventory");
            return null;
        }

        return invetoryItems[itemIndex];


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
        //if (!startdFlashing)
        //{
        //    startdFlashing = true;
        //    //  this.GetComponent<MONO_HiglightObject>().startFlashing();
       
        //}
        buttonHiglight.startFlashing();

    }

    /// <summary>
    /// to sto the flashing then teh inventory is opend
    /// </summary>
    private void StopPickUpReaction()
    {
 
        buttonHiglight.StopFlashing();

    }



    private void Update()
    {
        if (MONO_AdventureCursor.instance.getMonoHoldedItem.isHoldingItem)
        {
            if (wait != null)
                if (!raycast())
                {
                    if (!timerStarted)
                    {
                        timerStarted = true;
                        curentTimer = StartCoroutine(ReactCoroutine());
                    }

                }
                else if (timerStarted)
                {
                    StopCoroutine(curentTimer);
                    timerStarted = false;
                }
        }
       
    }

    private bool raycast()
    {
        List<RaycastResult> results = EXT_RayCast.GraphicRayCast(m_Raycaster, m_EventSystem, Input.mousePosition);

        //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.name == this.gameObject.name)
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
