using Ignix.EventBusSystem;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public IEventBus EventBus { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);
        EventBus = new EventBus();
    }

    private void Update()
    {
        EventBus.ExecuteQueue();
    }
}
