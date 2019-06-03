using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MONO_setActivFungusSaydialog : MonoBehaviour {

    public Fungus.SayDialog lastBox;

   public void setMeActive()
    {
        Fungus.SayDialog.ActiveSayDialog = lastBox;
    }
}
