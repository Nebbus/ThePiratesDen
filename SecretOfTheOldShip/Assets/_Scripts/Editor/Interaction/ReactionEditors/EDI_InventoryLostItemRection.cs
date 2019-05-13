using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SOBJ_InventoryLostItemRection))]
public class EDI_InventoryLostItemRection : EDI_Reaction
{

    private SOBJ_InventoryLostItemRection targetReaction;

    private float imageSide = 125f;

    protected override void Init()
    {
        targetReaction = (SOBJ_InventoryLostItemRection)target;

    }

    protected override string GetFoldoutLabel()
    {
        return "Lost Item Reaction";
    }

    protected override void DrawReaction()
    {
        base.DrawReaction();
        DrawImages();
    }

    /// <summary>
    /// Displays the sprite of the item in the conditions
    /// </summary>
    private void DrawImages()
    {


        if (targetReaction != null &&
            targetReaction.item != null &&
            targetReaction.item.sprite != null)
        {
            GUILayout.Box(targetReaction.item.sprite.texture, GUILayout.Width(imageSide), GUILayout.Height(imageSide));


        }
    }
}
