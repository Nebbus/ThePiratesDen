using UnityEngine;

public class SOBJ_ConditionCollection : ScriptableObject
{
    public string                   description;                                 // Description of the ConditionCollection.  This is used purely for identification in the inspector.
    public SOBJ_ConditionAdvanced[] requiredConditions = new SOBJ_ConditionAdvanced[0];   // The Conditions that need to be met in order for the ReactionCollection to React.
    public MONO_ReactionCollection reactionCollection;                           // Reference to the ReactionCollection that will React should all the Conditions be met.

    /// <summary>
    /// This is called by the Interactable one at a time for 
    /// each of its ConditionCollections until one returns true.
    /// </summary>
    /// <returns></returns>
    public bool CheckAndReact()
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

        // If there is an ReactionCollection assigned, call its React function.
        if (reactionCollection)
        {
            reactionCollection.React();
        }
        // A Reaction happened so return true.
        return true;
    }
}
