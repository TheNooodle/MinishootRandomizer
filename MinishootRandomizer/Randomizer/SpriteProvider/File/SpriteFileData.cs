namespace MinishootRandomizer;

public class SpriteFileData
{
    private string _fileName;
    private int _width;
    private int _height;
    private float _scale = 1.0f;
    private float _angleOffset = 0.0f;

    public string FileName => _fileName;
    public int Width => _width;
    public int Height => _height;
    public float Scale => _scale;
    public float AngleOffset => _angleOffset;

    public SpriteFileData(string fileName, int width, int height, float scale = 1.0f, float angleOffset = 0.0f)
    {
        _fileName = fileName;
        _width = width;
        _height = height;
        _scale = scale;
        _angleOffset = angleOffset;
    }
}
