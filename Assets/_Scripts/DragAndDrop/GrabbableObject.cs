using UnityEngine;

public class GrabbableObject : MonoBehaviour
{
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void OnGrab()
    {
        _rb.isKinematic = true;
    }

    public void OnDrop()
    {
        _rb.isKinematic = false;
    }
}
