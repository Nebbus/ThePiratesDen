using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SOBJ_InventoryGrabItemToMouseReaction))]
public class EDI_InventoryGrabItemToMouseReaction : EDI_Reaction
{

    private float imageSide         = 125f;

    private SerializedProperty itemToGrabPrperty;
    private const string itemToGrabPropertyName = "itemToGrab";


    private SOBJ_InventoryGrabItemToMouseReaction lokalReaction;

    protected override void Init()
    {
        lokalReaction     = target as SOBJ_InventoryGrabItemToMouseReaction;
        itemToGrabPrperty = serializedObject.FindProperty(itemToGrabPropertyName);
    }


    protected override string GetFoldoutLabel()
    {
        return "Inventory Grab Item To Mouse Reaction";
    }

    protected override void DrawReaction()
    {
        EditorGUILayout.PropertyField(itemToGrabPrperty, GUIContent.none,GUILayout.Width((EditorGUIUtility.currentViewWidth / 3f) + 30f));
        if (lokalReaction                       != null &&
            lokalReaction.getItemToGrabe        != null &&
            lokalReaction.getItemToGrabe.sprite != null)
        {
            GUILayout.Box(lokalReaction.getItemToGrabe.sprite.texture, GUILayout.Width(imageSide), GUILayout.Height(imageSide));

        }
    }


 

}
