using DG.Tweening;
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

    private void Start()
    {
        SetArrowToSection(Random.Range(0, _sectionsAmount));
    }

    private void Update()
    {
        if(Keyboard.current.qKey.wasPressedThisFrame)
            AddPoint(-1);
        
        if(Keyboard.current.wKey.wasPressedThisFrame)
            AddPoint(0);
        
        if(Keyboard.current.eKey.wasPressedThisFrame)
            AddPoint(1);
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
