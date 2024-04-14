using System;

public class OnObjectRemovedFromItemSlotEvent : EventArgs
{
    public GrabbableObject Instance;
    public int SlotIndex;
}
