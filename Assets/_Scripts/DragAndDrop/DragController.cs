using UnityEngine;
using UnityEngine.InputSystem;

public class DragController : MonoBehaviour
{
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private LayerMask _grabableMask;
    [SerializeField] private float _heldHeight = 1; //How high from the plane should the object be

    private Camera _camera;
    private GrabbableObject _heldObject;
    private Vector2 _mousePos;
    private Vector3 _targetPos;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if(!_heldObject)
            return;

        _heldObject.transform.position = _targetPos;
    }

    public void OnMove(InputValue value)
    {
        _mousePos = value.Get<Vector2>();
        var ray = _camera.ScreenPointToRay(_mousePos);
        
        if(!Physics.Raycast(ray, out var hit, 1000, _groundMask))
            return;

        Debug.Log($"{hit.collider.name}");
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

    private void TryGrabObject()
    {
        var ray = _camera.ScreenPointToRay(_mousePos);
        
        if(!Physics.Raycast(ray, out var hit, 1000, _grabableMask))
            return;
        
        if(!hit.collider.TryGetComponent<GrabbableObject>(out var draggableObject))
            return;

        _heldObject = draggableObject;
        _heldObject.OnGrab();
    }

    private void DropObject()
    {
        _heldObject.OnDrop();
        _heldObject = null;
    }
}
