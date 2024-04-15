using System;
using Ignix.EventBusSystem;
using TMPro;
using UnityEngine;

public class ItemNameUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    
    private IEventBus EventBus => GameManager.Instance.EventBus;
    
    private void Awake()
    {
        EventBus.Register<OnObjectGrabbedEvent>(OnObjectGrabbed);
        EventBus.Register<OnObjectDroppedEvent>(OnObjectDropped);
        EventBus.Register<OnObjectPlacedOnItemSlotEvent>(OnObjectPlacedOnSlot);

        _text.text = "";
    }

    private void OnDestroy()
    {
        EventBus.Unregister<OnObjectGrabbedEvent>(OnObjectGrabbed);
        EventBus.Unregister<OnObjectDroppedEvent>(OnObjectDropped);
        EventBus.Unregister<OnObjectPlacedOnItemSlotEvent>(OnObjectPlacedOnSlot);
    }

    private void OnObjectGrabbed(OnObjectGrabbedEvent args)
    {
        _text.text = args.Instance.ItemSettings.name;
    }

    private void OnObjectDropped(OnObjectDroppedEvent args)
    {
        _text.text = "";
    }
    
    private void OnObjectPlacedOnSlot(OnObjectPlacedOnItemSlotEvent obj)
    {
        _text.text = "";
    }
}
