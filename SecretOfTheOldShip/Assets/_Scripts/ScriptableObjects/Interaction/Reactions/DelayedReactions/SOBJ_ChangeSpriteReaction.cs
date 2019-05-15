using UnityEngine;

public class SOBJ_ChangeSpriteReaction : SOBJ_DelayedReaction
{
	public SpriteRenderer spriteToBeChanged;
	public Sprite newSprite;

	protected override void ImmediateReaction()
	{
		spriteToBeChanged.sprite = newSprite;
	}
}

