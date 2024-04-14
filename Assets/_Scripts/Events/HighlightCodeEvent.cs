using System;
using System.Collections.Generic;

public class HighlighCodeEvent : EventArgs
{
    public List<bool> ShouldHighlight = new();
}
