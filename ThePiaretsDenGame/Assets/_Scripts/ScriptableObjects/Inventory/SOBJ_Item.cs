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

[CreateAssetMenu]
public class SOBJ_Item : ScriptableObject
{
    public Sprite sprite;
    public SOBJ_Item combindsWith;
    public SOBJ_Item toMake;

    



    // combinde, investigate

    /// <summary>
    /// is planded to contain reactions and the conditons that accomplish them
    /// </summary>
   [Serializable]
    public struct reactionAndCondition
    {
        // Description of the ConditionCollection.  This is used purely for identification in the inspector.
        public string description;              

        public SOBJ_ConditionAdvanced[] conditions;
        public SOBJ_Reaction[]          reactions;

        /// <summary>
        /// Initilices all the ractions in the racticons list
        /// </summary>
        public void Init()
        {
            foreach(SOBJ_Reaction reaction in reactions)
            {
                SOBJ_DelayedReaction delayedReaction = reaction as SOBJ_DelayedReaction;
                if (delayedReaction)
                {
                    Debug.Log(delayedReaction.name);
                    delayedReaction.Init();
                }
                else
                {
                    reaction.Init();
                }
            }
        }

        public bool KontrolAndReact(MonoBehaviour caller)
        {
            for (int i = 0; i < conditions.Length; i++)
            {
                if (!SOBJ_AllConditions.CheckCondition(conditions[i]))
                {
                    return false;
                }

            }


            foreach(SOBJ_Reaction reaction in reactions)
            {
                SOBJ_DelayedReaction delayedReaction = reaction as SOBJ_DelayedReaction;

                if (delayedReaction)
                {
                    delayedReaction.React(caller);
                }
                else
                {
                    reaction.React(caller);
                }
            }



            return true;
        }
    }

    public reactionAndCondition[] conditioAndReactions = new reactionAndCondition[0];

    public void InitReaction()
    {
        foreach (reactionAndCondition interactables in conditioAndReactions)
        {
            interactables.Init();
        }
    }

    /// <summary>
    /// Run throug all the condition and runn appropriet 
    /// reactions
    /// </summary>
    /// <param name="caller"> the monobehavor that caled this funktion</param>
    public void InteractionRun(MonoBehaviour caller)
    {
        foreach (reactionAndCondition interactables in conditioAndReactions)
        {
            interactables.KontrolAndReact(caller);
        }
    }




    public int getHash
    {
        get
        {
            return Animator.StringToHash(this.name);
        }
    }

    public int getCombindsWithHash
    {
        get
        {
            return Animator.StringToHash(combindsWith.name);
        }
    }

    public int geTtoMakeHash
    {
        get
        {
            return Animator.StringToHash(toMake.name);
        }
    }



}
