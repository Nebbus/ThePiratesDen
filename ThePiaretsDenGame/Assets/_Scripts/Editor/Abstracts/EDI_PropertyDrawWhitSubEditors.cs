using UnityEngine;
using UnityEditor;

/*
* This is identical to EDI_EditorWithSubEditors but made for 
* Classes with propertyDraws
*/
/*
* This class acts as a base class for Editors that have Editors
* nested within them.  For example, the InteractableEditor has
* an array of ConditionCollectionEditors.
* It's generic types represent the type of Editor array that are
* nested within this Editor and the target type of those Editors.
*/

public abstract class EDI_PropertyDrawWhitSubEditors<TEditorOne, TEditorTwo, TTargetOne, TTargetTwo> : PropertyDrawer
    where TEditorOne : Editor
    where TTargetOne : Object
    where TEditorTwo : Editor
    where TTargetTwo : Object
{
    protected TEditorOne[] subEditorsOne; // Array of Editors nested within this Editor.
    protected TEditorTwo[] subEditorsTwo; // Array of Editors nested within this Editor.
    /// <summary>
    /// This should be called in OnEnable and at the start of OnInspectorGUI.
    /// </summary>
    /// <param name="subEditorTargets"> The scripts that will be edited</param>
    protected void CheckAndCreateSubEditors(TTargetOne[] subEditorTargetsOne)
    {
        // If there are the correct number of subEditors then do nothing.
        if (subEditorsOne != null && subEditorsOne.Length == subEditorTargetsOne.Length)
        {
            return;
        }
        // Otherwise get rid of the editors.
        CleanupEditorsOne();

        // Create an array of the subEditor type that is the right length for the targets.
        subEditorsOne = new TEditorOne[subEditorTargetsOne.Length];

        // Populate the array and setup each Editor.
        for (int i = 0; i < subEditorsOne.Length; i++)
        {
            subEditorsOne[i] = Editor.CreateEditor(subEditorTargetsOne[i]) as TEditorOne;
            SubEditorSetup(subEditorsOne[i]);
        }
    }

    /// <summary>
    /// This should be called in OnEnable and at the start of OnInspectorGUI.
    /// </summary>
    /// <param name="subEditorTargets"> The scripts that will be edited</param>
    protected void CheckAndCreateSubEditors(TTargetTwo[] subEditorTargetsTwo)
    {
        // If there are the correct number of subEditors then do nothing.
        if (subEditorsTwo != null && subEditorsTwo.Length == subEditorTargetsTwo.Length)
        {
            return;
        }
        // Otherwise get rid of the editors.
        CleanupEditorsTwo();

        // Create an array of the subEditor type that is the right length for the targets.
        subEditorsTwo = new TEditorTwo[subEditorTargetsTwo.Length];

        // Populate the array and setup each Editor.
        for (int i = 0; i < subEditorsTwo.Length; i++)
        {
            subEditorsTwo[i] = Editor.CreateEditor(subEditorTargetsTwo[i]) as TEditorTwo;
            SubEditorSetup(subEditorsTwo[i]);
        }
    }

    /// <summary>
    /// This should be called in OnDisable.
    /// </summary>
    protected void CleanupEditorsOne()
    {
        // If there are no subEditors do nothing.
        if (subEditorsOne == null)
        {
            return;
        }

        // Otherwise destroy all the subEditors.
        for (int i = 0; i < subEditorsOne.Length; i++)
        {
            Editor.DestroyImmediate(subEditorsOne[i]);
        }

        // Null the array so it's GCed.
        subEditorsOne = null;
    }
    /// <summary>
    /// This should be called in OnDisable.
    /// </summary>
    protected void CleanupEditorsTwo()
    {
        // If there are no subEditors do nothing.
        if (subEditorsTwo == null)
        {
            return;
        }

        // Otherwise destroy all the subEditors.
        for (int i = 0; i < subEditorsTwo.Length; i++)
        {
            Editor.DestroyImmediate(subEditorsTwo[i]);
        }

        // Null the array so it's GCed.
        subEditorsTwo = null;
    }


    /// <summary>
    /// This must be overridden to provide any setup the subEditorOne needs when it is first created.
    /// </summary>
    /// <param name="editor"> Editor to be setup</param>
    protected abstract void SubEditorSetup(TEditorOne editor);
    /// <summary>
    /// This must be overridden to provide any setup the subEditorTwo needs when it is first created.
    /// </summary>
    /// <param name="editor"> Editor to be setup</param>
    protected abstract void SubEditorSetup(TEditorTwo editor);
}
