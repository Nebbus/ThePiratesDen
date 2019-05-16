using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MONO_CursorSprite : MonoBehaviour {

	public Image cursorSpriteImage;
	[SerializeField]
	private string currentTag;

	//public SpriteRenderer cursorSpriteRenderer;
	[Space]
	public cursorSprite[] cursorSprites = new cursorSprite[0];


	/// <summary>
	/// Compares the tag of object being hovered over with the tag of the cursorSprite. 
	/// If they match, the sprite of the cursor changes to the sprite of the cursorSprite.
	/// </summary>
	/// <param name="objectTag">Tag of the object currently being hovered over.</param>
	private bool ChangeCursorSprite(string objectTag)
	{
		for (int i = 0; i < cursorSprites.Length; i++) 
		{
			if (cursorSprites [i].tag == objectTag) 
			{
				cursorSpriteImage.sprite = cursorSprites [i].sprite;
				currentTag = objectTag;
				return true;
			}

		}
		Debug.LogError ("No sprite with matching tag found");
		return false;
	}


	/// <summary>
	/// Get current tag. Returns tag of object currently being hovered over.
	/// </summary>
	/// <value>Current tag of object hovered being hovered over.</value>
	public string getTag
	{
		get
		{ 
			return currentTag;
		}

	}
}

[System.Serializable]
public class cursorSprite 
{
	public string tag;
	public Sprite sprite;
}
