using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    public ColorCode colorCode;

    public ElementType elementType;

    public Material colorMaterial;
}
