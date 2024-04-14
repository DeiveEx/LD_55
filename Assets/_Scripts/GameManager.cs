using System;
using System.Collections;
using System.Collections.Generic;
using Ignix.EventBusSystem;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public IEventBus EventBus { get; private set; }
    
    private void Awake()
    {
        EventBus = new EventBus();
    }

    private void Update()
    {
        EventBus.ExecuteQueue();
    }
}
