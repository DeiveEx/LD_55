using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ignix.EventBusSystem;

public class UISubmitCode : MonoBehaviour
{

    private IEventBus EventBus => GameManager.Instance.EventBus;
    public void SubmitCode()
    {
        EventBus.Send(new OnSubmitCode() {});
    }
}
