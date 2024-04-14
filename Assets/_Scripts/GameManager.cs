using Ignix.EventBusSystem;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public IEventBus EventBus { get; private set; }
    
    private void Awake()
    {
        Instance = this;
        EventBus = new EventBus();
    }

    private void Update()
    {
        EventBus.ExecuteQueue();
    }
}
