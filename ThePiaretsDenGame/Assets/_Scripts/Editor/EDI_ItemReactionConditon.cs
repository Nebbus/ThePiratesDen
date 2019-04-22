using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SOBJ_Item.reactionAndCondition))]
public class EDI_ItemReactionConditon : EDI_PropertyDrawWhitSubEditors<EDI_ConditionAdvanced, 
                                                                       EDI_Reaction, 
                                                                       SOBJ_Condition, 
                                                                       SOBJ_Reaction>
{

    private SOBJ_Item.reactionAndCondition itemReaction;         // Reference to the target.

    SerializedProperty conditionsProperty;
    SerializedProperty reactionsProperty;

    private const string reactionPropName     = "reactions";
    private const string conditionsPropName   = "conditions";
    private string name;
    private bool cached = false;

  

    protected override void SubEditorSetup(EDI_ConditionAdvanced editor)
    {
        // Set the editor type so that the correct GUI for Condition is shown.
        editor.editorType = EDI_ConditionAdvanced.EditorType.ConditionCollection;

        /* Assign the conditions property so that the 
         * ConditionEditor can remove its target if necessary.
         */
        editor.conditionsProperty = conditionsProperty;
    }

    protected override void SubEditorSetup(EDI_Reaction editor)
    {
        throw new System.NotImplementedException();
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //if (!cached)
        //{
        //    name = property.displayName;

        //    property.Next(true);
        //    conditionsProperty = property.Copy();
        //    property.Next(true);
        //    reactionsProperty = property.Copy();

        //    cached = true;
        //}
        //Rect contentPosition = EditorGUI.PrefixLabel(position, new GUIContent(name));

        //  base.OnGUI(position, property, label);
        if (!cached)
        {
            name = property.displayName;

            property.Next(true);
            conditionsProperty = property.Copy();
            property.Next(true);
            reactionsProperty = property.Copy();
            cached = true;
        }


        EditorGUI.BeginProperty(position, label, property );


        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        var reactuonsRect = new Rect(position.x, position.y, 30, position.height);
        var conditionsRect = new Rect(position.x + 35f, position.y, 30, position.height);

        EditorGUI.BeginProperty(reactuonsRect, label, property.FindPropertyRelative(conditionsPropName));
        {
            EditorGUI.PropertyField(conditionsRect, property.FindPropertyRelative(conditionsPropName), GUIContent.none);

        }

        EditorGUI.BeginProperty(reactuonsRect, label, property.FindPropertyRelative(reactionPropName));
        {
            
            EditorGUI.PropertyField(reactuonsRect, property.FindPropertyRelative(reactionPropName), GUIContent.none);

        }
        EditorGUI.EndProperty();

    


        EditorGUI.EndProperty();
    }


}
