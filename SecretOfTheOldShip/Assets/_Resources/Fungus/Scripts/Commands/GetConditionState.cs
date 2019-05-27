using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus
{
	[CommandInfo("Condition", 
		"Get Condition", 
		"Get the current state of a condition.")]
	[AddComponentMenu("")]
	public class GetConditionState : Command {
		/*
		[Tooltip("The variable whos value will be set")]
		[VariableProperty(typeof(GlobalBooleanVariable))]
		[SerializeField] protected GlobalVariable variable;*/


		[SerializeField]
		protected SOBJ_ConditionAdvanced condition;

		[Tooltip("The variable in which the conditions state will be saved.")]
		[VariableProperty(typeof(BooleanVariable),
			typeof(IntegerVariable), 
			typeof(FloatVariable), 
			typeof(StringVariable))]
		[SerializeField] protected Variable variable;


		private SetVariable setVariableCommand;

		public override void OnEnter()
		{
			ConditionToVariable();
		}

		private void ConditionToVariable (){
			SetOperator assign = SetOperator.Assign;
			setVariableCommand.SetNewBoolVariable (variable, assign, condition.satisfied);
			setVariableCommand.OnEnter ();
		}
	}
}
