using System;
using UnityEngine;
using UnityEditor;

public abstract class EDI_Reaction : Editor
{
    public bool showReaction;                       // Is the Reaction editor expanded?
    public SerializedProperty reactionsProperty;    // Represents the SerializedProperty of the array the target belongs to.

    public EDI_ItemInteractable2 parentEditor = null;  // not null if the reaction is in a itemInteractable


    private SOBJ_Reaction reaction;  // The target Reaction


    private const float buttonWidth = 30f;          // Width in pixels of the button to remove this Reaction from the ReactionCollection array.


    private void OnEnable()
    {
        // Cache the target reference.
        reaction = (SOBJ_Reaction)target;
        if (target == null)
        {
            DestroyImmediate(this);
        }

        // Call an initialisation method for inheriting classes.
        Init();
    }

    /// <summary>
    /// This function should be overridden by 
    /// inheriting classes that need initialisation.
    /// </summary>
    protected virtual void Init() { }


    public override void OnInspectorGUI()
    {
        // Pull data from the target into the serializedObject.
        serializedObject.Update();

        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;

        EditorGUILayout.BeginHorizontal();

        // Display a foldout for the Reaction with a custom label.
        showReaction = EditorGUILayout.Foldout(showReaction, GetFoldoutLabel(), true);
       

        /* Show a button which, if clicked, will remove this 
         * Reaction from the ReactionCollection.
         */
        bool destroyMe = GUILayout.Button("-", GUILayout.Width(buttonWidth));
        
        EditorGUILayout.EndHorizontal();

        /* If the foldout is open, draw the GUI specific 
         * to the inheriting ReactionEditor.
         */ 
        if (showReaction)
        {
            DrawReaction();
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();
        
        //Removes this object her to avoid errer from the line above
        if (destroyMe)
        {
            if (parentEditor != null)
            {
                voidEdiorItemRemove();
            }
            else
            {
                reactionsProperty.RemoveFromObjectArray(reaction);

            }


        }

    }

    /// <summary>
    /// used by the items to remove this item
    /// </summary>
    public void voidEdiorItemRemove()
    {
        parentEditor.RemoveReaction(reaction);

    }


    public static SOBJ_Reaction CreateReaction(Type reactionType)
    {
        // Create a reaction of a given type.
        return (SOBJ_Reaction)CreateInstance(reactionType);
    }


    protected virtual void DrawReaction()
    {
        /* This function can overridden by inheriting classes, 
         * but if it isn't, draw the default for it's properties.
         */ 
        DrawDefaultInspector();
    }


    /* The inheriting class must override this function to 
     * create the label of the foldout.
     */ 
    protected abstract string GetFoldoutLabel();
}
