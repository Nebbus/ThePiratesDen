using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MONO_ItemLogic : MonoBehaviour {

    public SOBJ_Item invetoryItems;

    public Reaction flavourText;


    public struct Reaction
    {
        public SOBJ_ConditionCollection[] conditionCollections;
        public SOBJ_Reaction[] reactions;

    }


    public void Combind()
    {

    }

    private void OnMouseEnter()
    {
        
    }

    private void OnMouseDown()
    {
        if(MONO_PickedUpItem.instance.currentItem != null)
        {

        }
        
    }

    private void OnMouseUp()
    {
        
    }

    
}
