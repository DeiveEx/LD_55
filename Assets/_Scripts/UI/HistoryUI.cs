using System;
using System.Collections.Generic;
using Ignix.EventBusSystem;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class HistoryUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private RectTransform _entryParent;
    [SerializeField] private TMP_Text _entryPrefab;
    [SerializeField] private Color _correctColor = Color.green;
    [SerializeField] private Color _wrongColor = Color.red;
    [SerializeField] private Color _misplacedColor = Color.yellow;

    private List<TMP_Text> _entries = new();
    private RectTransform _myRectTransform;
    private Canvas _canvas;

    private IEventBus EventBus => GameManager.Instance.EventBus;

    private void Awake()
    {
        _myRectTransform = transform as RectTransform;
        _canvas = GetComponentInParent<Canvas>();
        
        EventBus.Register<OnHoverHistoryObjectChangedEvent>(OnHoverChanged);
    }

    private void OnDestroy()
    {
        EventBus.Unregister<OnHoverHistoryObjectChangedEvent>(OnHoverChanged);
    }

    private void Update()
    {
        //Position on Mouse
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, Mouse.current.position.value, _canvas.worldCamera, out var pos);
        transform.position = _canvas.transform.TransformPoint(pos);
    }

    private void OnHoverChanged(OnHoverHistoryObjectChangedEvent args)
    {
        if (args.HistoryEntry == null)
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
            return;
        }
        
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
        
        ClearEntries();

        for (int i = 0; i < args.HistoryEntry.sequence.Length; i++)
        {
            var item = args.HistoryEntry.sequence[i];
            var textColor = GetTextColorForPoint(args.HistoryEntry.points[i]);
            
            var entryName = Instantiate(_entryPrefab, _entryParent);
            entryName.text = $"<color=#{textColor}>{item.name}</color>";
            _entries.Add(entryName);
        }
    }

    private void ClearEntries()
    {
        foreach (var entry in _entries)
        {
            Destroy(entry.gameObject);
        }
        
        _entries.Clear();
    }

    private string GetTextColorForPoint(int point)
    {
        switch (point)
        {
            case 1:
                return ColorUtility.ToHtmlStringRGB(_correctColor);
            case 0:
                return ColorUtility.ToHtmlStringRGB(_misplacedColor);
            case -1:
                return ColorUtility.ToHtmlStringRGB(_wrongColor);
            default:
                return ColorUtility.ToHtmlStringRGB(Color.white);
        }
    }
}
