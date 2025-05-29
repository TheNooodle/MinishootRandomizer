using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MinishootRandomizer;

public class FileSpriteProvider : ISpriteProvider
{
    private readonly ISpriteProvider _innerProvider;
    private readonly string _rootPath;

    private Dictionary<string, SpriteFileData> _spriteFiles = new Dictionary<string, SpriteFileData>()
    {
        { "Archipelago", new SpriteFileData("archipelago.png", 128, 128, 0.7f) },
        { "ArchipelagoArrowUp", new SpriteFileData("archipelago_arrow_up.png", 128, 128, 0.7f) },
        { "ArchipelagoGrayscale", new SpriteFileData("archipelago_grayscale.png", 128, 128, 0.7f) },
        { "LocationMarker", new SpriteFileData("location_marker.png", 116, 180)},
        { "LocationMarkerSimple", new SpriteFileData("location_marker_simple.png", 116, 180)},
        { "NpcMarker", new SpriteFileData("npc_marker.png", 148, 172)},
        { "NpcMarkerSimple", new SpriteFileData("npc_marker_simple.png", 148, 172)},
        { "PrimordialScarabDialog", new SpriteFileData("prim_scarab_dialog.png", 140, 116)},
        { "Scarab", new SpriteFileData("scarab.png", 108, 108)},
        { "ScarabMarker", new SpriteFileData("scarab_marker.png", 132, 156)},
        { "ScarabMarkerSimple", new SpriteFileData("scarab_marker_simple.png", 132, 156)},
        { "SkullMarker", new SpriteFileData("skull_marker.png", 160, 188) },
        { "SkullMarkerSimple", new SpriteFileData("skull_marker_simple.png", 160, 188) },
        { "Spirit", new SpriteFileData("spirit.png", 51, 73)},
        { "SpiritMarker", new SpriteFileData("spirit_marker.png", 148, 172)},
        { "SpiritMarkerSimple", new SpriteFileData("spirit_marker_simple.png", 148, 172)},
        { "SuperCrystal", new SpriteFileData("super_crystal.png", 92, 92, 1f, -20f) },
        { "Tick", new SpriteFileData("tick.png", 84, 76) },
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
        string resourceName = _rootPath + "." + spriteFileData.FileName;
        using Stream stream = StreamFactory.CreateStream(resourceName);
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
