using System;
using Ignix.EventBusSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragController : MonoBehaviour
{
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private LayerMask _grabableMask;
    [SerializeField] private LayerMask _itemSlotMask;
    [Range(0, 1)]
    [SerializeField] private float _dragSnappines = .9f;
    [SerializeField] private float _heldHeight = 1; //How high from the plane should the object be

    private Camera _camera;
    private GrabbableObject _heldObject;
    private Vector2 _mousePos;
    private Vector3 _targetPos;
    
    private IEventBus EventBus => GameManager.Instance.EventBus;

    private void Awake()
    {
        _camera = Camera.main;
        
        EventBus.Register<OnObjectPlacedOnItemSlotEvent>(OnObjectPlaced);
    }

    private void OnDestroy()
    {
        EventBus.Unregister<OnObjectPlacedOnItemSlotEvent>(OnObjectPlaced);
    }

    private void Update()
    {
        if(!_heldObject)
            return;

        _heldObject.transform.position = Vector3.Lerp(_heldObject.transform.position, _targetPos, _dragSnappines);
    }

    public void OnMove(InputValue value)
    {
        _mousePos = value.Get<Vector2>();
        var ray = _camera.ScreenPointToRay(_mousePos);
        
        if(!Physics.Raycast(ray, out var hit, 1000, _groundMask))
            return;

        _targetPos = hit.point;
        _targetPos.y += _heldHeight;
    }

    public void OnGrab(InputValue value)
    {
        if(_heldObject == null)
            TryGrabObject();
        else
            DropObject();
    }
    
    private void OnObjectPlaced(OnObjectPlacedOnItemSlotEvent args)
    {
        if(_heldObject != args.Instance)
            return;

        _heldObject = null;
    }

    private void TryGrabObject()
    {
        var ray = _camera.ScreenPointToRay(_mousePos);
        
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red, 2);
        
        if(!Physics.Raycast(ray, out var hit, 1000, _grabableMask))
            return;
        
        if(!hit.collider.TryGetComponent<GrabbableObject>(out var draggableObject))
            return;

        _heldObject = draggableObject;
        _heldObject.OnGrab();
    }

    private void DropObject()
    {
        //Check if there's an itemSlot below
        var ray = _camera.ScreenPointToRay(_mousePos);
        bool placed = false;

        if (Physics.Raycast(ray, out var hit, 1000, _itemSlotMask))
        {
            //Check if we can place the object in this slot
            var itemSlot = hit.collider.GetComponent<ItemSlot>();
            placed = itemSlot.TryPlaceObject(_heldObject);
        }
        
        //If we couldn't place the object, simply drop it
        if(!placed)
            FreeDrop();
    }

    private void FreeDrop()
    {
        _heldObject.OnDrop();
        _heldObject = null;
    }
}
