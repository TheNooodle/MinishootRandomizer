using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace MinishootRandomizer;

public class FileSpriteProvider : ISpriteProvider
{
    private readonly ISpriteProvider _innerProvider;
    private readonly string _rootPath;

    private Dictionary<string, SpriteFileData> _spriteFiles = new Dictionary<string, SpriteFileData>()
    {
        { "Archipelago", new SpriteFileData("archipelago.png", 128, 128, 0.7f) },
        { "Scarab", new SpriteFileData("scarab.png", 108, 108)}
    };

    public FileSpriteProvider(ISpriteProvider innerProvider, string rootPath)
    {
        _innerProvider = innerProvider;
        _rootPath = rootPath;
    }

    public SpriteData GetSprite(string identifier)
    {
        if (!_spriteFiles.ContainsKey(identifier))
        {
            return _innerProvider.GetSprite(identifier);
        }

        SpriteFileData spriteFileData = _spriteFiles[identifier];
        Assembly assembly = Assembly.GetExecutingAssembly();
        using Stream stream = assembly.GetManifestResourceStream(_rootPath + "." + spriteFileData.FileName);
        Texture2D texture = new Texture2D(spriteFileData.Width, spriteFileData.Height, TextureFormat.DXT1, false);
        byte[] buffer = new byte[stream.Length];
        stream.Read(buffer, 0, buffer.Length);
        ImageConversion.LoadImage(texture, buffer);

        Sprite sprite = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f),
            100f,
            0,
            SpriteMeshType.FullRect,
            Vector4.zero,
            false
        );

        return new SpriteData(sprite, spriteFileData.Scale, spriteFileData.AngleOffset);
    }
}
