using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOBJ_ItemInteractable : ScriptableObject
{
    // Description of the ConditionCollection.  This is used purely for identification in the inspector.
    public string description;                                 
    public SOBJ_ConditionAdvanced[] requiredConditions       = new SOBJ_ConditionAdvanced[0];   // Array of all the Conditions
    public SOBJ_Reaction[]          itemInteractionReactions = new SOBJ_Reaction[0];            // Array of all the Reactions to play when React is called.

    /// <summary>
    /// Funktion thet inizilising all the reactions
    /// </summary>
    public void InitReactions()
    {
        // Go through all the Reactions and call their Init function.
        for (int i = 0; i < itemInteractionReactions.Length; i++)
        {
            /*The DelayedReaction 'hides' the Reaction's Init function with it's own.
             * This means that we have to try to cast the Reaction to a DelayedReaction
             * and then if it exists call it's Init function. Note that this mainly
             * done to demonstrate hiding and not especially for functionality.
             */
            SOBJ_DelayedReaction delayedReaction = itemInteractionReactions[i] as SOBJ_DelayedReaction;

            if (delayedReaction)
            {
                delayedReaction.Init();
            }
            else
            {
                itemInteractionReactions[i].Init();
            }

        }
    }

    // This is called when the player arrives at the interactionLocation.
    /// <summary>
    /// Starts a interactions
    /// </summary>
    /// <param name="caller">the monovehavor cald for the interaction</param>
    public bool Interact(MonoBehaviour caller)
    {
       return CheckAndReact(caller);

    }

    /// <summary>
    /// This is called by the Interactable one at a time for 
    /// each of its ConditionCollections until one returns true.
    /// </summary>
    /// <returns></returns>
    private bool CheckAndReact(MonoBehaviour caller)
    {
        // Go through all Conditions...
        for (int i = 0; i < requiredConditions.Length; i++)
        {
            /* ... and check them against the AllConditions version of the Condition. 
             * If they don't have the same satisfied flag, return false.
             */
            if (!SOBJ_AllConditions.CheckCondition(requiredConditions[i]))
            {
                return false;
            }

        }
        React(caller);

        // A Reaction happened so return true.
        return true;
    }


    /// <summary>
    /// Runs throug and all the reactions
    /// </summary>
    /// <param name="caller">the monovehavor cald for the interaction</param>
    private void React(MonoBehaviour caller)
    {
        // Go through all the Reactions and call their React function.
        for (int i = 0; i < itemInteractionReactions.Length; i++)
        {
            // The DelayedReaction hides the Reaction.React function.
            // Note again this is mainly done for demonstration purposes.
            SOBJ_DelayedReaction delayedReaction = itemInteractionReactions[i] as SOBJ_DelayedReaction;

            if (delayedReaction)
            {
                delayedReaction.React(caller);
            }
            else
            {
                itemInteractionReactions[i].React(caller);
            }

        }
    }

}
