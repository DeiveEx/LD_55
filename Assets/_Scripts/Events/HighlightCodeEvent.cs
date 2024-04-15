using System;
using System.Collections.Generic;

public class HighlightCodeEvent : EventArgs
{
    public List<bool> EntryHighlights = new();
    public List<int> EntryPoints = new();
}
