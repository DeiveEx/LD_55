using DG.Tweening;
using TMPro;
using UnityEngine;

public class CurrentDayUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _fadeDuration = 0.5f;
    [SerializeField] private float _displayDuration = 0.5f;

    private void Awake()
    {
        _canvasGroup.alpha = 0;
    }

    public void SetCurrentDay(int currentTurn)
    {
        _text.text = $"DIA {currentTurn + 1}";

        var sequence = DOTween.Sequence();
        sequence.Append(_canvasGroup.DOFade(1, _fadeDuration));
        sequence.AppendInterval(_displayDuration);
        sequence.Append(_canvasGroup.DOFade(0, _fadeDuration));

        sequence.Play();
    }
}
