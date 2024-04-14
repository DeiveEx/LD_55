using System;

public class OnObjectPlaced : EventArgs
{
    public GrabbableObject Instance;
    public int SlotIndex;
}
