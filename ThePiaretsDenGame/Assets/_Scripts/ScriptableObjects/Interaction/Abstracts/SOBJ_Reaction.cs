using UnityEngine;

// This is the base class for all Reactions.
// There are arrays of inheriting Reactions on ReactionCollections.
public abstract class SOBJ_Reaction : ScriptableObject
{
    /// <summary>
    /// This is called from ReactionCollection.
    /// This function contains everything that is required to be done for all
    /// Reactions as well as call the SpecificInit of the inheriting Reaction.
    /// </summary>
    public void Init()
    {
        SpecificInit();
    }

    /// <summary>
    /// This function is virtual so that it can be overridden and used purely
    /// for the needs of the inheriting class.
    /// </summary>
    protected virtual void SpecificInit()
    { }


    /// <summary>
    /// This is called from ReactionCollection.
    /// This function contains everything that is required to be done for all
    /// Reactions as well as call the SpecificOnAppQuit of the inheriting Reaction.
    /// </summary>
    public void OnAppQuit()
    {
        SpecificOnAppQuit();
    }

    /// <summary>
    /// This function is virtual so that it can be overridden and used purely
    /// for the needs of the inheriting class.
    /// </summary>
    protected virtual void SpecificOnAppQuit()
    { }

    /// <summary>
    /// This function is called from ReactionCollection.
    /// It contains everything that is required for all for all Reactions as
    /// well as the part of the Reaction which needs to happen immediately.
    /// </summary>
    /// <param name="monoBehaviour"> MONO_ReactionCollection that called this 
    ///  funktions  </param>
    public void React(MonoBehaviour monoBehaviour)
    {
        ImmediateReaction();
    }

    /// <summary>
    /// This is the core of the Reaction and must 
    /// be overridden to make things happpen.
    /// </summary>
    protected abstract void ImmediateReaction();
}
