using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour {

    public enum MODE  {DEFAULT, GLITTER, HIGLIGHT };

    public bool run = true;
    public Material mat;
    [Tooltip("Delay for the uppdate of the effect in sec")]
    public float delay = 0.5f;
    public MODE currentMod = MODE.DEFAULT;

    [Space]
    [Space]

    [Tooltip("Normal value of the metalic smoothnes")]
    public float defultGlossines = 0.5f;
    public float currentGlossines;
    [Space]
    public float metalicDefautl = 0;
    public float metalicCurrent = 0;

    [Space]
    public float backToDefualtSpeed = 0.1f;

    [Space]
    [Space]

    public float glitterGlossinesMax     = 0.75f;
    public float glitterGlossinesMin     = 0.6f;
    public float glitterGlossinesChange  = 0.05f;


    [Space]
    [Space]
    public float normalOffsetDefult = 0.5f;
    public float normalOffsetCurrent = 0.5f;
    public float normalOffsetMax = -0.27f;
    public float normalOffsetMin = -0.39f;
    public float normalOffsetChange = 0.0005f;

    [Space]
    [Space]

    public float higlightGlossinesMax;
    public float higlightGlossinesMin   = 0.6f;
    public float higlightGlossinesChange = 0.05f;

    public float higlightMetalic        = 1f;
    public float higlightMetalicSpeed   = 0.1f;

    public float higlightNormal         = 2f;
    public float higlightNormalSpeed    = 0.1f;

    private WaitForSeconds timer;
    private const string glossinesName      = "_Glossiness";
    private const string bumpMapScale   = "_BumpScale";
    private const string metalic        = "_Metallic";
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

        mat.SetFloat(glossinesName, defultGlossines);
        mat.SetFloat(bumpMapScale, normalOffsetDefult);
        mat.SetFloat(metalic, metalicDefautl);
    }
   
   
    public IEnumerator gliter()
    {
        currentGlossines    = defultGlossines;
        normalOffsetCurrent = normalOffsetDefult;
        while (run)
        {

            yield return timer;


            switch (currentMod)
            {
                case MODE.DEFAULT:
                    if (metalicCurrent != metalicDefautl)
                    {
                        float temp = Mathf.MoveTowards(metalicCurrent, metalicDefautl, backToDefualtSpeed);
                        metalicCurrent = temp;
                        mat.SetFloat(metalic, metalicCurrent);

                    }
                    if (normalOffsetCurrent != normalOffsetDefult)
                    {
                        float temp = Mathf.MoveTowards(normalOffsetCurrent, normalOffsetDefult, backToDefualtSpeed);
                        normalOffsetCurrent = temp;
                        mat.SetFloat(bumpMapScale, normalOffsetCurrent);
                    }
                    if (currentGlossines != defultGlossines)
                    {
                        float temp = Mathf.MoveTowards(currentGlossines, defultGlossines, backToDefualtSpeed);
                        currentGlossines = temp;
                        mat.SetFloat(glossinesName, currentGlossines);
                    }

            


                    break;
                

                case MODE.GLITTER:

                    if (metalicCurrent != metalicDefautl)
                    {
                        float temp = Mathf.MoveTowards(metalicCurrent, metalicDefautl, backToDefualtSpeed);
                        metalicCurrent = temp;
                        mat.SetFloat(metalic, metalicCurrent);

                    }

                    //Fluters the glossynes
                    if ((currentGlossines > glitterGlossinesMax && glitterGlossinesChange > 0) || (currentGlossines < glitterGlossinesMin && glitterGlossinesChange < 0))
                    {
                        glitterGlossinesChange *= -1;
                    }
                    currentGlossines = mat.GetFloat(glossinesName) + glitterGlossinesChange;
                    mat.SetFloat(glossinesName, currentGlossines);

                    //Fluters the normal
                    if ((normalOffsetCurrent > normalOffsetMax && normalOffsetChange > 0) || (normalOffsetCurrent < normalOffsetMin && normalOffsetChange < 0))
                    {
                        normalOffsetChange *= -1;
                    }
                    normalOffsetCurrent = mat.GetFloat(bumpMapScale) + normalOffsetChange;  
                    mat.SetFloat(bumpMapScale, normalOffsetCurrent);


                    break;
                case MODE.HIGLIGHT:

                    //Fluters the glossynes
                    if ((currentGlossines > higlightGlossinesMax && higlightGlossinesChange > 0) || (currentGlossines < higlightGlossinesMin && higlightGlossinesChange < 0))
                    {
                        higlightGlossinesChange *= -1;
                    }
                    currentGlossines = mat.GetFloat(glossinesName) + higlightGlossinesChange;
                    mat.SetFloat(glossinesName, currentGlossines);





                    //Sets the metalic value
                    if (metalicCurrent != higlightMetalic)
                    {
                        metalicCurrent = Mathf.Clamp(mat.GetFloat(metalic) + higlightMetalicSpeed, metalicDefautl, higlightMetalic);
                        mat.SetFloat(metalic, metalicCurrent);

                    }

                    if (normalOffsetCurrent != higlightNormal)
                    {
                        float temp = Mathf.MoveTowards(normalOffsetCurrent, higlightNormal, higlightNormalSpeed);
                        normalOffsetCurrent = temp;
                        mat.SetFloat(bumpMapScale, normalOffsetCurrent);
                    }

                    break;
            }



            


        }
    }
}
