using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class MONO_GlowComposite : MonoBehaviour
{

    public static MONO_GlowComposite glowCompositeInstance;


	[Range (0, 10)]
	public float glowIntensity = 2;

    [Tooltip("Global Color for the higligth ")]
    public Color globalGlowColor = Color.white;

    [Tooltip("Global time for the higligth to fade in")]
    public float globalLerpFactor = 10;


    private MONO_GlowPrePass prePass;


	private Material _compositeMat;

    [Space]
    [Tooltip("Will activete the higligt for all higligt objects")]
    public bool DEBUGhiglightAll = false;




    private void Awake()
    {
        if (glowCompositeInstance == null)
        {
            glowCompositeInstance = this;
        }
        else
        {
            Debug.LogError(this.ToString() + " is trying to create a second GlowComposite");
        }
    }



    private void Update()
    {
        if (DEBUGhiglightAll)
        {
            
            MONO_EventManager.EventParam param = new MONO_EventManager.EventParam();
            MONO_EventManager.TriggerEvent(MONO_EventManager.onHiglightAllInteractablesInScene_NAME, param);
            MONO_EventManager.TriggerEvent(MONO_EventManager.onGroundEvnetManager_NAME, param);
        }
    }



    void OnEnable()
	{
		_compositeMat = new Material(Shader.Find("Hidden/GlowComposite"));
    }

	void OnRenderImage(RenderTexture src, RenderTexture dst)
	{

		_compositeMat.SetFloat("_Intensity", glowIntensity);
        Graphics.Blit(src, dst, _compositeMat, 0);
	}
}
