using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOBJ_OnOffGUIelemet : SOBJ_DelayedReaction
{

    public bool pauseButtonEnavbe = false;
    public bool HintButtonEnavbe = false;
    public bool InventoryEnavbe = false;


    public GameObject hintButtonbject;
    public GameObject pauseButtonbject;
    public GameObject invnetoryObject;


    /// <summary>
    /// Realy ugly but works..
    /// </summary>
    protected override void SpecificInit()
    {
        MONO_HintButton temp  = FindObjectOfType(typeof(MONO_HintButton)) as MONO_HintButton;
        MONO_Inventory temp2 = FindObjectOfType(typeof(MONO_Inventory)) as MONO_Inventory;
        hintButtonbject = temp.gameObject;
        GameObject tempP = hintButtonbject.transform.parent.gameObject;
        invnetoryObject = temp2.gameObject;

        for (int i = 0; i < tempP.transform.childCount; i++)
        {
           
            if (tempP.transform.GetChild(i).gameObject.name != hintButtonbject.name && tempP.transform.GetChild(i).gameObject.name != invnetoryObject.name)
            {
                pauseButtonbject = tempP.transform.GetChild(i).gameObject;
            }


        }
    }


    protected override void ImmediateReaction()
    {
        hintButtonbject.SetActive(HintButtonEnavbe);
        pauseButtonbject.SetActive(pauseButtonEnavbe);
        invnetoryObject.SetActive(InventoryEnavbe);

    }
}
