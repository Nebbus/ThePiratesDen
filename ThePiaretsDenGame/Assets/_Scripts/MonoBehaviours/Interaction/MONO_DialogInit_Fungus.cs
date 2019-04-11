﻿using UnityEngine;
using System.Collections;
using Fungus;


	/// <summary>
	/// The block will execute when an object is interacted with.
	/// </summary>
	[EventHandlerInfo("Reactions",
		"OnInteract",
		"The block will execute when an object is interacted with.")]
	[AddComponentMenu("")]
	public class MONO_DialogInit_Fungus : EventHandler
	{
		protected virtual void DialogInit()
		{
			ExecuteBlock ();
		}
	}}
