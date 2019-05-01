using System;
using UnityEngine;
using UnityEditor;

public abstract class EDI_ConditionAdvanced : Editor
{
    // This enum is used to represent where the Condition is being seen in the inspector.
    // ConditionAsset is for when a single Condition asset is selected as a child of the AllConditions asset.
    // AllConditionAsset is when the AllConditions asset is selected and this is a nested Editor.
    // ConditionCollection is when an Interactable is selected and this is a nested Editor within a ConditionCollection.
    public enum EditorType
    {
        ConditionAsset, AllConditionAsset, ConditionCollection
    }
//-----------------------------------------------------------------------------------
//  Public variablesvariables
//-----------------------------------------------------------------------------------
    public EditorType editorType;                      // The type of this Editor.

    public EDI_ItemInteractable2 parentEditor = null;  // not null if the reaction is in a itemInteractable

    public    SerializedProperty conditionsProperty;   // The SerializedProperty representing an array of Conditions on a ConditionCollection.

 //-----------------------------------------------------------------------------------
 //  Protected variables
 //-----------------------------------------------------------------------------------

    protected SerializedProperty satisfiedProperty;    // Represents a bool of whether this Editor's target is satisfied.

    protected SOBJ_ConditionAdvanced condition;        // Reference to the target.


    protected const float conditionButtonWidth = 30f;  // Width in pixels of the button to remove this Condition from it's array.
    protected const float toggleOffset         = 30f; // Offset to line up the satisfied toggle with its label.

    protected string[] relevatnConditionDescriptions; // will be used in the edditor, drop down menu



    //-----------------------------------------------------------------------------------
    //  private variables
    //-------------------------------------------------------------------------------

    private SerializedProperty descriptionProperty;  // Represents a string description of this Editor's target.
    private SerializedProperty hashProperty;         // Represents the number that identified this Editor's target.

    private const string conditionPropDescriptionName = "description";         // Name of the field that represents the description.
    private const string conditionPropHashName        = "hash";                // Name of the field that represents the Condition's identifier.
    private const string conditionPropSatisfiedName   = "satisfied";           // Name of the field that represents whether or not the Condition is satisfied.
    private const string blankDescription             = "No conditions set.";  // Description to use in case no Conditions have been created yet.

  


    private void OnEnable()
    {

        // Cache the target.
        condition = (SOBJ_ConditionAdvanced)target;

        /* If this Editor has persisted through 
         *the destruction of it's target then destroy it.
         */
        if (target == null)
        {
            DestroyImmediate(this);
            return;
        }

        // Cache the SerializedProperties.
        descriptionProperty = serializedObject.FindProperty(conditionPropDescriptionName);
        satisfiedProperty   = serializedObject.FindProperty(conditionPropSatisfiedName);
        hashProperty        = serializedObject.FindProperty(conditionPropHashName);

        // Call an initialisation method for inheriting classes.
        Init();
    }
    
    
    
    /// <summary>
    /// This function should be overridden by 
    /// inheriting classes that need initialisation.
    /// </summary>
    protected virtual void Init() { }

    /// <summary>
    /// used by the SOBJ_itemInteractable to remove this item
    /// </summary>
    public void voidEdiorItemRemove()
    {
        parentEditor.RemoveCondition(condition);

    }

    public override void OnInspectorGUI()
    {
        // Call different GUI depending where the Condition is.
        switch (editorType)
        {
            case EditorType.AllConditionAsset:
               AllConditionsAssetGUI(); 
                break;
            case EditorType.ConditionAsset:
                ConditionAssetGUI();
                break;
            case EditorType.ConditionCollection:
                InteractableGUI();
                break;
            default:
                throw new UnityException("Unknown EDI_Condition.EditorType.");
        }
    }

//-----------------------------------------------------------------------------------
//  The main editors for the condition, same for all conditions
//-----------------------------------------------------------------------------------
    /// <summary>
    /// This is displayed for each Condition when
    /// the AllConditions asset is selected.
    /// </summary>
    private void AllConditionsAssetGUI()
    {
        EditorGUILayout.BeginHorizontal(GUI.skin.box);
        EditorGUI.indentLevel++;

        EditorGUILayout.BeginVertical();
         // Display the description of the Condition.
         EditorGUILayout.LabelField("Name: " + condition.description);

         // Display the Condition type.
         EditorGUILayout.LabelField("Type: " + condition.GetType().ToString());

        // Display the hash.
        EditorGUILayout.LabelField("Hash: " + condition.hash.ToString());

        EditorGUILayout.EndVertical();

        DrawConditionAllConditionsAssetGUI();

        // Display a button showing a '-' that if clicked removes this Condition from the AllConditions asset.
        if (GUILayout.Button("-", GUILayout.Width(conditionButtonWidth)))
        {
          
            
                EDI_AllConditions.RemoveCondition(condition);
       
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// GUI displayed when a single Condition asset is selected as a child of the AllConditions asset.
    /// </summary>
    private void ConditionAssetGUI()
    {
        EditorGUILayout.BeginHorizontal(GUI.skin.box);
        EditorGUI.indentLevel++;

        EditorGUILayout.BeginVertical();
         // Display the description of the Condition.
         EditorGUILayout.LabelField("Name: " + condition.description);

         // Display the Condition type.
         EditorGUILayout.LabelField("Type: " + condition.GetType().ToString());

        // Display the hash.
        EditorGUILayout.LabelField("Hash: " + condition.hash.ToString());

        EditorGUILayout.EndVertical();

        DrawConditionConditionAssetGUI();

        EditorGUI.indentLevel--;
        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// GUI show then the condition is inspected in the SOBJ_condidionCollection
    /// or in the SOBJ_itemInteractable
    /// </summary>
    private void InteractableGUI()
    {
        // Pull the information from the target into the serializedObject.
        serializedObject.Update();

        // The width for the Popup, Toggle and remove Button.
        float width = EditorGUIUtility.currentViewWidth /3f;

        EditorGUILayout.BeginHorizontal();
        
        // Find the index for the target based on the AllConditions array.
        int conditionIndex = EDI_AllConditions.TryGetConditionIndex(condition);

        /* If the target can't be found in the AllConditions 
         * array use the first condition.
         */
        if (conditionIndex == -1)
        {
            conditionIndex = 0;
        }

        /* Set the index based on the user selection 
         * of the relevant condition by the user.
         */
        conditionIndex                         = EditorGUILayout.Popup(conditionIndex, getListOfReleveantConditions(), GUILayout.Width(width ));
        SOBJ_ConditionAdvanced globalCondition = EDI_AllConditions.TryGetConditionAt(conditionIndex);

        // Set the description based on the globalCondition's description.
        descriptionProperty.stringValue = (globalCondition != null) ? globalCondition.description : blankDescription;
      
        // Set the hash based on the description.
        hashProperty.intValue = Animator.StringToHash(descriptionProperty.stringValue);

         DrawConditionInteractableGUI();
        /* Display a button with a '-' that when clicked removes
          * the target from the ConditionCollection's conditions array.
          */
        bool destroyMe = GUILayout.Button("-", GUILayout.Width(conditionButtonWidth));
       
        EditorGUILayout.EndHorizontal();
 
        serializedObject.ApplyModifiedProperties();

        /*Removers this after  serializedObject.ApplyModifiedProperties();
         * to avoid attemt to update null
         */
        if (destroyMe)
        {

            if (parentEditor != null)
            {
                voidEdiorItemRemove();
            }
            else
            {
                conditionsProperty.RemoveFromObjectArray(condition);
            }
        }
    }

//-----------------------------------------------------------------------------------
//  The custom editors for the condition individual conditions
//  if custom editor is wanted.
//-----------------------------------------------------------------------------------

    /// <summary>
    /// Costum editor for how the condition 
    /// appers in the AllConditionsAsset GUI
    /// (as defult dos this funktion contain nothing)
    /// </summary>
    protected virtual void DrawConditionAllConditionsAssetGUI(){}

    /// <summary>
    /// Costum editor for how the condition 
    /// appers in the AllConditionsAsset GUI
    /// (as defult dos this funktion contain nothing)
    /// </summary>
    protected virtual void DrawConditionConditionAssetGUI(){}

//-----------------------------------------------------------------------------------
//  Abstract funktions, unic for all funktions
//-----------------------------------------------------------------------------------

    /// <summary>
    /// Costum editor for how the condition 
    /// appers in the Interactable GUI
    /// (as defult dos this funktion contain nothing)
    /// </summary>
    protected abstract void DrawConditionInteractableGUI();


    /// <summary>
    /// Just a raper funktion for the EDI_AllCondition.getListOfReleveantConditions<T>()
    /// ther the T is replaced whit th erelevant type
    /// </summary>
    /// <returns> a list containg the name of all 
    ///  scriptable object that is of the same taype 
    ///  as the this object</returns>
    public abstract string[] getListOfReleveantConditions();


//-----------------------------------------------------------------------------------
//  Static funktions, unic for all funktions
//-----------------------------------------------------------------------------------

    /// <summary>
    /// Creates a instans of a condition type
    /// will be used by ConditionCollektion then new 
    /// </summary>
    /// <param name="conditionType"> the condition to be instantiated</param>
    /// <returns> A instance of conditionType </returns>
    public static SOBJ_ConditionAdvanced CreateCondition(Type conditionType)
    {
        // Create a reaction of a given type.
        return (SOBJ_ConditionAdvanced)CreateInstance(conditionType);
    }


    /// <summary>
    /// This function is static such that
    /// it can be called without an editor being instanced.
    /// </summary>
    /// <returns> first SOBJ_Condtion condition</returns>
    public static SOBJ_ConditionAdvanced CreateCondition()
    {
        // Create a new instance of Condition.
        SOBJ_Condition newCondition = CreateInstance<SOBJ_Condition>();

        string blankDescription = "No conditions set.";

        /* Try and set the new condition's description 
         * to the first condition in the AllConditions array.
         */
        SOBJ_ConditionAdvanced globalCondition = (SOBJ_ConditionAdvanced)EDI_AllConditions.TryGetConditionAt(0);
        newCondition.description = globalCondition != null ? globalCondition.description : blankDescription;

        // Set the hash based on this description.
        SetHash(newCondition);
        return newCondition;
    }


    /// <summary>
    /// Creats an instans, used by SOBJ_Allcondition, of an object and givs it a name
    /// </summary>
    /// <param name="description"> the name/description of the condition</param>
    /// <param name="condisionType"> the condition type</param>
    /// <returns> the new namned condition</returns>
    public static SOBJ_ConditionAdvanced CreateCondition(string description, Type condisionType)
    {
     
        // Create a new instance of the Condition.
        SOBJ_ConditionAdvanced newCondition = (SOBJ_ConditionAdvanced)CreateInstance(condisionType);

        // Set the description and the hash based on it.
        newCondition.description = description;
        SetHash(newCondition);
        return newCondition;
    }

    /// <summary>
    /// Sets the hase of conditon
    /// </summary>
    /// <param name="condition"> condition that is suposed to get hased</param>
    private static void SetHash(SOBJ_ConditionAdvanced condition)
    {
        condition.hash = Animator.StringToHash(condition.description);
    }
}
