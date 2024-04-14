using Ignix.EventBusSystem;
using UnityEngine;

[SelectionBase]
public class GrabbableObject : MonoBehaviour
{
    private Rigidbody _rb;

    private IEventBus EventBus => GameManager.Instance.EventBus;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void OnGrab()
    {
        _rb.isKinematic = true;
        EventBus.Send(new OnObjectGrabbedEvent() { Instance = this});
    }

    public void OnDrop()
    {
        _rb.isKinematic = false;
        EventBus.Send(new OnObjectDroppedEvent() { Instance = this});
    }

    public void OnPlaced()
    {
        
    }
}
