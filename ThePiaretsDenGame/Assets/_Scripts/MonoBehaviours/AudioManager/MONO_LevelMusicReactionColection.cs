using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MONO_LevelMusicReactionColection : MonoBehaviour
{
    // Array of all the Reactions to play when React is called.
    public SOBJ_FMODreaction[] FMODreactions = new SOBJ_FMODreaction[0];


    private void Start()
    {
        // Go through all the Reactions and call their Init function.
        for (int i = 0; i < FMODreactions.Length; i++)
        {
            /*The DelayedReaction 'hides' the Reaction's Init function with it's own.
             * This means that we have to try to cast the Reaction to a DelayedReaction
             * and then if it exists call it's Init function. Note that this mainly
             * done to demonstrate hiding and not especially for functionality.
             */
            SOBJ_DelayedReaction delayedReaction = FMODreactions[i] as SOBJ_DelayedReaction;

            if (delayedReaction)
            {
                delayedReaction.Init();
            }
            else
            {
                FMODreactions[i].Init();
            }

        }
    }

    public void React()
    {
        // Go through all the Reactions and call their React function.
        for (int i = 0; i < FMODreactions.Length; i++)
        {
            // The DelayedReaction hides the Reaction.React function.
            // Note again this is mainly done for demonstration purposes.
            SOBJ_DelayedReaction delayedReaction = FMODreactions[i] as SOBJ_DelayedReaction;

            if (delayedReaction)
            {
                delayedReaction.React(this);
            }
            else
            {
                FMODreactions[i].React(this);
            }

        }
    }
}
