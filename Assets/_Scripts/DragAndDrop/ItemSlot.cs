using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private Transform _snapPosition;
    
    private GrabbableObject _placedObject;
    private GameObject _placedGameObject;

    public GrabbableObject PlacedObject => _placedObject;

    private void OnTriggerEnter(Collider other)
    {
        if(other.attachedRigidbody == null ||
           !other.attachedRigidbody.TryGetComponent<GrabbableObject>(out var placedObject))
            return;

        _placedObject = placedObject;
        _placedGameObject = placedObject.gameObject;
        
        _placedObject.transform.SetPositionAndRotation(_snapPosition.position, _snapPosition.rotation);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject != _placedGameObject)
            return;

        _placedGameObject = null;
        _placedObject = null;
    }
}
