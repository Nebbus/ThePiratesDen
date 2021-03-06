﻿using UnityEngine;
using System.Collections.Generic;


[RequireComponent(typeof(MONO_GlowObject_higlightAll))]
public class MONO_GlowObject : MonoBehaviour
{

    public bool DebugHigligth = false;

//==========================================================
// Opperation mode bools
//==========================================================
    [Tooltip(" if false, uses the color determed in " +
             "MONO_GlocwComposite that is on the main camera")]
    public bool useLocalGlowColor = false;

    [Tooltip(" if false, uses the lerpFacto determed in " +
             "MONO_GlocwComposite that is on the main camera")]
    public bool useLocalLerpFactor = false;
    [Space]


    [Tooltip("For acteveting and de acteveting the higligt ")]
    public bool doHiglight = true;

    [Tooltip("Disides if the higligt all call should be ignored")]
    public bool ignorHigligtAll = false;

//==========================================================
// Local operation variables
//==========================================================

    public Color localGlowColor = Color.white;
    [Tooltip("Time it takes for the hilgit to fade in")]
    public float localLerpFactor   = 10;




	private List<Material> _materials = new List<Material>();
	private Color _currentColor;
	private Color _targetColor;

    private bool isHintButtonCalled = false;


//==========================================================
// Get and set structures
//==========================================================
    private float GetlerpFacto
    {
        get
        {
            return (useLocalLerpFactor) ? localLerpFactor : MONO_GlowComposite.glowCompositeInstance.globalLerpFactor;
        }
    }
    private Color GetGlowColor
    {
        get
        {

            if (useLocalGlowColor)
            {
                return localGlowColor;
            }
            else
            {
                return  MONO_GlowComposite.glowCompositeInstance.globalGlowColor;
            }


        }
    }

    public Renderer[] Renderers
    {
        get;
        private set;
    }
    public Color CurrentColor
    {
        get { return _currentColor; }
    }

    void Start()
	{
        Renderers = GetComponentsInChildren<Renderer>();

		foreach (var renderer in Renderers)
		{	
			_materials.AddRange(renderer.materials);
		}


    }

//==========================================================
// Higligt on and offf
//==========================================================
    public void HigligtON(bool isPartOfHint)
    {
        if (doHiglight && !isHintButtonCalled)
        {
            isHintButtonCalled = isPartOfHint && !ignorHigligtAll;
            _targetColor       = GetGlowColor;
            enabled            = doHiglight;
        }
  
    }
    public void HigligtOFF(bool isCalldFromHintButton)
    {
        //uggli butt works
        if (isCalldFromHintButton && !ignorHigligtAll)
        {
            _targetColor        = Color.black;
            enabled             = true;
            isHintButtonCalled  = false;
        }
        else 
        {
            if (!isHintButtonCalled)
            {
                _targetColor = Color.black;
                enabled = true;
            }

        }
       
    }


	/// <summary>
	/// Loop over all cached materials and update their 
    /// color, disable self if we reach our target color.
	/// </summary>
	private void Update()
	{

        if (DebugHigligth)
        {
            HigligtON(false);
            DebugHigligth = false;
        }

		_currentColor = Color.Lerp(_currentColor, _targetColor, Time.deltaTime * GetlerpFacto);

		for (int i = 0; i < _materials.Count; i++)
		{
			_materials[i].SetColor("_GlowColor", _currentColor);
        }
        

        if (_currentColor.Equals(_targetColor))
		{
			enabled = false;
		}
	}
}
