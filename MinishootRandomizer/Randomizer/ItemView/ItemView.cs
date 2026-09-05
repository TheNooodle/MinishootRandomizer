using UnityEngine;

namespace MinishootRandomizer;

public class ItemView
{
    private string _spriteDataIdentifier;
    private string _itemIdentifier;
    private Vector2 _offset;
    private Vector2 _size;
    private ISelector _parentSelector;

    public string SpriteDataIdentifier => _spriteDataIdentifier;
    public string ItemIdentifier => _itemIdentifier;
    public Vector2 Offset => _offset;
    public Vector2 Size => _size;
    public ISelector ParentSelector => _parentSelector;

    public ItemView(string spriteDataIdentifier, string itemIdentifier, Vector2 offset, Vector2 size, ISelector parentSelector)
    {
        _spriteDataIdentifier = spriteDataIdentifier;
        _itemIdentifier = itemIdentifier;
        _offset = offset;
        _size = size;
        _parentSelector = parentSelector;
    }
}
