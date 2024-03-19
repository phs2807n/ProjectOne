using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenTempSlot : InventorySlot
{
    const uint NotSet = uint.MaxValue;

    uint fromIndex = NotSet;

    public uint FromIndex => fromIndex;

    public InvenTempSlot(uint index) : base(index)
    {
        fromIndex = NotSet;
    }

    public override void ClearSlotItem()
    {
        base.ClearSlotItem();
        fromIndex = NotSet;
    }

    public void SetFromIndex(uint index)
    {
        fromIndex = index;
    }
}
