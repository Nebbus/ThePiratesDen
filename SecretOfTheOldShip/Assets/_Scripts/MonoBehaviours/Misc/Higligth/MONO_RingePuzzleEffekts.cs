using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/*
 * This scriopt has as gol to higlight the rings 
 * for diffrents effekts, it dose this by shainge valus in the materaials
 * (the start valus contains the start values and resets the 
 * material then this scritp is disabled), it cald from the 
 * 
 * 
 */ 


public class MONO_RingePuzzleEffekts : MonoBehaviour {

    public enum MODE  {DEFAULT, GLITTER, HIGLIGHT };

    public bool run = true;
    public Material mat;
    [Tooltip("Delay for the uppdate of the effect in sec")]
    public float delay = 0.5f;
    public MODE currentMod = MODE.DEFAULT;
    [Space]
    public float startGlossines                 = 0.5f;
    public float startMetalic                   = 0;
    public float startNormal                    = 0.5f;
    public Color startColor                     = new Color(1, 1, 1, 1);
    [Space]
    [Space]
    [Tooltip("Normal value of the metalic smoothnes")]
    public float defultGlossines                = 0.5f;
    public float defultMetalic                  = 0;
    public float defultNormal                   = 0.5f;
    public Color defultColor = new Color(1, 1, 1, 1);
    [Space]
    public float defultReturnSpeed              = 0.1f;
    [Space]
    [Space]
    public float currentGlossines               = 0.5f;
    public float currentMetalic                 = 0f;
    public float currentNormal                  = 0.5f;
    public Color currentColor;
    [Space]
    [Space]
    [Space]
    public float glitterGlossinesMax            = 0.75f;
    public float glitterGlossinesMin            = 0.6f;
    public float glitterGlossinesChangeSpeed    = 0.05f;
    [Space]
    public float glitterMetalicMax              = 1f;
    public float glitterMetalicMin              = 1f;
    public float glitterMetalicChangeSpeed      = 0.1f;
    [Space]
    public float glitterNormalMax               = -0.27f;
    public float glitterNormalMin               = -0.39f;
    public float glitterNormalChangeSpeed            = 0.0005f;
    [Space]
    public Color glitterColor = new Color(1, 1, 1, 1);
    [Space]
    [Space]
    [Space]
    public float higlightGlossinesMax;
    public float higlightGlossinesMin           = 0.6f;
    public float higlightGlossinesChangeSpeed   = 0.05f;
    [Space]
    public float higlightMetalicMax             = 1f;
    public float higlightMetalicMin             = 1f;
    public float higlightMetalicChangeSpeed     = 0.1f;
    [Space]
    public float higlightNormalMax              = 2f;
    public float higlightNormalMin              = 2f;
    public float higlightNormalChangeSpeed      = 0.1f;
    [Space]
    public Color higlightColor = new Color(1, 1, 1, 1);
    [Space]
    [Space]
    public float rotation = 0;

    private WaitForSeconds timer;
    private const string glossinesName        = "_Glossiness";
    private const string normalName           = "_BumpScale";
    private const string metalicName          = "_Metallic";
    private const string colorName            = "_Color";
    private Coroutine glitrar;

    //will beset by ring manager, prevents higlight
    public bool BlockChanges = false;
    public bool lastHiglight = false;


    public void SetHig()
    {
        
        if (BlockChanges)
        {
            return;
        }

        currentMod = MODE.HIGLIGHT;
    }
    public void SetDefult()
    {
        if (BlockChanges)
        {
            return;
        }
        currentMod = MODE.DEFAULT;
    }



    private void OnEnable()
    {
        //defaultValue = mat.GetFloat(gliterVar);
        timer = new WaitForSeconds(delay);

        if (glitrar != null) { StopCoroutine(glitrar); }
        glitrar = StartCoroutine(gliter());



    }

    private void OnDisable()
    {

        if (glitrar != null) { StopCoroutine(glitrar); }

        mat.SetFloat(glossinesName, startGlossines);
        mat.SetFloat(normalName, startNormal);
        mat.SetFloat(metalicName, startMetalic);
        mat.SetColor(colorName, startColor);
    }

    private float round(float value)
    {
        float temp = value * 100f;
        temp = Mathf.Round(temp);

        return temp / 100f;

    }

    public void Update()
    {
        rotation = round(transform.eulerAngles.z);
        if (rotation == 0f)
        {
            if (currentMod != MONO_RingePuzzleEffekts.MODE.GLITTER)
            {
                currentMod = MONO_RingePuzzleEffekts.MODE.GLITTER;
                BlockChanges = true;
            }
        }
        else
        {
            if (currentMod == MONO_RingePuzzleEffekts.MODE.GLITTER)
            {
                currentMod = MONO_RingePuzzleEffekts.MODE.DEFAULT;
                BlockChanges = false;
            }
        }
    }


    public IEnumerator gliter()
    {
        currentGlossines    = startGlossines;
        currentNormal       = startNormal;
        currentMetalic      = startMetalic;
        currentColor        = startColor;
        while (run)
        {

            yield return timer;


            switch (currentMod)
            {
                case MODE.DEFAULT:

                    MoveTowardsValue(defultMetalic, defultReturnSpeed, out currentMetalic, metalicName);

                    MoveTowardsValue(defultNormal, defultReturnSpeed, out currentNormal, normalName);

                    MoveTowardsValue(defultGlossines, defultReturnSpeed, out currentGlossines, glossinesName);

                    mat.SetColor(colorName, defultColor);

                    break;     
                case MODE.GLITTER:
                    MoveTowardsValue(defultMetalic, defultReturnSpeed, out currentMetalic, metalicName);

                    FlutterEffecktBase(glitterGlossinesMax, glitterGlossinesMin, ref glitterGlossinesChangeSpeed, out currentGlossines, glossinesName, true);

                    FlutterEffecktBase(glitterNormalMax, glitterNormalMin, ref glitterNormalChangeSpeed, out currentNormal, normalName, true);

                    mat.SetColor(colorName, glitterColor);

                    break;
                case MODE.HIGLIGHT:

                    FlutterEffecktBase(higlightGlossinesMax, higlightGlossinesMin, ref higlightGlossinesChangeSpeed, out currentGlossines, glossinesName, false);

                    MoveTowardsValueUsingClamp(higlightMetalicMax, defultMetalic, ref higlightMetalicChangeSpeed, out currentMetalic, metalicName);

                    MoveTowardsValue(higlightNormalMax, higlightNormalChangeSpeed, out currentNormal, metalicName);

                    mat.SetColor(colorName, higlightColor);

                    break;
            }
        }
    }

    private void FlutterEffecktBase(float max, float min, ref float changeSpeed, out float current, string nameOfvariable, bool useClamp)
    {
        current = mat.GetFloat(nameOfvariable);
        if ((current >= max && changeSpeed > 0) || (current <= min && changeSpeed < 0))
        {
            changeSpeed *= -1;
        }
        current = (useClamp) ? Mathf.Clamp(current + changeSpeed, min, max) : current + changeSpeed;
        mat.SetFloat(nameOfvariable, current);
    }

    private void MoveTowardsValue(float newValue, float changeSpeed, out float current, string nameOfvariable)
    {
        current = mat.GetFloat(nameOfvariable);
        if (current != newValue)
        {
            current = Mathf.MoveTowards(current, newValue, changeSpeed);
            mat.SetFloat(nameOfvariable, current);

        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="newValue">         The new value to move</param>
    /// <param name="bottomFloor">      The botom flor of the clamp</param>
    /// <param name="changeSpeed">      The speed the change chould happen at</param>
    /// <param name="current">          The current value</param>
    /// <param name="nameOfvariable">   The name of the varibla to change</param>
    private void MoveTowardsValueUsingClamp(float newValue, float bottomFloor, ref float changeSpeed, out float current, string nameOfvariable)
    {
        current = mat.GetFloat(nameOfvariable);
        if (current != newValue)
        {
            if ((current > newValue && changeSpeed > 0) || (current < newValue && changeSpeed < 0))
            {
                changeSpeed *= -1;
            }
            current = Mathf.Clamp(current + changeSpeed, bottomFloor, newValue);
            mat.SetFloat(nameOfvariable, current);
        }

    }


}
