
public class SOBJ_InventorInputReaction : SOBJ_Reaction
{
    private MONO_Inventory monoInventory;    // Reference to the Inventory component.
    public bool handelInput;


    protected override void SpecificInit()
    {
        monoInventory = FindObjectOfType<MONO_Inventory>();

    }


    protected override void ImmediateReaction()
    {
        monoInventory.SetHandleINput(handelInput);
    }
}
