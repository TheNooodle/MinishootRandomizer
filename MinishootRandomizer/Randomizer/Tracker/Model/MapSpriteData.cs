using System.Numerics;

namespace MinishootRandomizer;

public class MapSpriteData
{
    public string SpriteName { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public float Scale { get; set; } = 1.0f;
    public Vector3 Center { get; set; } = Vector3.Zero;

    public MapSpriteData(string spriteName, int width, int height, float scale, Vector3 center)
    {
        SpriteName = spriteName;
        Width = width;
        Height = height;
        Scale = scale;
        Center = center;
    }
}
