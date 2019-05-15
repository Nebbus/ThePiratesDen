using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MONO_CursorSprite : MonoBehaviour {


	public SpriteRenderer cursorSpriteRenderer;
	public cursorSprite[] cursorSprites = new cursorSprite[0];

	/// <summary>
	/// Compares the tag of object being hovered over with the tag of the cursorSprite. 
	/// If they match, the sprite of the cursor changes to the sprite of the cursorSprite.
	/// </summary>
	/// <param name="objectTag">Tag of the object currently being hovered over.</param>
	private void ChangeCursorSprite(string objectTag)
	{
		for (int i = 0; i < cursorSprites.Length; i++) 
		{
			if (cursorSprites [i].tag == objectTag) 
			{
				cursorSpriteRenderer.sprite = cursorSprites [i].sprite;
				return;
			}

		}
		Debug.LogError ("No sprite with matching tag found");
	}
}

public class cursorSprite 
{
	public string tag;
	public Sprite sprite;
}
