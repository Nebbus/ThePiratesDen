using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;


[CustomEditor(typeof(MONO_CursorSprite))]
public class EDI_test :  Editor{

    private bool[] showItemSlosts = new bool[MONO_CursorSprite.numberOfCursorMods];
    private int[] selectedTags    = new int[MONO_CursorSprite.numberOfCursorMods];

    private SerializedProperty cursorSpriteImageProperty;
    private SerializedProperty cursorTagProperty;
    private SerializedProperty cursorSpritsProperty;
    private SerializedProperty cursorTagHolderProperty;


    private const string cursorSpriteImagePropertyName  = "cursorSpriteImage";
    private const string cursorTagPropertyName          = "cursorTag";
    private const string cursorSpritsPropertyName       = "cursorSprits";
    private const string cursorTagHolderPropertyName    = "cursorTagHolder";




    private MONO_CursorSprite monoCursorSprite;

    private float buttonWhidt = 30f;

    private bool addSlot = false;
    private bool remobeSlot = false;


    //===========================================
    // Drop down menu variables
    //===========================================
    private readonly float verticalSpacing = EditorGUIUtility.standardVerticalSpacing;  // Caching the vertical spacing between GUI elements.
    private const float dropAreaHeight = 50f;                                      // Height in pixels of the area for dropping scripts.
    private const float controlSpacing = 5f;                                       // Width in pixels between the popup type selection and drop area.
    private const float buttonWidth = 30f;

    private string[] getTags
    {
        get
        {
            return UnityEditorInternal.InternalEditorUtility.tags;
        }
    }

    private void OnEnable()
    {

        monoCursorSprite = (MONO_CursorSprite)target;

        cursorSpriteImageProperty   = serializedObject.FindProperty(cursorSpriteImagePropertyName);
        cursorTagProperty           = serializedObject.FindProperty(cursorTagPropertyName);
        cursorSpritsProperty        = serializedObject.FindProperty(cursorSpritsPropertyName);
        cursorTagHolderProperty     = serializedObject.FindProperty(cursorTagHolderPropertyName);

        if (MONO_CursorSprite.numberOfCursorMods != monoCursorSprite.cursorSprits.Length)
        {
            MONO_CursorSprite.numberOfCursorMods = monoCursorSprite.cursorSprits.Length;
            showItemSlosts = new bool[MONO_CursorSprite.numberOfCursorMods];

        }

    }

    private void uppdateIndexList()
    {
        int[] temp =  new int[MONO_CursorSprite.numberOfCursorMods];
        if(selectedTags.Length < MONO_CursorSprite.numberOfCursorMods)
        {
            for (int i = 0; i < selectedTags.Length; i++)
            {
                temp[i] = selectedTags[i];
            }
            selectedTags = temp;
        }
        else if(selectedTags.Length > MONO_CursorSprite.numberOfCursorMods)
        {
            for (int i = 0; i < MONO_CursorSprite.numberOfCursorMods; i++)
            {
                temp[i] = selectedTags[i];
            }
            selectedTags = temp;
        }
    }

    public override void OnInspectorGUI()
    {

        serializedObject.Update();

        EditorGUILayout.PropertyField(cursorSpriteImageProperty);

        // saftety uppdate. 
        if (MONO_CursorSprite.numberOfCursorMods != monoCursorSprite.cursorSprits.Length)
        {
            MONO_CursorSprite.numberOfCursorMods = monoCursorSprite.cursorSprits.Length;
            showItemSlosts = new bool[MONO_CursorSprite.numberOfCursorMods];
            uppdateIndexList();
        }


        if (addSlot)
        {
            addSlot = false;
            AddInvetorySlotV2();
        }

        if (remobeSlot)
        {
            remobeSlot = false;
            RemoveInventorySlotV2();
        }

        //Saftety Catch
        if (showItemSlosts.Length != MONO_CursorSprite.numberOfCursorMods)
        {
            showItemSlosts = new bool[MONO_CursorSprite.numberOfCursorMods];
        }
        if (selectedTags.Length != MONO_CursorSprite.numberOfCursorMods)
        {
            uppdateIndexList();
        }


        // The buttosn for adding and removin item slots in the invetory
        EditorGUILayout.BeginHorizontal();
            addSlot     = GUILayout.Button("+", GUILayout.Width(buttonWhidt));
            remobeSlot = GUILayout.Button("-", GUILayout.Width(buttonWhidt));
        EditorGUILayout.EndHorizontal();

        //Displays all currsor mods
        for (int i = 0; i < MONO_CursorSprite.numberOfCursorMods; i++)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
                EditorGUI.indentLevel++;
                showItemSlosts[i] = EditorGUILayout.Foldout(showItemSlosts[i], cursorTagProperty.GetArrayElementAtIndex(i).stringValue);
                if (showItemSlosts[i])
                {
                    TagSelection(i);
                    displaysImage(i);
                }
                EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }

        serializedObject.ApplyModifiedProperties();


    }


    /// <summary>
    /// Displays the cursors 
    /// </summary>
    /// <param name="index">curent cursor sprite index<param>
    private void displaysImage(int index)
    {
        EditorGUILayout.PropertyField(cursorSpritsProperty.GetArrayElementAtIndex(index));
        if (monoCursorSprite.cursorSprits[index] != null)
        {
            GUILayout.Box(monoCursorSprite.cursorSprits[index].texture);
        }

    }


    /// <summary>
    /// Displays the selected tag 
    /// </summary>
    /// <param name="index"> curent selected tag</param>
    private void TagSelection(int index)
    {
        Rect fullWidthRect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(dropAreaHeight));

        Rect leftAreaRect = fullWidthRect;

        // It should be in half a space from the top.
        leftAreaRect.y += verticalSpacing * 0.5f;

        /* The width should be slightly less than half 
         * the width of the inspector.
         */
        leftAreaRect.width *= 0.5f;
        leftAreaRect.width -= controlSpacing * 0.5f;

        // The height should be the same as the drop area.
        leftAreaRect.height = dropAreaHeight;

        // Create Rects for the top and .
        Rect topHalf = leftAreaRect;
        topHalf.height *= 0.5f;
        selectedTags[index] = EditorGUI.Popup(topHalf, selectedTags[index], getTags);

        cursorTagProperty.GetArrayElementAtIndex(index).stringValue = getTags[selectedTags[index]];
    }


    /// <summary>
    /// Function that Add muse type slot
    /// </summary>
    private void AddInvetorySlotV2()
    {
        MONO_CursorSprite.numberOfCursorMods++;

        // Pull all the information from the target of the serializedObject.
        cursorTagProperty.serializedObject.Update();

        /* Add a null array element to the end of the array
         * then populate it with the object parameter.
         */
        cursorTagProperty.InsertArrayElementAtIndex(cursorTagProperty.arraySize);
        cursorTagProperty.GetArrayElementAtIndex(cursorTagProperty.arraySize - 1).stringValue = getTags[0];
        cursorTagProperty.serializedObject.ApplyModifiedProperties();

        EXT_SerializedProperty.AddToObjectArray<Sprite>(cursorSpritsProperty, null);

    }

    /// <summary>
    /// Function that removes a muse type sloty
    /// </summary>
    private void RemoveInventorySlotV2()
    {
        MONO_CursorSprite.numberOfCursorMods--;

        EXT_SerializedProperty.RemoveFromObjectArrayAt(cursorSpritsProperty, MONO_CursorSprite.numberOfCursorMods);     

        //REMOVES THE TAG DESCRIPTION
        // Pull all the information from the target of the serializedObject.
        cursorTagProperty.serializedObject.Update();

        // If there is a non-null element at the index, null it.
        if (cursorTagProperty.GetArrayElementAtIndex(MONO_CursorSprite.numberOfCursorMods).stringValue == null)
        {
            cursorTagProperty.DeleteArrayElementAtIndex(MONO_CursorSprite.numberOfCursorMods);
        }


        // Delete the null element from the array at the index.
        cursorTagProperty.DeleteArrayElementAtIndex(MONO_CursorSprite.numberOfCursorMods);

        // Push all the information on the serializedObject back to the target.
        cursorTagProperty.serializedObject.ApplyModifiedProperties();



        showItemSlosts = new bool[MONO_CursorSprite.numberOfCursorMods]; 
        uppdateIndexList();
    }


  






}
