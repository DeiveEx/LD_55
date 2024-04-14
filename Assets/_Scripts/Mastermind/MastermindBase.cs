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
    public Elements[] resultCode;
    public Elements[] playerInput;


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

        if (turnMoment == TurnMoment.ReceivingInput && input!= Elements.Empty)
        {
            if (playerInputIndex < 4)
            {
                playerInput[playerInputIndex] = input;
                playerInputIndex++;

                Debug.Log("Key Pressed: " + input);
            }
            if(playerInputIndex == 4)
            {
                if(autoPlayCode || input == Elements.Confirm)
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

    bool CheckIfWon(Elements[] input, Elements[] result)
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
        playerInput = new Elements[4];
    }


    Elements ReadInput(KeyCode key)
    {
        //Elements elel;

        switch (key)
        {
            case KeyCode.A:
                return Elements.A;
            case KeyCode.B:
                return Elements.B;
            case KeyCode.C:
                return Elements.C;
            case KeyCode.D:
                return Elements.D;
            case KeyCode.E:
                return Elements.E;
            case KeyCode.F:
                return Elements.F;
            case KeyCode.G:
                return Elements.G;
            case KeyCode.H:
                return Elements.H;
            default:
                return Elements.Empty;
        }
    }


    Elements ReadInput2()
    {
        //Elements elel;

        if (Input.GetKey(KeyCode.A))
            return Elements.A;
        else if (Input.GetKey(KeyCode.B))
            return Elements.B;
        else if (Input.GetKey(KeyCode.C))
            return Elements.C;
        else if (Input.GetKey(KeyCode.D))
            return Elements.D;
        else if (Input.GetKey(KeyCode.E))
            return Elements.E;
        else if (Input.GetKey(KeyCode.F))
            return Elements.F;
        else if (Input.GetKey(KeyCode.G))
            return Elements.G;
        else if (Input.GetKey(KeyCode.H))
            return Elements.H;
        else if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Return))
            return Elements.Confirm;
        else
            return Elements.Empty;
    }
}