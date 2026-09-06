using UnityEngine;

namespace MinishootRandomizer;

public class ForceSizeDeltaComponent : MonoBehaviour
{
    private RectTransform _rectTransform;

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (_rectTransform != null)
        {
            _rectTransform.sizeDelta = new Vector2(_rectTransform.localScale.x, _rectTransform.localScale.y);
        }
    }
}
