using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MONO_AllConditionManager : MonoBehaviour {



	public bool CheckCondition(SOBJ_ConditionAdvanced condition)
	{
		return condition.IsSatesfied ();
	}

	public void SetCondition(SOBJ_Condition condition, bool satisfied)
	{
		condition.satisfied = satisfied;
	}
}
