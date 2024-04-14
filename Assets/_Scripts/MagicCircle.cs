using System.Linq;
using DG.Tweening;
using Ignix.EventBusSystem;
using UnityEngine;

public class MagicCircle : MonoBehaviour
{
    [SerializeField] private MeshRenderer _mesh;
    [SerializeField] private float _animDuration = .5f;

    private IEventBus EventBus => GameManager.Instance.EventBus;

    private void Awake()
    {
        EventBus.Register<HighlighCodeEvent>(OnHighlight);
    }

    private void OnDestroy()
    {
        EventBus.Unregister<HighlighCodeEvent>(OnHighlight);
    }

    private void OnHighlight(HighlighCodeEvent args)
    {
        Debug.Log($"Code received: {string.Join(";", args.ShouldHighlight.Select(x => x))}");
        for (int i = 0; i < args.ShouldHighlight.Count; i++)
        {
            float current = _mesh.material.GetFloat($"_HighlightEdge{i}");
            float target = args.ShouldHighlight[i] ? 1 : 0;
            int index = i;

            DOTween.To(value =>
            {
                _mesh.material.SetFloat($"_HighlightEdge{index}", value);
            }, current, target, _animDuration);
        }
    }
}
