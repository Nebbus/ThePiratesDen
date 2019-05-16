using UnityEngine;
using System.Collections.Generic;


[RequireComponent(typeof(MONO_GlowObject_higlightAll))]
public class MONO_GlowObject : MonoBehaviour
{


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


//==========================================================
// Local operation variables
//==========================================================

    public Color localGlowColor = Color.white;
    [Tooltip("Time it takes for the hilgit to fade in")]
    public float localLerpFactor   = 10;




	private List<Material> _materials = new List<Material>();
	private Color _currentColor;
	private Color _targetColor;




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
    public void HigligtON()
    {
        _targetColor = GetGlowColor;
        enabled      = true;
    }
    public void HigligtOFF()
    {
        _targetColor = Color.black;
        enabled      = true;
    }


	/// <summary>
	/// Loop over all cached materials and update their 
    /// color, disable self if we reach our target color.
	/// </summary>
	private void Update()
	{
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
