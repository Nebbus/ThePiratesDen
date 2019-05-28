using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// let the interactable inherent from this class
///  will make it easyer to handle mouse input whit the fake
///  mouse
/// </summary>
public abstract class MONO_InteractionBase : MonoBehaviour {


    private void Start()
    {

            gameObject.AddComponent(typeof(Selectable));

        Init();
    }

    /// <summary>
    /// private version off start
    /// </summary>
    protected virtual void Init() { }

    /// <summary>
    /// To work as the new click event
    /// </summary>
    public abstract void OnClick();

    public abstract void OnHoverEnterd();

    public abstract void OnHover();

    public abstract void OnHoverExit();

  



}
