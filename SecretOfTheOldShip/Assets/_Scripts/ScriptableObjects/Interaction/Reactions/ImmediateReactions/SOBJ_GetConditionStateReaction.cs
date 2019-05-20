using UnityEngine;

public class SOBJ_GetConditionStateReaction : SOBJ_Reaction
{
	[Tooltip("The condition to be checked")]
	public SOBJ_ConditionAdvanced condition;

	[Space]
	[Tooltip("Fungus Flowchart containing variable to be set to condition state.")]
	public Fungus.Flowchart destinationFlowchart;
	[Tooltip("Name of the variable keeping condition state.")]
	public string variableName;

	private bool conditionState;

	protected override void ImmediateReaction()
	{
		conditionState = condition.satisfied;
		destinationFlowchart.SetBooleanVariable (variableName, conditionState);
	}
}
