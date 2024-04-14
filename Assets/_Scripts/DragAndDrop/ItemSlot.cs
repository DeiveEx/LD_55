using Ignix.EventBusSystem;
using UnityEngine;

[SelectionBase]
public class ItemSlot : MonoBehaviour
{
    [SerializeField] private Transform _snapPosition;
    
    private GrabbableObject _placedObject;
    private GameObject _placedGameObject;

    public GrabbableObject PlacedObject => _placedObject;
    private IEventBus EventBus => GameManager.Instance.EventBus;

    public bool TryPlaceObject(GrabbableObject targetObject)
    {
        if (_placedObject != null)
            return false;
        
        _placedObject = targetObject;
        _placedGameObject = targetObject.gameObject;
        _placedObject.OnPlaced();
        
        _placedObject.transform.SetPositionAndRotation(_snapPosition.position, _snapPosition.rotation);
        EventBus.Send(new OnObjectPlaced() { Instance = _placedObject });

        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.attachedRigidbody == null ||
           !other.attachedRigidbody.TryGetComponent<GrabbableObject>(out var placedObject))
            return;

        TryPlaceObject(placedObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject != _placedGameObject)
            return;

        _placedGameObject = null;
        _placedObject = null;
    }
}
