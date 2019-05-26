using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SOBJ_ChangePositionOnObjectReaction))]
public class EDI_ChangePositionOnObjectReaction : EDI_Reaction {
    protected override string GetFoldoutLabel()
    {
        return " change cosed transform position ";
    }
    protected override void DrawReaction()
    {
        EditorGUILayout.HelpBox("Teleports the chosed transform to the chosed new position", MessageType.Info);
        base.DrawReaction();
    }


}
