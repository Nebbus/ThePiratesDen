using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This script will run throu
/// all the condition and if
/// they are true will this script ass defult 
/// deactevate this boject, if needed 
/// can a reaction collection be used insted
/// but then will the deactevation not be runed.
/// </summary>
public class MONO_OnSceneStart_Object : MonoBehaviour
{
    public string description;

    public SOBJ_ConditionAdvanced[] conditiosns =  new SOBJ_ConditionAdvanced[0];
    public MONO_ReactionCollection reactionCollection = null;
    public bool setGamobjectTo = false;
    /// <summary>
    /// This is called by the Interactable one at a time for 
    /// each of its ConditionCollections until one returns true.
    /// </summary>
    /// <returns></returns>
    public void CheckAndReact()
    {
        // Go through all Conditions...
        for (int i = 0; i < conditiosns.Length; i++)
        {
            /* ... and check them against the AllConditions version of the Condition. 
             * If they don't have the same satisfied flag, return false.
             */
            if (!SOBJ_AllConditions.CheckCondition(conditiosns[i]))
            {
                return;
            }

        }

        // If there is an ReactionCollection assigned, call its React function.
        // else turn this gamibject on or of.
        if(reactionCollection == null)
        {

            gameObject.SetActive(setGamobjectTo);
        }
        else
        {
            reactionCollection.React();
        }

    }



}
