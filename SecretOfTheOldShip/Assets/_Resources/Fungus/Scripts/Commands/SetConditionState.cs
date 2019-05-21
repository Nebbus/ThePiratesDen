using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus
{
	[CommandInfo("Condition", 
		"Set Condition", 
		"Sets a condition to a new value.")]
	[AddComponentMenu("")]
	public class SetConditionState : Command {
		/*
		[Tooltip("The variable whos value will be set")]
		[VariableProperty(typeof(GlobalBooleanVariable))]
		[SerializeField] protected GlobalVariable variable;*/


		[SerializeField]
		protected SOBJ_ConditionAdvanced condition;
		[SerializeField]
		private bool setSatisfiedTo;


		public override void OnEnter()
		{
			SetCondition(setSatisfiedTo);

			Continue();
		}

		private void SetCondition (bool newValue){
			condition.satisfied = newValue;
		}
	}
}
