using UnityEngine;
using UnityEditor;

/* This is the Editor for the Interactable MonoBehaviour.
 * However, since the Interactable contains many sub-objects, 
 * it requires many sub-editors to display them.
 * For more details see the EditorWithSubEditors class.
 */
[CustomEditor(typeof(MONO_Interactable))]
public class EDI_Interactable : EDI_EditorWithSubEditors<EDI_ConditionCollection, SOBJ_ConditionCollection>
{
    private MONO_Interactable   interactable;                        // Reference to the target.
    private SerializedProperty  conditionCollectionsProperty;         // Represents the Transform which is where the player walks to in order to Interact with the Interactable.
    private SerializedProperty  collectionsProperty;                 // Represents the ConditionCollection array on the Interactable.
    private SerializedProperty  defaultReactionCollectionProperty;   // Represents the ReactionCollection which is used if none of the ConditionCollections are.


    private const float collectionButtonWidth = 125f;               // Width in pixels of the button for adding to the ConditionCollection array.
    /* Name of the Transform field for where the player walks 
     * to in order to Interact with the Interactable.
     */ 
    private const string interactablePropInteractionLocationName  = "interactionLocation";
    private const string conditionCollectionsPropertyName         = "conditionCollections";
    private const string defaultReactionCollectionName            = "defaultReactionCollection";

    private float space = 0;

    private void OnEnable()
    {
        // Cache the target reference.
        interactable = (MONO_Interactable)target;

        // Cache the SerializedProperties.
        collectionsProperty                 = serializedObject.FindProperty(conditionCollectionsPropertyName);
        conditionCollectionsProperty        = serializedObject.FindProperty(interactablePropInteractionLocationName);
        defaultReactionCollectionProperty   = serializedObject.FindProperty(defaultReactionCollectionName);

        // Create the necessary Editors for the ConditionCollections.
        CheckAndCreateSubEditors(interactable.conditionCollections);
    }


    private void OnDisable()
    {
        /* When the InteractableEditor is disabled,
         * destroy all the ConditionCollection editors.
         */ 
        CleanupEditors();
    }

    /// <summary>
    /// This is called when the ConditionCollection editors are created.
    /// </summary>
    /// <param name="editor">editor what will get a reference 
    /// to the array to which it belongs.</param>
    protected override void SubEditorSetup(EDI_ConditionCollection editor)
    {
        /* Give the ConditionCollection editor a 
         * reference to the array to which it belongs.
         */ 
        editor.collectionsProperty = collectionsProperty;
    }

   



    public override void OnInspectorGUI()
    {

        space = EditorGUIUtility.currentViewWidth / 1.2f;

        // Pull information from the target into the serializedObject.
        serializedObject.Update();

        // If necessary, create editors for the ConditionCollections.
        CheckAndCreateSubEditors(interactable.conditionCollections);

        // Use the default object field GUI for the interactionLocation.
        EditorGUILayout.PropertyField(conditionCollectionsProperty);

        // Display all of the ConditionCollections.
        for (int i = 0; i < subEditors.Length; i++)
        {
            subEditors[i].OnInspectorGUI();
            EditorGUILayout.Space();
        }

        /* Create a right-aligned button which when clicked,
         * creates a new ConditionCollection in the
         * ConditionCollections array.
         */ 
        EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Add Collection", GUILayout.Width(collectionButtonWidth)))
            {
                SOBJ_ConditionCollection newCollection = EDI_ConditionCollection.CreateConditionCollection();
                collectionsProperty.AddToObjectArray(newCollection);
            }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        // Use the default object field GUI for the defaultReaction.
        EditorGUILayout.PropertyField(defaultReactionCollectionProperty);

        /* Push information back to the target from the
         * serializedObject.
         */ 
        serializedObject.ApplyModifiedProperties();
    }
}