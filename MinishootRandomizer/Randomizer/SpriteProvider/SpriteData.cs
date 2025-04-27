using UnityEngine;

namespace MinishootRandomizer;

public class SpriteData
{
    private Sprite _sprite;
    private float _scale = 1f;
    private float _angleOffset = 0f;

    public Sprite Sprite => _sprite;
    public float Scale => _scale;
    public float AngleOffset => _angleOffset;

    public SpriteData(Sprite sprite, float scale = 1f, float angleOffset = 0f)
    {
        _sprite = sprite;
        _scale = scale;
        _angleOffset = angleOffset;
    }

    public void ApplyTo(SpriteRenderer spriteRenderer)
    {
        spriteRenderer.sprite = _sprite;
        spriteRenderer.transform.localScale *= _scale;
        spriteRenderer.transform.Rotate(0, 0, _angleOffset);
    }
}
