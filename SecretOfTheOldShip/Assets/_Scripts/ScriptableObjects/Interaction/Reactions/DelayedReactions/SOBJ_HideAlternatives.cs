using UnityEngine;

public class SOBJ_HideAlternatives : SOBJ_DelayedReaction
{
	public GameObject rootReaction;
    public bool handelInputThenDon = false;

	protected override void ImmediateReaction()
	{
		/*Find the root defaultreaction that holds the ShowAlterantives script and close the
		 * interactionalternatives, then unlock the player.
		 */
		rootReaction.GetComponent<MONO_ShowAlternatives> ().HideAlternatives ();
		FindObjectOfType<MONO_SceneManager> ().getSetHandleInput = handelInputThenDon;
	}
}

