namespace MinishootRandomizer;

public class TowerHandler
{
    private readonly IRandomizerEngine _randomizerEngine;
    private readonly IItemRepository _itemRepository;

    public TowerHandler(
        IRandomizerEngine randomizerEngine,
        IItemRepository itemRepository
    )
    {
        _randomizerEngine = randomizerEngine;
        _itemRepository = itemRepository;
    }

    public int GetRequiredSpiritCount()
    {
        if (!_randomizerEngine.IsRandomized())
        {
            return 8; // Default count if not randomized
        }

        SpiritTowerRequirement requirement = _randomizerEngine.GetSetting<SpiritTowerRequirement>();
        return requirement.Value;
    }

    public bool HaveSpirit(int spiritIndex)
    {
        if (!_randomizerEngine.IsRandomized())
        {
            return WorldState.Get("NpcTiny" + spiritIndex);
        }

        SpiritItem spiritItem = (SpiritItem)_itemRepository.Get(Item.Spirit);
        return spiritIndex < spiritItem.GetOwnedQuantity();
    }
}
