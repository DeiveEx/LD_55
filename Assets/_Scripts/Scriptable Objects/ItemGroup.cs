using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemGroup : ScriptableObject
{
    public Item[] items;

    public GameObject[] itemPrefabs;
}
