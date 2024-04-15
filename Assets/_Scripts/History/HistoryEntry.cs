using UnityEngine;

public class HistoryEntry : MonoBehaviour
{
    public HistorySphere[] entries;
    public Item[] sequence;
    public int[] points;

    public void InitializeEntry(Item[] sequence, int[] points)
    {
        this.sequence = sequence;
        this.points = points;
        
        for (int result = 0; result < points.Length; result++)
        {
            entries[result].UpdateMaterial(points[result]);
        }
    }
}
