using System;

namespace MinishootRandomizer;

public class MarkerSpriteInfo
{
    private readonly string _spriteIdentifier;
    private readonly Tuple<float, float> _scale;

    public string SpriteIdentifier => _spriteIdentifier;
    public Tuple<float, float> Scale => _scale;

    public MarkerSpriteInfo(string spriteIdentifier, Tuple<float, float> scale)
    {
        _spriteIdentifier = spriteIdentifier;
        _scale = scale;
    }
}
