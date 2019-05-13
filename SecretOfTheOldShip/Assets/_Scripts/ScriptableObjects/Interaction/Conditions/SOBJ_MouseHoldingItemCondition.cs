using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*This is a condition witch simply compares if the item 
 * that the player is holding in the mouse pointer is the
 * same as the item specifyed by holdinItem. All futere condition
 * that compares stuff in the game live muste inherret[ärva] 
 * this class and not from SOBJ_ConditionAdvanced.
 * (But the editor can inherret[ärva] from EDI_ConditionAdvanced)
 */
public class SOBJ_MouseHoldingItemCondition : SOBJ_ConditionAdvanced
{
    [SerializeField]
    private SOBJ_Item requierdHoldingItem;



    public SOBJ_Item getRequierdHoldingItem
    {
        get
        {
            return requierdHoldingItem;
        }
      }


    protected override bool advancedCondition()
    {

             return satisfied == (requierdHoldingItem.getHash == MONO_AdventureCursor.instance.getMonoHoldedItem.currentItem.getHash);

    }
}
