using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MONO_CursorSprite : MonoBehaviour {

	public Image cursorSpriteImage;
	[SerializeField]
	private string currentTag;

    public string getCurrentTag
    {
        get
        {
            return currentTag;
        }
    }



    public string[] cursorTag    = new string[numberOfCursorMods];
    public Sprite[] cursorSprits = new Sprite[numberOfCursorMods];

    public static int numberOfCursorMods = 0;

    /// <summary>
    /// Compares the tag of object being hovered over with the tag of the cursorSprite. 
    /// If they match, the sprite of the cursor changes to the sprite of the cursorSprite.
    /// </summary>
    /// <param name="objectTag">Tag of the object currently being hovered over.</param>
    public bool ChangeCursorSprite(string objectTag)
    {
        for (int i = 0; i < cursorTag.Length; i++)
        {
            if (cursorTag[i] == objectTag)
            {
                cursorSpriteImage.sprite = cursorSprits[i];
                currentTag = objectTag;
                return true;
            }
        }
        Debug.LogError("No sprite with matching tag found");
        return false;
    }


    /// <summary>
    /// Sets the defult (nonInteractable) cursotr
    /// </summary>
    public void setDefultCursor()
    {
      
        cursorSpriteImage.sprite = cursorSprits[0];
        currentTag = cursorTag[0];
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

//[System.Serializable]
//public class cursorSprite 
//{
//	public string tag;
//	public Sprite sprite;
//}
