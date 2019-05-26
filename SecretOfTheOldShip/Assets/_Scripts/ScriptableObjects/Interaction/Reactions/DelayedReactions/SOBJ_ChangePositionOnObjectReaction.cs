using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOBJ_ChangePositionOnObjectReaction : SOBJ_Reaction
{

    public Transform transformToRelocate = null;
    public Transform newPosition         = null;


    protected override void ImmediateReaction()
    {
        if (transformToRelocate != null && newPosition != null)
        {
            transformToRelocate.position = newPosition.position;
            transformToRelocate.rotation = newPosition.rotation;
        }
        
    }
}
