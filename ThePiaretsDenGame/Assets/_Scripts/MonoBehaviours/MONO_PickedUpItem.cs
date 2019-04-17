using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is a singleton 
/// </summary>
public class MONO_PickedUpItem : MonoBehaviour
{


    public static MONO_PickedUpItem instance;


    public SOBJ_Item currentItem = null;
    public Image     currentItemImage;
    public GameObject itemInventoryInstance;



  

    public void Start()
    {

        if( instance == null)
        {
            instance = this;
        }
        else
        {
            GameObject.Destroy(this);
        }
        
    }


    void Update ()
    {
        transform.position =  Input.mousePosition;
	}


  
}
