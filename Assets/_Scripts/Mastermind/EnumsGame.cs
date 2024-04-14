using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumsGame : MonoBehaviour
{

}

public enum ElementType{
    Empty,
    A,
    B,
    C,
    D,
    E,
    F,
    G,
    H,
    I,
    J,
    K,
    L,
    Confirm
}

public enum TurnMoment
{
    Beggining,
    ReceivingInput,
    CheckingWin,
    GameOver

}

[System.Flags]
public enum ColorCode
{
    Empty = 0,
    Red = 1 << 0,
    Blue = 1 << 1,
    White = 1 << 2,
    Black = 1 << 3,
    Brown = 1 << 4,
    Purple = 1 << 5,
    Green = 1 << 6,
    Orange = 1 << 7,
    Gray = 1 << 8,
    Cyan = 1 << 9,
    Violet = 1 << 10,
    FlagGreen = 1 << 11,
    Magenta = 1 << 12,
    Yellow = 1 << 13
}