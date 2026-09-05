using UnityEngine;

namespace MinishootRandomizer;

public class RandomizerMapSpriteComponent : MonoBehaviour
{
    public TrackerMap Map;

    private CanvasRenderer _canvasRenderer;

    void Awake()
    {
        _canvasRenderer = GetComponent<CanvasRenderer>();
    }

    void Update()
    {
        if (Map == null)
        {
            return;
        }

        TrackerMap currentMap = RandomizerMapComponent.CurrentMap;
        if (currentMap == null)
        {
            _canvasRenderer.SetAlpha(0f);
            return;
        }

        _canvasRenderer.SetAlpha(currentMap == Map ? 1f : 0f);
    }
}
