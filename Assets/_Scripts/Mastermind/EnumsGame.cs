using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumsGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}

public enum Elements{
    Empty,
    A,
    B,
    C,
    D,
    E,
    F,
    G,
    H,
    Confirm
}

public enum TurnMoment
{
    Beggining,
    ReceivingInput,
    CheckingWin,
    GameOver

}