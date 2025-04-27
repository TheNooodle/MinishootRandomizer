using UnityEngine;

namespace MinishootRandomizer;

public class FloatyAnimationComponent : MonoBehaviour
{
    private float _speed = 1.0f;
    private float _amplitude = 1.0f;
    private float _offset = 0.0f;
    private float _initialY;

    void Start()
    {
        _initialY = transform.localPosition.y;
    }

    void Update()
    {
        _offset += Time.unscaledDeltaTime * _speed;
        _offset %= Mathf.PI * 2.0f; // Prevent overflow
        float sine = (Mathf.Sin(_offset) + 1.0f) / 2.0f;

        transform.localPosition = new Vector3(transform.localPosition.x, _initialY + (sine * _amplitude), transform.localPosition.z);
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }

    public void SetAmplitude(float amplitude)
    {
        _amplitude = amplitude;
    }
}
