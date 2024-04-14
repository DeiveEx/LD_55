using UnityEngine;

public class DropZone : MonoBehaviour
{
    [SerializeField] private Transform _returnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if(!other.TryGetComponent<GrabbableObject>(out var grabbableObject))
            return;

        other.transform.position = _returnPoint.position;
    }
}
