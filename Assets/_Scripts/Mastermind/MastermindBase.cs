using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ignix.EventBusSystem;
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

    // --- Event Buss ---
    private IEventBus EventBus => GameManager.Instance.EventBus;

    private void OnEnable()
    {
        EventBus.Register<OnObjectPlacedOnItemSlotEvent>(ObjectPlacedOnSlot);
        EventBus.Register<OnSubmitCode>(CodeSubmited);
    }

    private void OnDisable()
    {
        EventBus.Unregister<OnObjectPlacedOnItemSlotEvent>(ObjectPlacedOnSlot);
        EventBus.Unregister<OnSubmitCode>(CodeSubmited);
    }

    private void Start()
    {
        GenerateResultCode();
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

        //ClearPlayerCode();

        Log("Next Turn");
    }

    void ReceiveInput()
    {
        var input = ReadInput2(); 

        if (turnMoment == TurnMoment.ReceivingInput && input!= ElementType.Empty)
        {
            if (playerInputIndex < numMaxElements)
            {
                playerInput[playerInputIndex] = input;
                playerInputIndex++;

                Log("Key Pressed: " + input);
            }
            
            if(playerInputIndex == numMaxElements)
            {
                if(autoPlayCode || input == ElementType.Confirm)
                {
                    StartCoroutine(CheckCodeRoutine());
                }
            }
        }
    }

    void CodeSubmited(OnSubmitCode args)
    {
        Log("Receive code");
        if (turnMoment == TurnMoment.ReceivingInput)    // Also check if all slots are filled
        {
            StartCoroutine(CheckCodeRoutine());
        }
    }


    void ObjectPlacedOnSlot(OnObjectPlacedOnItemSlotEvent args)
    {
        ReceiveItem(args.Instance.ItemSettings, args.SlotIndex);
    }

    void ReceiveItem(Item item, int slotPos)
    {
        var inputType = item.elementType;

        if (turnMoment == TurnMoment.ReceivingInput && inputType != ElementType.Empty)
        {
            if (slotPos < numMaxElements)
            {
                playerInput[slotPos] = inputType;

                Log(string.Format("Placed item {0} on slot {1}", inputType.ToString(), slotPos)); //"Received Item: " + inputType
            }
            else
            {
                Log("Invalid item slot");
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
        playerInput = new ElementType[numMaxElements];
    }


    void GenerateResultCode()
    {
        // This should be random latter, now its deterministc
        resultCode = new ElementType[numMaxElements];

        for (int i = 0; i < numMaxElements; i++)
        {
            resultCode[i] = (ElementType)(i+1);
        }
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

    private IEnumerator CheckCodeRoutine()
    {
        //First, check if we got the right code
        List<bool> options = new();
        EventBus.Send(new HighlighCodeEvent() { ShouldHighlight = options }); //Reset

        for (int i = 0; i < playerInput.Length; i++)
        {
            options.Add(GetPointForInput(i) == 1);
        }

        EventBus.Send(new HighlighCodeEvent() { ShouldHighlight = options });
        yield return new WaitForSeconds(1);

        options = options.Select(x => false).ToList();
        EventBus.Send(new HighlighCodeEvent() { ShouldHighlight = options }); //Reset

        //Then show the player the result of each option
        for (int i = 0; i < options.Count; i++)
        {
            for (int j = 0; j < options.Count; j++)
            {
                options[j] = i == j;
            }
            
            //Highlight the option
            EventBus.Send(new HighlighCodeEvent() { ShouldHighlight = options });
            yield return new WaitForSeconds(.5f);

            int amount = GetPointForInput(i);
            
            //Move the clock pointer
            EventBus.Send(new OnPointAddedEvent(){ Amount = amount});
            yield return new WaitForSeconds(.5f);
        }
        
        //Finally, go to the next turn
        for (int i = 0; i < options.Count; i++)
        {
            options[i] = false;
        }
        
        EventBus.Send(new HighlighCodeEvent() { ShouldHighlight = options });

        yield return new WaitForSeconds(1);
        AdvanceTurn();
    }

    private int GetPointForInput(int index)
    {
        int amount = 0;

        var input = playerInput[index];
        
        //Correct item, correct position
        if (resultCode[index] == input)
        {
            amount = 1;
        }
        //Correct item, wrong position
        else if(resultCode.Contains(input))
        {
            amount = 0;
        }
        //Wrong item, wrong position
        else
        {
            amount = -1;
        }
        
        return amount;
    }

    private void Log(string text)
    {
        Debug.Log(text);
        EventBus.Send(new OnPrintDebug() { Text = text });
    }
}