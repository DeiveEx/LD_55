using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MastermindBase : MonoBehaviour
{
    [Header("Settings")]
    public int maxNumTurns = 7;
    public int currentTurn = 0;

    public int numMaxElements = 4;

    public int numMaxSlots;

    [Space]
    [Tooltip("If the player ")]
    public bool autoPlayCode = false;

    [Header("Gameplay")]
    public TurnMoment turnMoment;
    public ElementType[] resultCode;
    public ElementType[] playerInput;

    [Header("Not Implemented")]
    // Play history
    [Space]
    [SerializeField] int playerInputIndex;

    private void Start()
    {
        StartGameplay();
    }

    private void Update()
    {
        if(turnMoment == TurnMoment.ReceivingInput)
        {
            if (Input.anyKeyDown)
            {
                ReceiveInput();
            }
        }
    }

    void ResetGame()
    {
        currentTurn = 0;
        playerInputIndex = 0;
        turnMoment = TurnMoment.Beggining;

        ClearPlayerCode();
    }

    void StartGameplay()
    {
        ResetGame();

        turnMoment = TurnMoment.ReceivingInput;
    }


    void AdvanceTurn()
    {
        currentTurn++;
        playerInputIndex = 0;

        ClearPlayerCode();

        Debug.Log("Next Turn");
    }

    void ReceiveInput()
    {

        var input = ReadInput2(); 

        if (turnMoment == TurnMoment.ReceivingInput && input!= ElementType.Empty)
        {
            if (playerInputIndex < 4)
            {
                playerInput[playerInputIndex] = input;
                playerInputIndex++;

                Debug.Log("Key Pressed: " + input);
            }
            if(playerInputIndex == 4)
            {
                if(autoPlayCode || input == ElementType.Confirm)
                {
                    if (CheckIfWon(playerInput, resultCode))
                    {
                        Debug.Log("Right Code");
                    }
                    else
                    {
                        Debug.Log("Wrong Code");
                        AdvanceTurn();
                    }
                }
            }
        }
    }

    bool CheckIfWon(ElementType[] input, ElementType[] result)
    {
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] != result[i])
                return false;
        }

        return true;
    }

    void ClearPlayerCode()
    {
        playerInput = new ElementType[4];
    }


    ElementType ReadInput(KeyCode key)
    {
        //Elements elel;

        switch (key)
        {
            case KeyCode.A:
                return ElementType.A;
            case KeyCode.B:
                return ElementType.B;
            case KeyCode.C:
                return ElementType.C;
            case KeyCode.D:
                return ElementType.D;
            case KeyCode.E:
                return ElementType.E;
            case KeyCode.F:
                return ElementType.F;
            case KeyCode.G:
                return ElementType.G;
            case KeyCode.H:
                return ElementType.H;
            default:
                return ElementType.Empty;
        }
    }


    ElementType ReadInput2()
    {
        //Elements elel;

        if (Input.GetKey(KeyCode.A))
            return ElementType.A;
        else if (Input.GetKey(KeyCode.B))
            return ElementType.B;
        else if (Input.GetKey(KeyCode.C))
            return ElementType.C;
        else if (Input.GetKey(KeyCode.D))
            return ElementType.D;
        else if (Input.GetKey(KeyCode.E))
            return ElementType.E;
        else if (Input.GetKey(KeyCode.F))
            return ElementType.F;
        else if (Input.GetKey(KeyCode.G))
            return ElementType.G;
        else if (Input.GetKey(KeyCode.H))
            return ElementType.H;
        else if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Return))
            return ElementType.Confirm;
        else
            return ElementType.Empty;
    }
}