using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ignix.EventBusSystem;

public class UI_ShowVictory : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    private IEventBus EventBus => GameManager.Instance.EventBus;
    private void OnEnable()
    {
        EventBus.Register<OnPlayerWon>(ShowUI);
    }

    private void OnDisable()
    {
        EventBus.Unregister<OnPlayerWon>(ShowUI);
    }

    private void ShowUI(OnPlayerWon args)
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }
}
