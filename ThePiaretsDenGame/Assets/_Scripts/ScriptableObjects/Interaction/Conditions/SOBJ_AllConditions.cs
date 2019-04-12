﻿using UnityEngine;

// This script works as a singleton asset.  That means that
// it is globally accessible through a static instance
// reference.  
public class SOBJ_AllConditions : SOBJ_ResettableScriptableObject
{
    public SOBJ_Condition[] conditions;                      // All the Conditions that exist in the game.


    private static SOBJ_AllConditions instance;              // The singleton instance.


    private const string loadPath = "SOBJ_AllConditions";    // The path within the Resources folder that 


    public static SOBJ_AllConditions Instance                // The public accessor for the singleton instance.
    {
        get
        {
            // If the instance is currently null, try to find an AllConditions instance already in memory.
            if (!instance)
            {
                instance = FindObjectOfType<SOBJ_AllConditions>();
            }
            // If the instance is still null, try to load it from the Resources folder.
            if (!instance)
            {
                instance = Resources.Load<SOBJ_AllConditions>(loadPath);
            }

            // If the instance is still null, report that it has not been created yet.
            if (!instance)
            {
                Debug.LogError("SOBJ_AllConditions has not been created yet.  Go to Assets > Create > SOBJ_AllConditions.");
            }

            return instance;
        }
        set { instance = value; }
    }

    /// <summary>
    /// This function will be called at Start once per run of the game.
    /// </summary>
    public override void Reset()
    {
        // If there are no conditions, do nothing.
        if (conditions == null)
            return;

        // Set all of the conditions to not satisfied.
        for (int i = 0; i < conditions.Length; i++)
        {
            conditions[i].satisfied = false;
        }
    }

    /// <summary>
    /// This is called from ConditionCollections when they 
    /// are being checked by an Interactable that has been clicked on.
    /// </summary>
    /// <param name="requiredCondition"></param>
    /// <returns></returns>
    public static bool CheckCondition(SOBJ_Condition requiredCondition)
    {
        // Cache the condition array.
        SOBJ_Condition[] allConditions   = Instance.conditions;
        SOBJ_Condition   globalCondition = null;

        // If there is at least one condition...
        if (allConditions != null && allConditions[0] != null)
        {
            // ... go through all the conditions...
            for (int i = 0; i < allConditions.Length; i++)
            {
                /* ... and if they match the given condition then this
                 * is the global version of the requiredConditiond.
                 * */
                if (allConditions[i].hash == requiredCondition.hash)
                {
                    globalCondition = allConditions[i];
                }
            }
        }

        // If by this point a globalCondition hasn't been found then return false.
        if (!globalCondition)
            return false;

        // Return true if the satisfied states match, false otherwise.
        return globalCondition.satisfied == requiredCondition.satisfied;
    }
}