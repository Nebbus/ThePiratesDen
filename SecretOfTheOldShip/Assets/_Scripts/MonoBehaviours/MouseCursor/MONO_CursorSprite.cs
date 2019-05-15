using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MONO_CursorSprite : MonoBehaviour {

	public Image cursorSpriteImage;
	//public SpriteRenderer cursorSpriteRenderer;
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
				cursorSpriteImage.sprite = cursorSprites [i].sprite;
				return;
			}

		}
		Debug.LogError ("No sprite with matching tag found");
	}
}

[System.Serializable]
public class cursorSprite 
{
	public string tag;
	public Sprite sprite;
}
