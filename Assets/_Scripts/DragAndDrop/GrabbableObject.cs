using System;
using DG.Tweening;
using Ignix.EventBusSystem;
using UnityEngine;
using Random = UnityEngine.Random;

[SelectionBase]
public class GrabbableObject : MonoBehaviour
{
    [SerializeField] private Item _itemSettings;
    [SerializeField] private float _returnAnimDuration = .5f;
    [SerializeField] private Ease _retunAnimEase;
    [SerializeField] private AudioClip _grabSound;
    [SerializeField] private AudioClip _dropSound;
    [SerializeField] private AudioClip _hitSound;
    [SerializeField] private AudioSource _audioSource;
    
    private Rigidbody _rb;
    private Vector3 _startPos;
    private Quaternion _startRot;
    
    public Item ItemSettings { get { return _itemSettings; } }

    private IEventBus EventBus => GameManager.Instance.EventBus;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _startPos = transform.position;
        _startRot = transform.rotation;
        
        EventBus.Register<OnTurnStartedEvent>(OnTurnStarted);
    }

    private void OnDestroy()
    {
        EventBus.Unregister<OnTurnStartedEvent>(OnTurnStarted);
    }

    private void OnTurnStarted(OnTurnStartedEvent args)
    {
        transform.DOKill();
        transform.DOMove(_startPos, _returnAnimDuration).SetEase(_retunAnimEase);
        transform.DORotateQuaternion(_startRot, _returnAnimDuration).SetEase(_retunAnimEase);
    }

    public void OnGrab()
    {
        _rb.isKinematic = true;
        EventBus.Send(new OnObjectGrabbedEvent() { Instance = this});
        _audioSource.PlayOneShot(_grabSound);
    }

    public void OnDrop()
    {
        _rb.isKinematic = false;
        EventBus.Send(new OnObjectDroppedEvent() { Instance = this});
        _audioSource.PlayOneShot(_dropSound);
    }

    public void OnPlaced()
    {
        _audioSource.PlayOneShot(_dropSound);
    }

    private void OnCollisionEnter(Collision other)
    {
        _audioSource.pitch = Random.Range(.9f, 1.1f);
        _audioSource.PlayOneShot(_hitSound);
    }


    // Editor Help Functions

    [ContextMenu("SetMaterial")]
    private void SetMaterial()
    {
        var rend = GetComponentInChildren<Renderer>();

        rend.material = _itemSettings.colorMaterial;
    }
}
