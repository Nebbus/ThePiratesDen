using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(MONO_OnSceneStart_Object))]
public class EDI_OnSceneStart_Object : EDI_EditorWithSubEditors<EDI_ConditionAdvanced, SOBJ_ConditionAdvanced>
{

    private MONO_OnSceneStart_Object monoOnSceneSterObject;         // Reference to the target.

    private SerializedProperty descriptionProperty;         // Represents a string description for the target.
    private SerializedProperty conditionsProperty;          // Represents an array of Conditions for the target.
    private SerializedProperty reactionCollectionProperty;  // Represents the ReactionCollection that is referenced by the target.
    private SerializedProperty setGamobjectToProperty;

    private const float conditionButtonWidth = 30f;             // Width of the button for adding a new Condition.
    private const float collectionButtonWidth = 125f;           // Width of the button for removing the target from it's Interactable.


    private const string descriptionPropertyName        = "description";
    private const string conditionsPropertyName         = "conditiosns";
    private const string reactionCollectionPropertyName = "reactionCollection";
    private const string setGamobjectToPropertyName     = "setGamobjectTo";


    // Caching the vertical spacing between GUI elements.
    private readonly float verticalSpacing = EditorGUIUtility.standardVerticalSpacing;


    private Type[]      conditionTypes;          // SOBJ_conditions in this case
    private string[]    conditionTypeNames;      // The names of all appropriate SOBJ_ConditionAdvanced types.
    private int         selectedIndex;           // The index of the currently selected SOBJ_ConditionAdvanced type.


    private const float dropAreaHeight = 50f;  // Height in pixels of the area for dropping scripts.
    private const float controlSpacing = 5f;   // Width in pixels between the popup type selection and drop area.
    private const float buttonWidth = 30f;  // Width in pixels of the button to create Conditions.

    public float space = 0;
    private void OnEnable()
    {
        // Cache a reference to the target.
        monoOnSceneSterObject = (MONO_OnSceneStart_Object)target;

        // If this Editor exists but isn't targeting anything destroy it.
        if (target == null)
        {
            DestroyImmediate(this);
            return;
        }

        // Cache the SerializedProperties.
        descriptionProperty        = serializedObject.FindProperty(descriptionPropertyName);
        conditionsProperty         = serializedObject.FindProperty(conditionsPropertyName);
        reactionCollectionProperty = serializedObject.FindProperty(reactionCollectionPropertyName);
        setGamobjectToProperty     = serializedObject.FindProperty(setGamobjectToPropertyName);

        // Check if the Editors for the Conditions need creating and optionally create them.
        CheckAndCreateSubEditors(monoOnSceneSterObject.conditiosns);
        EXT_GetListOfScriptableObjects.SetGenericNamesArray(typeof(SOBJ_ConditionAdvanced), out conditionTypes, out conditionTypeNames);
    }


    private void OnDisable()
    {
        // When this Editor ends, destroy all it's subEditors.
        CleanupEditors();
    }


    // This is called immediately when a subEditor is created.
    protected override void SubEditorSetup(EDI_ConditionAdvanced editor)
    {
        // Set the editor type so that the correct GUI for Condition is shown.
        editor.editorType = EDI_ConditionAdvanced.EditorType.ConditionCollection;

        /* Assign the conditions property so that the 
         * ConditionEditor can remove its target if necessary.
         */
        editor.conditionsProperty = conditionsProperty;
    }


    public override void OnInspectorGUI()
    {

        // Pull the information from the target into the serializedObject.
        serializedObject.Update();

        /* Check if the Editors for the Conditions need 
         * creating and optionally create them.
         */
        CheckAndCreateSubEditors(monoOnSceneSterObject.conditiosns);

        EditorGUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(space));
            EditorGUI.indentLevel++;

            EditorGUILayout.BeginHorizontal(GUILayout.Width(space));

            /* Use the isExpanded bool for the descriptionProperty 
             * to store whether the foldout is open or closed.
             */
            descriptionProperty.isExpanded = EditorGUILayout.Foldout(descriptionProperty.isExpanded, descriptionProperty.stringValue);

    
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.HelpBox("OBS DO NOT FORGET TO ADD THIS TO THE " +
                                    "'To Run On Scene Start' EVENT IN THE SCENES " +
                                    "MONO_OnSceneStar SCRIPT!!!", MessageType.Warning);
            EditorGUILayout.Space();
           ExpandedGUI();
        
           EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();

        // Push all changes made on the serializedObject back to the target.
        serializedObject.ApplyModifiedProperties();
    }


    private void ExpandedGUI()
    {
        EditorGUILayout.Space();

        // Display the description for editing.
        EditorGUILayout.PropertyField(descriptionProperty);

        EditorGUILayout.Space();

        /* Display the Labels for the Conditions evenly split over
         * the width of the inspector.
         */
        float space2 = EditorGUIUtility.currentViewWidth / 3f;

        EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Condition", GUILayout.Width(space2));
            EditorGUILayout.LabelField("Satisfied?", GUILayout.Width(space2));
            EditorGUILayout.LabelField("Add/Remove", GUILayout.Width(space2));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginVertical(GUI.skin.box);
            // Display each of the Conditions.
            EditorGUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(space2));
                for (int i = 0; i < subEditors.Length; i++)
                {
                    EditorGUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(space2));
                        EditorGUILayout.Space();
                        subEditors[i].OnInspectorGUI();
                    EditorGUILayout.EndVertical();
                }
            EditorGUILayout.EndVertical();

            TypeSelectionGUI();
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.BeginVertical(GUI.skin.box);
            // Display the reference to the ReactionCollection for editing.
            EditorGUILayout.PropertyField(reactionCollectionProperty);

            if(monoOnSceneSterObject.reactionCollection == null)
            {
                EditorGUILayout.HelpBox("Will then all conditions is true enable/disavle " +
                                        "this gamobject, if mor elaborate reactions is wanted" +
                                        " add reaction collection ass you dose thit the MONO_Interactable",MessageType.Info);
                EditorGUILayout.PropertyField(setGamobjectToProperty);

            }
            else
            {
                EditorGUILayout.HelpBox("this is the mor elaborate on start reactions, " +
                                        "the enable/disable is not don by defult then a " +
                                        "reaction collection is shosed, it mus be dun in " +
                                        "that reactioncolection if it is wanted. If enavle/disavle " +
                                        "is the only wanted raction: do just set reactionCollection to null", MessageType.Info);
            }
        EditorGUILayout.EndVertical();

    }


    private void TypeSelectionGUI()
    {
        Rect fullWidthRect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(dropAreaHeight + verticalSpacing));

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


        // Create Rects for the top and bottom half.
        Rect topHalf = leftAreaRect;
        topHalf.height *= 0.5f;
        Rect bottomHalf = topHalf;
        bottomHalf.y += bottomHalf.height;

        // Display a popup in the top half showing all the reaction types.
        selectedIndex = EditorGUI.Popup(topHalf, selectedIndex, conditionTypeNames);

        // Display a button in the bottom half that if clicked...
        if (GUI.Button(bottomHalf, "Add Selected Condition type"))
        {
            // ... finds the type selected by the popup, creates an appropriate reaction and adds it to the array.
            Type reactionType = conditionTypes[selectedIndex];
            SOBJ_ConditionAdvanced newCondition = EDI_ConditionAdvanced.CreateCondition(reactionType);
            conditionsProperty.AddToObjectArray(newCondition);
        }
    }
}
