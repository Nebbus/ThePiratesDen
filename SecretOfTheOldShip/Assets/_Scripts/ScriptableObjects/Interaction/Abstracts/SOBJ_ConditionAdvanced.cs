using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SOBJ_ConditionAdvanced : ScriptableObject
{
    public string description;      // A description of the Condition, for example 'BeamsOff'.
    public bool satisfied;          // Whether or not the Condition has been satisfied, for example are the beams off?

    public int hash;                // A number which represents the description.  This is used to compare ConditionCollection Conditions to AllConditions Conditions.

    /// <summary>
    /// This is called from ReactionCollection.
    /// This function contains everything that is required to be done for all
    /// Reactions as well as call the SpecificInit of the inheriting Reaction.
    /// </summary>
    public void Init()
    {
        SpecificInit();
    }

    /// <summary>
    /// This function is virtual so that it can be overridden and used purely
    /// for the needs of the inheriting class.
    /// </summary>
    protected virtual void SpecificInit()
    { }

    /// <summary>
    /// This function is called from ReactionCollection.
    /// It contains everything that is required for all for all Reactions as
    /// well as the part of the Condition which needs to happen immediately.
    /// </summary>
    /// <param name="monoBehaviour"> MonoBehaviour that called this 
    ///  funktions  </param>
    public bool IsSatesfied()
    {
        return advancedCondition();
    }

    /// <summary>
    /// So that we can creat condition that 
    /// relays on aother thoings then bools
    /// </summary>
    /// <returns></returns>
    protected abstract bool advancedCondition();
}
