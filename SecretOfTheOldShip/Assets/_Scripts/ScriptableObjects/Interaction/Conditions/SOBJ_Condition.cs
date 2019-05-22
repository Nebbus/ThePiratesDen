using UnityEngine;

// This class is used to determine whether or not Reactions
// should happen.  Instances of Condition exist in two places:
// as assets which are part of the AllConditions asset and as
// part of ConditionCollections.  The Conditions that are part
// of the AllConditions asset are those that are set by
// Reactions and reflect the state of the game.  Those that
// are on ConditionCollections are compared to the
// AllConditions asset to determine whether other Reactions
// should happen.
public class SOBJ_Condition : SOBJ_ConditionAdvanced
{
    [SerializeField]
    private bool startValue = false;

    public bool getStartValue
    {
        get
        {
            return startValue;
        }
    }

    
    protected override bool advancedCondition()
    {
        return satisfied;
    }
}
