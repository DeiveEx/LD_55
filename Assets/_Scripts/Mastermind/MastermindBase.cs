using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ignix.EventBusSystem;
using UnityEngine;
using Random = UnityEngine.Random;

public class MastermindBase : MonoBehaviour
{
    [Header("References")]
    public LoveGaugeUI _loveGaugeUI;
    public CurrentDayUI _currentDayDisplay;
    public AudioSource _audioSource;
    public AudioClip _submitClip;
    
    [Header("Settings")]
    public int maxNumTurns = 7;
    public int currentTurn = 0;
    public Vector2Int _minMaxEmotionIndex = new Vector2Int(0, 8);

    public int numMaxElements = 4;

    public int numMaxSlots;
    public float _confirmWaitTime = 3f;

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

    private bool _allItemsPlaced;
    
    // --- Event Buss ---
    private IEventBus EventBus => GameManager.Instance.EventBus;

    private void OnEnable()
    {
        EventBus.Register<OnObjectPlacedOnItemSlotEvent>(ObjectPlacedOnSlot);
        EventBus.Register<OnObjectRemovedFromItemSlotEvent>(ObjectRemovedFromSlot);
        EventBus.Register<OnSubmitCode>(CodeSubmitted);
    }

    private void OnDisable()
    {
        EventBus.Unregister<OnObjectPlacedOnItemSlotEvent>(ObjectPlacedOnSlot);
        EventBus.Unregister<OnObjectRemovedFromItemSlotEvent>(ObjectRemovedFromSlot);
        EventBus.Unregister<OnSubmitCode>(CodeSubmitted);
    }

    private void Start()
    {
        StartGameplay();
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
        _loveGaugeUI.SetArrowToSection(Random.Range(_minMaxEmotionIndex.x, _minMaxEmotionIndex.y));
        GenerateResultCode();
        ResetMagicCircle();
        
        ResetGame();
        turnMoment = TurnMoment.ReceivingInput;
        _currentDayDisplay.SetCurrentDay(currentTurn);
    }

    void AdvanceTurn()
    {
        currentTurn++;
        playerInputIndex = 0;
        ClearPlayerCode();
        Log("Next Turn");
        
        _currentDayDisplay.SetCurrentDay(currentTurn);
        EventBus.Send(new OnTurnStartedEvent());
    }

    void CodeSubmitted(OnSubmitCode args)
    {
        CodeSubmitted();
    }

    private void CodeSubmitted()
    {
        //Is the code valid?
        if(playerInput.Any(x => x == ElementType.Empty || x == ElementType.Confirm))
            return;
        
        Log($"Receive code: {string.Join(";", playerInput.Select(x => x))}");
        if (turnMoment == TurnMoment.ReceivingInput)    // Also check if all slots are filled
        {
            StartCoroutine(CheckCodeRoutine());
        }
    }

    void ObjectPlacedOnSlot(OnObjectPlacedOnItemSlotEvent args)
    {
        ReceiveItem(args.Instance.ItemSettings, args.SlotIndex);

        _allItemsPlaced = !playerInput.Any(x => x == ElementType.Empty || x == ElementType.Confirm);

        if (_allItemsPlaced)
        {
            _audioSource.PlayOneShot(_submitClip);
            Debug.Log($"All Items placed: {_allItemsPlaced}");
            StartCoroutine(SubmitCountdownRoutine());
        }
    }

    private void ObjectRemovedFromSlot(OnObjectRemovedFromItemSlotEvent ars)
    {
        Debug.Log($"Item {ars.Instance.ItemSettings.elementType} removed from Slot {ars.SlotIndex}");
        _allItemsPlaced = false;
        _audioSource.Stop();
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
        resultCode = new ElementType[numMaxElements];

        //Right now the password cannot have duplicated elements, we can chane later
        List<ElementType> options = new();

        foreach (ElementType elementType in Enum.GetValues(typeof(ElementType)))
        {
            if(elementType == ElementType.Empty || elementType == ElementType.Confirm)
                continue;
            
            options.Add(elementType);
        }

        for (int i = 0; i < numMaxElements; i++)
        {
            var chosenElementIndex = Random.Range(0, options.Count); 
            resultCode[i] = options[chosenElementIndex];
            options.RemoveAt(chosenElementIndex);
        }
    }

    private IEnumerator CheckCodeRoutine()
    {
        ResetMagicCircle();
        
        //First, check if we got the right code
        List<bool> options = new();

        for (int i = 0; i < playerInput.Length; i++)
        {
            options.Add(GetPointForInput(i) == 1);
        }

        EventBus.Send(new HighlighCodeEvent() { ShouldHighlight = options });
        yield return new WaitForSeconds(1);

        ResetMagicCircle();

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
        ResetMagicCircle();

        yield return new WaitForSeconds(1);
        AdvanceTurn();
    }
    
    private IEnumerator SubmitCountdownRoutine()
    {
        float timePassed = 0;

        while (timePassed < _confirmWaitTime)
        {
            timePassed += Time.deltaTime;
            yield return null;
            
            if(!_allItemsPlaced)
                yield break;
        }
        
        CodeSubmitted();
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
    
    private void ResetMagicCircle()
    {
        //Reset maic circle
        List<bool> circleHighlightSections = new();

        for (int i = 0; i < numMaxElements; i++)
        {
            circleHighlightSections.Add(false);
        }
        
        EventBus.Send(new HighlighCodeEvent() { ShouldHighlight = circleHighlightSections }); 
    }
}