namespace MinishootRandomizer;

public enum ArchipelagoItemCategory
{
    Filler,
    Progression,
    Useful,
    Trap,
    SkipBalancing,
    ProgressionSkipBalancing,
}

public class ArchipelagoItemData
{
    private long _itemId;
    private string _itemName;
    private ArchipelagoItemCategory _category;
    private string _slotName;
    private bool _isLocal;

    public long ItemId => _itemId;
    public string ItemName => _itemName;
    public ArchipelagoItemCategory Category => _category;
    public string SlotName => _slotName;
    public bool IsLocal => _isLocal;

    public ArchipelagoItemData(long itemId, string itemName, ArchipelagoItemCategory category, string slotName, bool isLocal = false)
    {
        _itemId = itemId;
        _itemName = itemName;
        _category = category;
        _slotName = slotName;
        _isLocal = isLocal;
    }
}
