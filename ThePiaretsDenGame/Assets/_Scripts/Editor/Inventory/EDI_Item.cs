using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(SOBJ_Item))]
[CanEditMultipleObjects]
public class EDI_Item : Editor {

    //The target item

    private SerializedProperty spriteProperty;
    private SerializedProperty combindsWithProperty;
    private SerializedProperty toMakeProperty;
    private SerializedProperty conditioAndReactionsProperty;

    private const string spritePropertyName               = "sprite";
    private const string combindWhitPropertyName          = "combindsWith";
    private const string toMakePropertyName               = "toMake";
    private const string conditioAndReactionsPropertyName = "conditioAndReactions";

    private SOBJ_Item sobjItem;

    private const float ownSpriteWidth = 1 / 3f;
    private const float ownSpriteHight = 1 / 3f;
    private const float spacing        = 1 / 5f;

    private void OnEnable()
    {
         sobjItem = (SOBJ_Item)target;

        spriteProperty               = serializedObject.FindProperty(spritePropertyName);
        combindsWithProperty         = serializedObject.FindProperty(combindWhitPropertyName);
        toMakeProperty               = serializedObject.FindProperty(toMakePropertyName);
        conditioAndReactionsProperty = serializedObject.FindProperty(conditioAndReactionsPropertyName);
    }


    //public override void OnInspectorGUI()
    //{
    //    float width = EditorGUIUtility.currentViewWidth;
    //    serializedObject.Update();

    //    CurentItem(width);

    //    EditorGUILayout.BeginHorizontal();
    //    EditorGUILayout.LabelField("Combindes with", GUILayout.Width(width * ownSpriteWidth));
    //    EditorGUILayout.LabelField("To make this", GUILayout.Width(width * ownSpriteWidth));
    //    EditorGUILayout.EndHorizontal();

    //    EditorGUILayout.BeginVertical(GUI.skin.box);
    //      EditorGUILayout.BeginHorizontal(GUI.skin.box);
    //      EditorGUILayout.PropertyField(combindsWithProperty, GUIContent.none, GUILayout.Width(width * ownSpriteWidth));
    //      GUILayout.Space(spacing * width);
    //      EditorGUILayout.PropertyField(toMakeProperty, GUIContent.none, GUILayout.Width(width * ownSpriteWidth));
    //      EditorGUILayout.EndHorizontal();
    //      DrawCombindImages(width);
    //    EditorGUILayout.EndVertical();

    //    //EditorGUILayout.PropertyField(conditioAndReactionsProperty, GUIContent.none);
    //    serializedObject.ApplyModifiedProperties();

    //}

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }



    /// <summary>
    /// draws the image of that will reptrsetn 
    /// this item in the inventory
    /// </summary>
    /// <param name="width"> width of the inventory box</param>
    private void CurentItem(float width)
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.PropertyField(spriteProperty, GUIContent.none, 
                                                      GUILayout.Width(width * ownSpriteWidth));
        if (sobjItem.sprite != null)
        {
          //  GUILayout.Box(sobjItem.sprite.texture, GUILayout.Width(width), GUILayout.Height(width * ownSpriteHight));
            GUILayout.Box(sobjItem.sprite.texture, GUILayout.Width(width * ownSpriteWidth),
                                                   GUILayout.Height(width * ownSpriteHight));
        }

        EditorGUILayout.EndVertical();
    }

  



    private void DrawCombindImages(float width)
    {
        EditorGUILayout.BeginHorizontal(GUI.skin.box);
        if(sobjItem.combindsWith != null)
        {
            GUILayout.Box(sobjItem.combindsWith.sprite.texture, GUILayout.Width(width * ownSpriteWidth), 
                                                                GUILayout.Height(width * ownSpriteHight));
        }
        GUILayout.Space(spacing * width);
        if (sobjItem.toMake != null)
        {
            GUILayout.Box(sobjItem.toMake.sprite.texture, GUILayout.Width( width * ownSpriteWidth),
                                                          GUILayout.Height(width * ownSpriteHight));
        }

        EditorGUILayout.EndHorizontal();
    }

}
