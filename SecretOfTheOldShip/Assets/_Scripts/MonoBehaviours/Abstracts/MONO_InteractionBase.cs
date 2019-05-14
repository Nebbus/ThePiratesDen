using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// let the interactable inherent from this class
///  will make it easyer to handle mouse input whit the fake
///  mouse
/// </summary>
public abstract class MONO_InteractionBase : MonoBehaviour {


    /// <summary>
    /// To work as the new click event
    /// </summary>
    public abstract void OnClick();

    public abstract void OnHoverEnterd();

    public abstract void OnHover();

    public abstract void OnHoverExit();





}
