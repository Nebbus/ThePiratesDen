using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SOBJ_InventoryPickedUpItemReaction))]
public class EDI_InventoryPickedUpItemReaction : EDI_Reaction
{
    private SOBJ_InventoryPickedUpItemReaction targetReaction;

    private float imageSide = 125f;

    protected override void Init()
    {
         targetReaction = (SOBJ_InventoryPickedUpItemReaction)target;

    }

    protected override string GetFoldoutLabel()
    {
        return "Picked Up Item Reaction";
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

