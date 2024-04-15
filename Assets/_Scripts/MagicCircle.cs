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
        EventBus.Register<HighlightCodeEvent>(OnHighlight);
    }

    private void OnDestroy()
    {
        EventBus.Unregister<HighlightCodeEvent>(OnHighlight);
    }

    private void OnHighlight(HighlightCodeEvent args)
    {
        Debug.Log($"Code received: {string.Join(";", args.EntryHighlights.Select(x => x))}");
        for (int i = 0; i < args.EntryHighlights.Count; i++)
        {
            float current = _mesh.material.GetFloat($"_HighlightEdge{i}");
            float target = args.EntryHighlights[i] ? 1 : 0;
            int index = i;

            DOTween.To(value =>
            {
                _mesh.material.SetFloat($"_HighlightEdge{index}", value);
            }, current, target, _animDuration);
        }
    }
}
