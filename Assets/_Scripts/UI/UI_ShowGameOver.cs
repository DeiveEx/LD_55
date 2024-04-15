using System;
using System.Collections.Generic;
using Ignix.EventBusSystem;
using UnityEngine;

public class UI_ShowGameOver : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    private IEventBus EventBus => GameManager.Instance.EventBus;
    
    private void OnEnable()
    {
        EventBus.Register<OnPlayerLost>(ShowUI);
    }

    private void OnDisable()
    {
        EventBus.Unregister<OnPlayerLost>(ShowUI);
    }

    private void ShowUI(OnPlayerLost args)
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }
}
