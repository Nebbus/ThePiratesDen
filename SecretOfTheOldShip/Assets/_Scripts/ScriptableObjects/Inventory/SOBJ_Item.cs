using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/*
* This simple script represents Items that can be picked
* up in the game.  The inventory system is done using
* this script instead of just sprites to ensure that items
* are extensible.
*/
[CreateAssetMenu(fileName = "newInventoryItem", menuName = "Inventory Item", order = 1)]
public class SOBJ_Item : ScriptableObject
{

    public Sprite sprite;
    public Fungus.Flowchart onHowerText;
    [FMODUnity.EventRef]
    public string putDownSound;

    public SOBJ_ItemInteractable[] onClickConditionAndReactions = new SOBJ_ItemInteractable[0];
    public SOBJ_ItemInteractable[] onHoverConditionAndReactions = new SOBJ_ItemInteractable[0];

    /// <summary>
    /// Initzilising all the reactions, is calld then 
    /// this item is added to the inventory
    /// </summary>
    public void InitReaction()
    {
        foreach (SOBJ_ItemInteractable interactables in onClickConditionAndReactions)
        {
            interactables.InitReactions();
        }
        foreach (SOBJ_ItemInteractable interactables in onHoverConditionAndReactions)
        {
            interactables.InitReactions();
        }
    }


    /// <summary>
    /// Run throug all the condition and runn appropriet 
    /// reactions click
    /// </summary>
    /// <param name="caller"> the monobehavor that caled this funktion</param>
    /// <returns> True if a interaction was done, otherwiseFalse</returns>
    public bool OnClickInteractionRun(MonoBehaviour caller)
    {
        /* Runs froug the itemInteractions, if one happens so will
         * the function be exited.
         */ 
        foreach (SOBJ_ItemInteractable interactables in onClickConditionAndReactions)
        {
            if (interactables.Interact(caller))
            {
                return true;
            }
         
        }
        return false;
    }

    /// <summary>
    /// Run throug all the condition and runn appropriet 
    /// reactions on hover over
    /// </summary>
    /// <param name="caller"> the monobehavor that caled this funktion</param>
    public void OnHoverInteractionRun(MonoBehaviour caller)
    {
        /* Runs froug the itemInteractions, if one happens so will
         * the function be exited.
         */
        foreach (SOBJ_ItemInteractable interactables in onHoverConditionAndReactions)
        {
            if (interactables.Interact(caller))
            {
                return;
            }
        }
    }


    /// <summary>
    /// Gets the hash of this item, used to 
    /// compare items
    /// </summary>
    public int getHash
    {
        get
        {
            return Animator.StringToHash(this.name);
        }
    }

}
