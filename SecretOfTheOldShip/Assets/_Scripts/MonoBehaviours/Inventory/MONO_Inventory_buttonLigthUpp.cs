using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;

public class MONO_Inventory_buttonLigthUpp : MonoBehaviour {


    [System.Serializable]
    public struct itemImageMove
    {
        public float pushUpForce;

        [Space]
        [Space]
        public Vector3 startPosition;
        public RectTransform imageTransform;
        public Rigidbody2D rigedBody2D;
        public Image image;
        public Sprite defultSprite;
        [Space]
        [Space]

        public GameObject itemImageObject;

        public void ItemImageMoveINIT()
        {
            rigedBody2D          = itemImageObject.GetComponent<Rigidbody2D>();
            image                = itemImageObject.GetComponent<Image>();
            defultSprite         = image.sprite;
            imageTransform       = itemImageObject.GetComponent<RectTransform>();
            startPosition        = imageTransform.position;
            rigedBody2D.simulated = false;
            itemImageObject.SetActive(false);

        }


        public void addItemToInventory()
        {
            itemImageObject.SetActive(true);
            rigedBody2D.simulated = true;
            rigedBody2D.AddForce(new Vector2(0f, pushUpForce));
        }

        private List<RaycastResult> resultsG;
        public GraphicRaycaster presistentCanvansPraycaster;
        private EventSystem presistentSeneEventSystem;

        private GraphicRaycaster getGraycaster
        {
            get
            {
                if (presistentCanvansPraycaster == null || !presistentCanvansPraycaster.gameObject.activeSelf)
                {
                    presistentCanvansPraycaster = FindObjectOfType<GraphicRaycaster>();
                }

                return presistentCanvansPraycaster;
            }
        }

        public bool overInvenotyButton()
        {
            resultsG = EXT_RayCast.GraphicRayCast(getGraycaster, presistentSeneEventSystem, imageTransform.position);


            //RaycastResult theLa
            //==================================================================================
            // Handel Graphical interactions
            //==================================================================================
            foreach (RaycastResult result in resultsG)
            {

                bool r = result.gameObject.name == "InventoryButtonImage";
                if (r)
                {
                    itemImageObject.SetActive(false);
                    rigedBody2D.simulated = false;
                    imageTransform.position = startPosition;
                    image.sprite = defultSprite;

                    return true;

                }
                return false;
            }


            return false;



        }
    }


    public itemImageMove higlightEffect = new itemImageMove();





















    public GameObject selectedObject;

    public Animator buttonAnimator;

    [Space]
    public bool flashing        = false;
    public bool flashingOff     = false;
    public bool flashingIn      = true;
    public bool startedFlash    = false;
    public bool startUseLowBound = false;
    [Space]

    public Coroutine curentFlash;
    public int alphaValue = 0;

    [Range(0, 224)]
    public int loweBound = 100;


    private int getLowBound
    {
        get
        {
            return (startUseLowBound) ? loweBound : 0;
        }
    }


    public float time = 10f;
    [Range(1,225)]
    public int changeValue = 25;


    private float lastTime;


    public Color originalFer;

    private void Start()
    {
        higlightEffect.ItemImageMoveINIT();
    }




    private void Update()
    {

        if (flashing || flashingOff)
        {
            if (!flashingOff && (Time.realtimeSinceStartup - lastTime) >= time)
            {
                StopFlashing();
            }

            if (selectedObject.GetComponent<Renderer>() == null)
            {
                selectedObject.GetComponent<Image>().color = new Color32((byte)255, (byte)255, (byte)255, (byte)alphaValue);
            }

        }
    }


    private bool t = false;
    public void startFlashing()
    {
        if (!t && !flashing)
        {
            t = true;
            flashing = true;
            StartCoroutine(lobbItem());
        }
    }

    public void StopFlashing()
    {
        startedFlash    = false;
        flashing        = false;
        flashingOff     = true;
        startUseLowBound = false;
        StopCoroutine(curentFlash);

        curentFlash = StartCoroutine(FadeOffHiglight());
        t = false;
    }



    public IEnumerator lobbItem()
    {

        higlightEffect.addItemToInventory();
        yield return higlightEffect.overInvenotyButton();


        if (selectedObject.GetComponent<Renderer>() == null)
        {
            originalFer = selectedObject.GetComponent<Image>().color;
        }
        buttonAnimator.SetTrigger("pickUpp");

        startedFlash = true;

        flashingIn = false;
        flashingOff = false;

        lastTime = Time.realtimeSinceStartup;
        curentFlash = StartCoroutine(FlashObject());



    }


    IEnumerator FadeOffHiglight()
    {
        while (alphaValue > 0)
        {

            yield return new WaitForSeconds(0.025f);
     
            alphaValue -= 25;
            alphaValue = Mathf.Clamp(alphaValue, 0, 255);

        }

        if (selectedObject.GetComponent<Renderer>() == null)
        {
            selectedObject.GetComponent<Image>().color = originalFer;

        }

        flashingOff = false;

    }
    IEnumerator FlashObject()
    {
        while (flashing)
        {

            yield return new WaitForSeconds(0.025f);


            if (!flashingIn && alphaValue >= 255)
            {
                flashingIn = true;
                startUseLowBound = true;
            }
            else if (flashingIn && (alphaValue <= 0))
            {
                flashingIn = false;
            }

            alphaValue += (flashingIn) ? -changeValue : changeValue;
            alphaValue = Mathf.Clamp(alphaValue, getLowBound, 255);

        }
    }
}
