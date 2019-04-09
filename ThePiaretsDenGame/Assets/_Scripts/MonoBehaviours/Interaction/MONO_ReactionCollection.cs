using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MONO_ReactionCollection : MonoBehaviour
{
    public Reaction[] reactions = new Reaction[0];      // Array of all the Reactions to play when React is called.


    private void Start()
    {
        // Go through all the Reactions and call their Init function.
        for (int i = 0; i < reactions.Length; i++)
        {
            // The DelayedReaction 'hides' the Reaction's Init function with it's own.
            // This means that we have to try to cast the Reaction to a DelayedReaction and then if it exists call it's Init function.
            // Note that this mainly done to demonstrate hiding and not especially for functionality.
            DelayedReaction delayedReaction = reactions[i] as DelayedReaction;

            if (delayedReaction)
                delayedReaction.Init();
            else
                reactions[i].Init();
        }
    }


    public void React()
    {
        // Go through all the Reactions and call their React function.
        for (int i = 0; i < reactions.Length; i++)
        {
            // The DelayedReaction hides the Reaction.React function.
            // Note again this is mainly done for demonstration purposes.
            DelayedReaction delayedReaction = reactions[i] as DelayedReaction;

            if (delayedReaction)
                delayedReaction.React(this);
            else
                reactions[i].React(this);
        }
    }
}
