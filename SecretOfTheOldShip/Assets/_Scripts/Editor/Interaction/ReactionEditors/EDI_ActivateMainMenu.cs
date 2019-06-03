using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(SOBJ_ActivateMainMenu))]
public class EDI_ActivateMainMenu : EDI_Reaction
{
    protected override string GetFoldoutLabel()
    {
        return "This is for activating the main menu";
    }
}
