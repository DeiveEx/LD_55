using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ignix.EventBusSystem;



public class DebugText : MonoBehaviour
{
    public TMP_Text text;

    private IEventBus EventBus => GameManager.Instance.EventBus;

    private void OnEnable()
    {
        EventBus.Register<OnPrintDebug>(LogEvent);
    }

    private void OnDisable()
    {
        EventBus.Unregister<OnPrintDebug>(LogEvent);
    }

    void PrintText(string s)
    {
        text.text = s;
    }

    void LogEvent(OnPrintDebug args)
    {
        PrintText(args.Text);
    }
}
