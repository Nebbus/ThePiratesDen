using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;

public class MONO_Inventory_buttonLigthUpp : MonoBehaviour {

    /// <summary>
    /// a struct for hangeling the lobing of the item.
    /// </summary>
    [System.Serializable]
    public struct itemImageMove
    {
        [Tooltip("The uppwards velosity of the item")]
        public float pushUpForce;

        public string debug;
        public string[] debuug;
        public bool debugg;
        public GameObject lobedItem;
        public GameObject inventoryButton;

        private Vector3             startPosition;
        private RectTransform       imageTransform;
        private Rigidbody2D         rigedBody2D;
        private Image               image;
        private Sprite              defultSprite;
        private List<RaycastResult> resultsG;
        public GraphicRaycaster     presistentCanvansPraycaster;
        private EventSystem         presistentSeneEventSystem;


        public void ItemImageMoveINIT()
        {
            rigedBody2D           = lobedItem.GetComponent<Rigidbody2D>();
            image                 = lobedItem.GetComponent<Image>();
            defultSprite          = image.sprite;
            imageTransform        = lobedItem.GetComponent<RectTransform>();
            startPosition         = imageTransform.position;
            rigedBody2D.simulated = false;
            lobedItem.SetActive(false);

        }

        /// <summary>
        /// acctivates the item (the sprite was set in the 
        /// add item funtkion in MONO_Invnetory.
        /// (sacrefised dignigty and embraised cuppllings to get the thing working quick)
        /// </summary>
        public void addItemToInventory()
        {
            lobedItem.SetActive(true);
            rigedBody2D.simulated = true;
            rigedBody2D.AddForce(new Vector2(0f, pushUpForce));
            rigedBody2D.velocity = new Vector2(0f, pushUpForce);
        }



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

        public void debbug()
        {
            resultsG = EXT_RayCast.GraphicRayCast(getGraycaster, presistentSeneEventSystem, imageTransform.position);
            RaycastResult[] temp = resultsG.ToArray();

            debuug = new string[temp.Length];

            for (int i = 0; i < temp.Length; i++)
            {
                debuug[i] = temp[i].gameObject.name;
            }

            //foreach (RaycastResult result in resultsG)
            //{
            //    debug = result.gameObject.name;
            //    debugg = result.gameObject.name == inventoryButton.name;
               
            //}
        }





        /// <summary>
        /// Controlls if the item has hitted the inventory yet
        /// </summary>
        /// <returns></returns>
        public bool overInvenotyButton()
        {
            resultsG = EXT_RayCast.GraphicRayCast(getGraycaster, presistentSeneEventSystem, imageTransform.position);


            //RaycastResult theLa
            //==================================================================================
            // Handel Graphical interactions
            //==================================================================================
            foreach (RaycastResult result in resultsG)
            {
                debug = result.gameObject.name;
                debugg = result.gameObject.name == inventoryButton.name;
                if (result.gameObject.name == inventoryButton.name)
                {
                    lobedItem.SetActive(false);
                    rigedBody2D.simulated = false;
                    imageTransform.position     = startPosition;
                    image.sprite                = defultSprite;

                    return true;

                }
                return false;
            }


            return false;



        }
    }


    public itemImageMove lobingItemEffectStruct = new itemImageMove();

    public GameObject selectedObject;

    public Animator buttonAnimator;

    [Space]
    public bool flashing                    = false;
    public bool startedFlash                = false;
    public bool flashingOff                 = false;
    public bool flashingIn                  = true;
    public bool startUseLowBound            = false;
    private bool lobingTheItemToInventoryn = false;
    [Space]

    public Coroutine curentFlash;
    public int alphaValue = 0;

    [Range(0, 224)]
    public int loweBound = 100;

    /// <summary>
    /// to set a lowe bund to be actiw only after it first has loaded in
    /// </summary>
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
        lobingItemEffectStruct.ItemImageMoveINIT();
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
        else if (lobingTheItemToInventoryn)
        {
            lobingItemEffectStruct.debbug();

            if (lobingItemEffectStruct.overInvenotyButton())
            {
                realsStaert();
                lobingTheItemToInventoryn = false;
            }
        }
    }


   //FindObjectOfType<MONO_Inventory>().SetHandleINput(handelInput)
    public void startHigligtReaction()
    {
        if (!lobingTheItemToInventoryn && !flashing)
        {
            lobingTheItemToInventoryn = true;

            lobingItemEffectStruct.addItemToInventory();
        }
    }

    private void realsStaert()
    {
      

        if (selectedObject.GetComponent<Renderer>() == null)
        {
            originalFer = selectedObject.GetComponent<Image>().color;
        }
        buttonAnimator.SetTrigger("pickUpp");

        startedFlash = true;

        flashingIn = false;
        flashingOff = false;
        flashing = true;
        lastTime = Time.realtimeSinceStartup;
        curentFlash = StartCoroutine(FlashObject());
    }
    public void StopFlashing()
    {
        startedFlash    = false;
        flashing        = false;
        flashingOff     = true;
        startUseLowBound = false;
        StopCoroutine(curentFlash);

        curentFlash = StartCoroutine(FadeOffHiglight());
        lobingTheItemToInventoryn = false;
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
