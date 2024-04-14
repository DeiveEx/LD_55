using DG.Tweening;
using Ignix.EventBusSystem;
using UnityEngine;

[SelectionBase]
public class ItemSlot : MonoBehaviour
{
    [SerializeField] private Transform _snapPosition;
    [SerializeField] private bool _snapOnTouch;
    [SerializeField] private float _animDuration = .5f;
    [SerializeField] private Ease _easing;
    
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

        _placedObject.transform.DOMove(_snapPosition.position, _animDuration).SetEase(_easing);
        _placedObject.transform.DORotate(_snapPosition.rotation.eulerAngles, _animDuration).SetEase(_easing);
        EventBus.Send(new OnObjectPlaced() { Instance = _placedObject });

        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!_snapOnTouch)
            return;
        
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
