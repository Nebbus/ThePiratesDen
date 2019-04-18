using UnityEngine;

/* This is one of the core features of the game.
*  Each one acts like a hub for all things that transpire
*  over the course of the game.
*  The script must be on a gameobject with a collider and
*  an event trigger.  The event trigger should tell the
*  player to approach the interactionLocation and the 
*  player should call the Interact function when they arrive.
*/
public class MONO_Interactable : MonoBehaviour
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
                Debug.Log("ON MOUSE  " + MONO_itemGradFromTheInventory.instance.currentItem);
                return;
            }

        }

        // If none of the reactions happened, use the default MONO_ReactionCollection.
        defaultReactionCollection.React();
    }
}
