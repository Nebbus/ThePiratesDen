using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MONO_PauseButtonKeyBrain : MonoBehaviour {

    public MONO_SimpleInteractable button;
    public MONO_SceneManager monoSceneManager;

    public void Start()
    {
        monoSceneManager = FindObjectOfType<MONO_SceneManager>();
        if(button == null)
        {
            button = GetComponent<MONO_SimpleInteractable>();
        }
    }


    public void Update()
    {
        if (MONO_Settings.instance.getPauseButtonKey && monoSceneManager.getSetHandleInput)
        {
            if(button != null)
            {
                button.OnClick() ;
            }
        }
    }
}
