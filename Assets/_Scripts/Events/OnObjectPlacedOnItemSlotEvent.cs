using System;

public class OnObjectPlacedOnItemSlotEvent : EventArgs
{
    public GrabbableObject Instance;
    public int SlotIndex;
}
