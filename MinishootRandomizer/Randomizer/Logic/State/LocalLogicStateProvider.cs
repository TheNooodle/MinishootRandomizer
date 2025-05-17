using System;
using System.Collections.Generic;

namespace MinishootRandomizer;

public class LocalLogicStateProvider : ILogicStateProvider
{
    private readonly IItemRepository _itemRepository;
    private readonly IRandomizerEngine _randomizerEngine;

    public LocalLogicStateProvider(
        IItemRepository itemRepository,
        IRandomizerEngine randomizerEngine
    ) {
        _itemRepository = itemRepository;
        _randomizerEngine = randomizerEngine;
    }

    public LogicState GetLogicState()
    {
        LogicState logicState = new LogicState();
        ItemsPass(logicState);
        SettingsPass(logicState);

        return logicState;
    }

    private void ItemsPass(LogicState logicState)
    {
        List<Item> items = _itemRepository.GetAll();
        foreach (Item item in items)
        {
            int count = item.GetOwnedQuantity();
            if (count > 0)
            {
                logicState.SetItemCount(item, count);
            }
        }
    }

    private void SettingsPass(LogicState logicState)
    {
        List<ISetting> settings = _randomizerEngine.GetSettings();
        foreach (ISetting setting in settings)
        {
            logicState.SetSetting(setting);
        }
    }
}
