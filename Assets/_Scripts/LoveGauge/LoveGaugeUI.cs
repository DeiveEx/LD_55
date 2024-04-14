using System;
using DG.Tweening;
using Ignix.EventBusSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LoveGaugeUI : MonoBehaviour
{
    [SerializeField] private Image _arrowHead;
    [SerializeField] private int _sectionsAmount = 8;
    [SerializeField] private float _angleeOffset = 90;
    [SerializeField] private float _animDuration = .5f;
    [SerializeField] private Ease _animEase;
    [SerializeField] private float _shakeStrenght;
    [SerializeField] private int _shakeFrequency;

    private int _currentSectionIndex;

    public int CurrentSectionIndex => _currentSectionIndex;
    public int SectionsAmount => _sectionsAmount;

    private IEventBus EventBus => GameManager.Instance.EventBus;

    private void Awake()
    {
        EventBus.Register<OnPointAddedEvent>(OnPointAdded);
    }

    private void OnDestroy()
    {
        EventBus.Unregister<OnPointAddedEvent>(OnPointAdded);
    }
    
    private void OnPointAdded(OnPointAddedEvent args)
    {
        AddPoint(args.Amount, true);
    }

    public void SetArrowToSection(int sectionIndex, bool animate = false)
    {
        var previousIndex = _currentSectionIndex;
        _currentSectionIndex = sectionIndex % _sectionsAmount;
        Vector3 rot = new Vector3();

        float step = 360 / (float)_sectionsAmount;
        rot.z = _currentSectionIndex * step;
        rot.z += _angleeOffset;

        _arrowHead.transform.DOKill();

        if (animate)
        {
            //If the index didn't chane, just shake the arrow
            if (previousIndex == _currentSectionIndex)
                _arrowHead.transform.DOShakeRotation(_animDuration, new Vector3(0, 0, _shakeStrenght), _shakeFrequency).SetEase(_animEase);
            else
                _arrowHead.transform.DORotateQuaternion(Quaternion.Euler(rot), _animDuration).SetEase(_animEase);
        }
        else
        {
            _arrowHead.transform.rotation = Quaternion.Euler(rot);
        }
    }

    public void AddPoint(int direction, bool animate = true)
    {
        int dir = 0;

        if (Mathf.Abs(direction) > 0)
            dir = (int)Mathf.Sign(direction);

        SetArrowToSection(_currentSectionIndex + dir, animate);
    }
}
