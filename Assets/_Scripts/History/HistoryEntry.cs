using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HistoryEntry : MonoBehaviour
{
    public HistorySphere[] entries;

    public void InitializeEntry(int[] Results)
    {
        for (int result = 0; result < Results.Length; result++)
        {
            entries[result].UpdateMaterial(Results[result]);
        }
    }
}
