using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HistorySphere : MonoBehaviour
{
    public Material WrongMaterial;
    public Material MisplacedMaterial;
    public Material CorrectMaterial;
    public void UpdateMaterial(int result)
    {
        Renderer r = GetComponent<Renderer>();
        switch (result)
        {
            case -1:
                r.SetMaterials(new List<Material>() { WrongMaterial });
                break;
            case 0:
                r.SetMaterials(new List<Material>() { MisplacedMaterial });
                break; 
            case 1:
                r.SetMaterials(new List<Material>() { CorrectMaterial });
                break;

        }
    }
        
    

}
