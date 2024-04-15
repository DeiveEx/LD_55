using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ignix.EventBusSystem;

public class UI_BtnReturnMenu : MonoBehaviour
{
    private IEventBus EventBus => GameManager.Instance.EventBus;
    public void GoMenuScene()
    {
        EventBus.Send(new OnReturnToMenuScene() {});
    }
}
