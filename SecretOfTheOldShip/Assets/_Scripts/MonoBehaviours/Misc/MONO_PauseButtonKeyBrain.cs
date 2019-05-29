using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MONO_PauseButtonKeyBrain : MonoBehaviour {

    public Button button;


    public void Update()
    {
        if (MONO_Settings.instance.getPauseButtonKey)
        {
            if(button != null)
            {
                button.onClick.Invoke();
            }
        }
    }
}
