using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOBJ_ActivateMainMenu : SOBJ_DelayedReaction
{



    protected override void ImmediateReaction()
    {
        MONO_Menus temp = GameObject.FindObjectOfType(typeof(MONO_Menus)) as MONO_Menus;
        temp.OpenMenu();
    }
}
