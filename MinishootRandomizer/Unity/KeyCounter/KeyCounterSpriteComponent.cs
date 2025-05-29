using UnityEngine;
using UnityEngine.UI;

namespace MinishootRandomizer;

public class KeyCounterSpriteComponent : MonoBehaviour
{
    private KeyStatus _status = KeyStatus.Missing;

    private CanvasRenderer _canvasRenderer;
    private Image _tickImage;

    void Start()
    {
        _canvasRenderer = GetComponent<CanvasRenderer>();
        _tickImage = transform.Find("Tick")?.GetComponent<Image>();
    }

    void Update()
    {
        switch (_status)
        {
            case KeyStatus.Missing:
                _canvasRenderer.SetColor(Color.black);
                _tickImage.enabled = false;
                break;
            case KeyStatus.Available:
                _canvasRenderer.SetColor(Color.white);
                _tickImage.enabled = false;
                break;
            case KeyStatus.Used:
                _canvasRenderer.SetColor(Color.white);
                _tickImage.enabled = true;
                break;
        }
    }

    public void SetStatus(KeyStatus status)
    {
        _status = status;
    }
}
