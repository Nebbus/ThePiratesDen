﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MONO_InventoryItemLogic : MONO_InteractionBase
{
    public int hashCode = -1;

    public Fungus.SayDialog lastBox;
    [SerializeField]
    private int index;
    private bool hasBenGrabed = false;
    private MONO_Inventory monoInventory;
    private Fungus.Flowchart FlowhartToShow = null;
    private int realHashCode = -1;

   


    public int getSetItemsHashCode
    {
        get
        {
            return realHashCode;
        }
        set
        {
            realHashCode = value;
            hashCode = realHashCode;
        }

    }


    public MONO_Inventory setMonoInventory
    {
        set
        {
            monoInventory = value;
        }

    }
    /// <summary>
    /// Sets the index of this itemSlot, 
    /// is cald then the slot is created.
    /// </summary>
    public int getStetIndex
    {
        set
        {
            index = value;
        }
        get
        {
            return index;
        }
    }


    private void Start()
    {
        if (gameObject.GetComponent(typeof(UnityEngine.UI.Selectable)) == null)
        {
            gameObject.AddComponent(typeof(UnityEngine.UI.Selectable));
        }
        if (monoInventory == null)
        {
            monoInventory = FindObjectOfType<MONO_Inventory>();

        }

   
    }

    /// <summary>
    /// Run throug the ractions the item has attached to it.
    /// </summary>
    private void ClickReact()
    {
        /*prevents the slot from caling
        * reactions then no item
        * is precent
        */
        bool doNotreturnItem = true;
        if (getSetItemsHashCode != -1)
        {

            //doNotreturnItem = !monoInventory.GetItem(getSetItemsHashCode).OnClickInteractionRun(this);
            doNotreturnItem = !monoInventory.GetSOBJitemFromInventory(getStetIndex, this.gameObject).OnClickInteractionRun(this);
        }
        // if no reaction was don ore no item is held, return to inventory
        if (doNotreturnItem)
        {
            FMODUnity.RuntimeManager.PlayOneShot(MONO_AdventureCursor.instance.getMonoHoldedItem.currentItem.putDownSound);
            monoInventory.ReturnToInventory(MONO_AdventureCursor.instance.getMonoHoldedItem.getSetIndex);
        }

    }

    /// <summary>
    /// Creates and starts a flowshart whit the description
    /// </summary>
    private void HovorEnterdReact()
    {
        /* handel input is active for inventorys, 
         * and that this thing has a item in it
         */ 
        if (monoInventory.localHandleInput && getSetItemsHashCode != -1)
        {
            if (getSetItemsHashCode != MONO_AdventureCursor.instance.getMonoHoldedItem.currentItem.getHash)
            {

                lastBox = Fungus.SayDialog.ActiveSayDialog;
                Fungus.SayDialog.ActiveSayDialog = monoInventory.descritpionBox;
                FlowhartToShow = GameObject.Instantiate(monoInventory.GetSOBJitemFromInventory(getStetIndex, this.gameObject).onHowerText);
                FlowhartToShow.ExecuteBlock("Description");


                //==================================================================================
                // Hack of the scenturey, to prevent on hover over item to turn of 
                // the input handeling p.g.a a flowchart starts, set item handeling to true
                // after the flowchart has started.
                //==================================================================================
                MONO_EventManager.EventParam param = new MONO_EventManager.EventParam();
                param.param4 = true;
                MONO_EventManager.TriggerEvent(MONO_EventManager.setInputHandling_NAME, param);
                MONO_EventManager.TriggerEvent(MONO_EventManager.setVisibilityOfInvnetory_NAME, param);


            }
        }
    }

    /// <summary>
    /// not used for the moment 
    /// </summary>
    private void HovorReact()
    {
      
        if (getSetItemsHashCode != -1)
        {

            // monoInventory.GetItem(getSetItemsHashCode).OnHoverInteractionRun(this);
            
            monoInventory.GetSOBJitemFromInventory(getStetIndex, this.gameObject).OnHoverInteractionRun(this);

        }
    }

 
    /// <summary>
    /// if the description is activated, stop it and destroy it
    /// </summary>
    private void HovorExitReact()
    {
        if (FlowhartToShow != null)
        {

            FlowhartToShow.StopAllBlocks();
            Destroy(FlowhartToShow.gameObject);// maybe is exist better solution, but worsk for now
            Fungus.SayDialog.ActiveSayDialog = lastBox;
        }
    }

    public override void OnClick()
    {
        HovorExitReact();
        ClickReact();
    }

    public override void OnHoverEnterd()
    {

        HovorEnterdReact();

    }

    public override void OnHover()
    {
        //not used for the moment
        // HovorReact()
    }

    public override void OnHoverExit()
    {
        HovorExitReact();
    }
}
