using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(SOBJ_AllConditions))]
public class EDI_AllConditions : Editor
{
    /// <summary>
    /// Property for accessing the descriptions for all the Conditions.
    /// This is used for the Popups on the ConditionEditor.
    /// </summary>
    public static string[] AllConditionDescriptions
    {
       
        get
        {
            // If the description array doesn't exist yet, set it.
            if (allConditionDescriptions == null)
            {
                SetAllConditionDescriptions();
            }
            return allConditionDescriptions;
        }
        private set { allConditionDescriptions = value; }
    }

    private static string[] allConditionDescriptions;                            // Field to store the descriptions of all the Conditions.


    private EDI_ConditionAdvanced[] conditionEditors;                            // All of the subEditors to display the Conditions.
    private SOBJ_AllConditions      allConditions;                               // Reference to the target.
    private string                  newConditionDescription = "New Condition";   // String to start off the naming of new Conditions.


    private const string creationPath = "Assets/Resources/SOBJ_AllConditions.asset";
    // The path that the AllConditions asset is created at.

    private SerializedProperty conditionsProperty;
    private const string       conditionsPropName = "conditions";
    private Type[]             conditionTypes;                    // All the non-abstract types which inherit from Reaction.  This is used for adding new Reactions.
    private string[]           conditionTypeNames;                // The names of all appropriate Reaction types.
    private int                selectedIndex;                     // The index of the currently selected Reaction type.


    private const float dropAreaHeight = 50f;           // Height in pixels of the area for dropping scripts.
    private const float controlSpacing = 5f;            // Width in pixels between the popup type selection and drop area.
    private const float buttonWidth = 30f;                      // Width in pixels of the button to create Conditions.

    // Caching the vertical spacing between GUI elements.
    private readonly float verticalSpacing = EditorGUIUtility.standardVerticalSpacing;


    private void OnEnable()
    {
        // Cache the reference to the target.
        allConditions = (SOBJ_AllConditions)target;

        // Cache the SerializedProperty
        conditionsProperty = serializedObject.FindProperty(conditionsPropName);


        /* If there aren't any Conditions on the target, 
         * create an empty array of Conditions.
         */
        if (allConditions.conditions == null)
        {
            allConditions.conditions = new SOBJ_ConditionAdvanced[0];
        }


        // If there aren't any editors, create them.
        if (conditionEditors == null)
        {
            CreateEditors();
        }
        // Set the array of types and type names of subtypes of Reaction.
       // SetConditionNamesArray();
        EXT_GetListOfScriptableObjects.SetGenericNamesArray(typeof(SOBJ_ConditionAdvanced),out conditionTypes,out conditionTypeNames);
    }


    private void OnDisable()
    {
        // Destroy all the editors.
        for (int i = 0; i < conditionEditors.Length; i++)
        {
            DestroyImmediate(conditionEditors[i]);
        }

        // Null out the editor array.
        conditionEditors = null;
    }


    private static void SetAllConditionDescriptions()
    {
        /* Create a new array that has the same number 
         * of elements as there are Conditions.
         */ 
        AllConditionDescriptions = new string[TryGetConditionsLength()];

        /* Go through the array and assign the description 
         * of the condition at the same index.
         */ 
        for (int i = 0; i < AllConditionDescriptions.Length; i++)
        {
            AllConditionDescriptions[i] = TryGetConditionAt(i).description;
        }
    }



    public override void OnInspectorGUI()
    {
        /* If there are different number of editors to Conditions,
         * create them afresh.
         */ 
        if (conditionEditors.Length != TryGetConditionsLength())
        {
            // Destroy all the old editors.
            for (int i = 0; i < conditionEditors.Length; i++)
            {
                DestroyImmediate(conditionEditors[i]);
            }

            // Create new editors.
            CreateEditors();
        }

        // Display all the conditions.
        for (int i = 0; i < conditionEditors.Length; i++)
        {
            conditionEditors[i].OnInspectorGUI();
        }

        // If there are conditions, add a gap.
        if (TryGetConditionsLength() > 0)
        {
            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }

        EditorGUILayout.BeginHorizontal();

        // Get and display a string for the name of a new Condition.
        newConditionDescription = EditorGUILayout.TextField(GUIContent.none, newConditionDescription);

        /* Display a button that when clicked adds a new Condition 
         * to the AllConditions asset and resets the new description string.
         */ 
        if (GUILayout.Button("+", GUILayout.Width(buttonWidth)))
        {

            AddCondition(newConditionDescription);
            newConditionDescription = "New Condition";
        }
        EditorGUILayout.EndHorizontal();

        /* Create a Rect for the full width of the
         * inspector with enough height for the drop area.
         */
        Rect fullWidthRect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(dropAreaHeight + verticalSpacing));


        // Create a Rect for the left GUI controls.
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

        // Display the GUI for the type popup and button on the left.
        TypeSelectionGUI(leftAreaRect);
    }

    private void TypeSelectionGUI(Rect containingRect)
    {
        // Create Rects for the top and bottom half.
        Rect topHalf    = containingRect;
        topHalf.height  *= 0.5f;
        Rect bottomHalf = topHalf;
        bottomHalf.y    += bottomHalf.height;

        // Display a popup in the top half showing all the reaction types.
        selectedIndex = EditorGUI.Popup(topHalf, selectedIndex, conditionTypeNames);

    }


    private void CreateEditors()
    {
        // Create a new array for the editors which is the same length at the conditions array.
        conditionEditors = new EDI_ConditionAdvanced[allConditions.conditions.Length];

        // Go through all the empty array...
        for (int i = 0; i < conditionEditors.Length; i++)
        {
            // ... and create an editor with an editor type to display correctly.
            conditionEditors[i]            = CreateEditor(TryGetConditionAt(i)) as EDI_ConditionAdvanced;
            conditionEditors[i].editorType = EDI_ConditionAdvanced.EditorType.AllConditionAsset;
        }
    }



    // Call this function when the menu item is selected.
    [MenuItem("Assets/Create/SOBJ_AllConditions")]
    private static void CreateAllConditionsAsset()
    {
        // If there's already an AllConditions asset, do nothing.
        if (SOBJ_AllConditions.Instance)
        {
            return;
        }


        // Create an instance of the AllConditions object and make an asset for it.
        SOBJ_AllConditions instance = CreateInstance<SOBJ_AllConditions>();
        AssetDatabase.CreateAsset(instance, creationPath);

        // Set this as the singleton instance.
        SOBJ_AllConditions.Instance = instance;

        // Create a new empty array of Conditions.
        instance.conditions = new SOBJ_Condition[0];
    }


    private void AddCondition(string description)
    {
        // If there isn't an AllConditions instance yet, put a message in the console and return.
        if (!SOBJ_AllConditions.Instance)
        {
            Debug.LogError("SOBJ_AllConditions has not been created yet.");
            return;
        }

        // ... finds the type selected by the popup, creates an appropriate reaction and adds it to the array.
        Type conditionType = conditionTypes[selectedIndex];

        // Create a condition based on the description.
        SOBJ_ConditionAdvanced newCondition = EDI_ConditionAdvanced.CreateCondition(description, conditionType);



        // The name is what is displayed by the asset so set that too.
        newCondition.name = description;

        // Record all operations on the newConditions so they can be undone.
        Undo.RecordObject(newCondition, "Created new SOBJ_Condition");

        // Attach the Condition to the AllConditions asset.
        AssetDatabase.AddObjectToAsset(newCondition, SOBJ_AllConditions.Instance);

        // Import the asset so it is recognised as a joined asset.
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(newCondition));

        // Add the Condition to the AllConditions array.
        ArrayUtility.Add(ref SOBJ_AllConditions.Instance.conditions, newCondition);

        // Mark the AllConditions asset as dirty so the editor knows to save changes to it when a project save happens.
        EditorUtility.SetDirty(SOBJ_AllConditions.Instance);

        // Recreate the condition description array with the new added Condition.
        SetAllConditionDescriptions();
    }


    public static void RemoveCondition(SOBJ_ConditionAdvanced condition)
    {
        // If there isn't an AllConditions asset, do nothing.
        if (!SOBJ_AllConditions.Instance)
        {
            Debug.LogError("SOBJ_AllConditions has not been created yet.");
            return;
        }

        /* Record all operations on the AllConditions 
         * asset so they can be undone.
         */ 
        Undo.RecordObject(SOBJ_AllConditions.Instance, "Removing SOBJ_condition");

        // Remove the specified condition from the AllConditions array.
        ArrayUtility.Remove(ref SOBJ_AllConditions.Instance.conditions, condition);

        /* Destroy the condition, including it's asset and 
         * save the assets to recognise the change.
         */
        DestroyImmediate(condition, true);
        AssetDatabase.SaveAssets();

        /* Mark the AllConditions asset as dirty so the editor 
         * knows to save changes to it when a project save happens.
         */ 
        EditorUtility.SetDirty(SOBJ_AllConditions.Instance);

        /* Recreate the condition description array without
         * the removed condition.
         */ 
        SetAllConditionDescriptions();
    }


    public static int TryGetConditionIndex(SOBJ_ConditionAdvanced condition)
    {
        // Go through all the Conditions...
        for (int i = 0; i < TryGetConditionsLength(); i++)
        {
            // ... and if one matches the given Condition, return its index.
            if (TryGetConditionAt(i).hash == condition.hash)
            {
                return i;
            }

        }

        // If the Condition wasn't found, return -1.
        return -1;
    }

    public static int TryGetConditionIndex(int hash)
    {
        // Go through all the Conditions...
        for (int i = 0; i < TryGetConditionsLength(); i++)
        {
            // ... and if one matches the given Condition, return its index.
            if (TryGetConditionAt(i).hash == hash)
            {
                return i;
            }

        }

        // If the Condition wasn't found, return -1.
        return -1;
    }

    public static int TryGetConditionIndex(string description)
    {
        int hash = Animator.StringToHash(description);
        // Go through all the Conditions...
        for (int i = 0; i < TryGetConditionsLength(); i++)
        {
            // ... and if one matches the given Condition, return its index.
            if (TryGetConditionAt(i).hash == hash)
            {
                return i;
            }

        }

        // If the Condition wasn't found, return -1.
        return -1;
    }



    public static SOBJ_ConditionAdvanced TryGetConditionAt(int index)
    {
        // Cache the AllConditions array.
        SOBJ_ConditionAdvanced[] allConditions = SOBJ_AllConditions.Instance.conditions;

        // If it doesn't exist or there are null elements, return null.
        if (allConditions == null || allConditions[0] == null)
        {
            return null;
        }

        // If the given index is beyond the length of the array return the first element.
        if (index >= allConditions.Length)
        {
            return allConditions[0];
        }


        // Otherwise return the Condition at the given index.
        return allConditions[index];
    }


    /// <summary>
    /// Funktion for geting the alla the conditions of a 
    /// specific condition reacion 
    /// </summary>
    /// <typeparam name="T"> the conditin type that is to be sorted out</typeparam>
    /// <returns></returns>
    public static string[] getListOfReleveantConditions<T>()
    {
        /* Create a new array that has the same number 
         * of elements as there are Conditions.
         */
        string[] allConditions = AllConditionDescriptions;

        /* Go through the array and assign the description 
         * of the condition at the same index.
         */
        int count = 0;
        for (int i = 0; i < allConditions.Length; i++)
        {
            T temp;
            // attemts to cast the conditon to the wanted type,
           // if it fails so is it set to null.
            try
            {
                temp = (T)Convert.ChangeType(TryGetConditionAt(i), typeof(T));
            }
            catch(InvalidCastException)
            {
                temp = (T)Convert.ChangeType(null, typeof(T));
            }
           
           if (temp != null)
            {
                allConditions[count] = TryGetConditionAt(i).description;
                count++;
            }
        }
        return allConditions;
    }


    public static int TryGetConditionsLength()
    {
        // If there is no Conditions array, return a length of 0.
        if (SOBJ_AllConditions.Instance.conditions == null)
        {
            return 0;
        }


        // Otherwise return the length of the array.
        return SOBJ_AllConditions.Instance.conditions.Length;
    }

  


    ///// <summary>
    ///// Funktion that retrives the names of the conditions in the  
    ///// </summary>
    //private void SetConditionNamesArray()
    //{
    //    // Store the Reaction type.
    //    Type reactionType = typeof(SOBJ_ConditionAdvanced);

    //    /* Get all the types that are in the same 
    //     * Assembly (all the runtime scripts) as the Reaction type.
    //     */
    //    Type[] allTypes = reactionType.Assembly.GetTypes();

    //    /* Create an empty list to store all the types 
    //     * that are subtypes of Reaction.
    //     */
    //    List<Type> reactionSubTypeList = new List<Type>();

    //    // Go through all the types in the Assembly...
    //    for (int i = 0; i < allTypes.Length; i++)
    //    {
    //        /* ... and if they are a non-abstract subclass of 
    //         * Reaction then add them to the list.
    //         */
    //        if (allTypes[i].IsSubclassOf(reactionType) && !allTypes[i].IsAbstract)
    //        {
    //            reactionSubTypeList.Add(allTypes[i]);
    //        }
    //    }

    //    // Convert the list to an array and store it.
    //    conditionTypes = reactionSubTypeList.ToArray();

    //    /* Create an empty list of strings to store the names 
    //     * of the Reaction types.
    //     */
    //    List<string> reactionTypeNameList = new List<string>();

    //    // Go through all the Reaction types and add their names to the list.
    //    for (int i = 0; i < conditionTypes.Length; i++)
    //    {
    //        reactionTypeNameList.Add(conditionTypes[i].Name);
    //    }

    //    // Convert the list to an array and store it.
    //    conditionTypeNames = reactionTypeNameList.ToArray();
    //}

}
