using UnityEngine;

/* This is one of the core features of the game.
*  Each one acts like a hub for all things that transpire
*  over the course of the game.
*  The script must be on a gameobject with a collider and
*  an event trigger.  The event trigger should tell the
*  player to approach the interactionLocation and the 
*  player should call the Interact function when they arrive.
*/

[RequireComponent(typeof(MONO_GlowObject))]// this is for the glow
public class MONO_Interactable : MONO_InteractionBase
{
    /* The position and rotation the player should go 
     * to in order to interact with this Interactable.
     */ 
    public Transform interactionLocation;
    /* All the different SOBJ_Conditions and relevant Reactions 
     * that can happen based on them.
     */ 
    public SOBJ_ConditionCollection[] conditionCollections = new SOBJ_ConditionCollection[0];
    // If none of the SOBJ_ConditionCollection are reacted to this one is used.
    public MONO_ReactionCollection defaultReactionCollection;

    private MONO_GlowObject glowObjectComponent;
    private MONO_GlowObject getGlowObjectComponent
    {
        get
        {
            if (glowObjectComponent == null)
            {
                glowObjectComponent = GetComponent<MONO_GlowObject>();
                // attemts to add the component
                if (glowObjectComponent == null)
                {
                    glowObjectComponent = gameObject.AddComponent(typeof(MONO_GlowObject)) as MONO_GlowObject;
                    if (glowObjectComponent == null)
                    {
                        Debug.LogError(gameObject.ToString() + " : It was inpossible to retrive the MONO_GlowObject from this object");
                    }
                }
            }
            
           
            return glowObjectComponent;
        }

    }

    /// <summary>
    /// creates the higlight
    /// </summary>
    private void Start()
    {
        if (gameObject.GetComponent(typeof(UnityEngine.UI.Selectable)) == null)
        {
            gameObject.AddComponent(typeof(UnityEngine.UI.Selectable));
        }
        MONO_GlowObject temp = getGlowObjectComponent;
    }
   
    



    // This is called when the player arrives at the interactionLocation.
    public void Interact()
    {
        // Go through all the ConditionCollections...
        for (int i = 0; i < conditionCollections.Length; i++)
        {
            /* ... then check and potentially react to each. 
             * If the reaction happens, exit the function.
             */ 
            if (conditionCollections[i].CheckAndReact())
            {
                return;
            }

        }
        // If none of the reactions happened, use the default MONO_ReactionCollection.
        if(defaultReactionCollection != null)
        defaultReactionCollection.React();
    }

    public override void OnClick()
    {
        // Not used, the interact is cald from player movment
        
    }

    public override void OnHoverEnterd()
    {
        MONO_AdventureCursor.instance.getMonoCursorSprite.ChangeCursorSprite(gameObject.tag);

        getGlowObjectComponent.HigligtON(false);
    }

    public override void OnHover()
    {
        if(MONO_AdventureCursor.instance.getMonoCursorSprite.getCurrentTag != gameObject.tag)
        {
            MONO_AdventureCursor.instance.getMonoCursorSprite.ChangeCursorSprite(gameObject.tag);
        }
    }

    public override void OnHoverExit()
    {
        MONO_AdventureCursor.instance.getMonoCursorSprite.setDefultCursor();

        getGlowObjectComponent.HigligtOFF(false);
    }




}
