using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace MinishootRandomizer;

public class ShopReplacementPatcher
{
    private Dictionary<string, ItemType> npcItemTypeMapping = new Dictionary<string, ItemType>
    {
        { "Blacksmith", ItemType.NextPickup },
        { "ScarabCollector", ItemType.NextPickup },
        { "MercantHub", ItemType.SelectedPickup },
        { "MercantBusher", ItemType.SelectedPickup },
        { "MercantJar", ItemType.SelectedPickup },
        { "MercantFrogger", ItemType.SelectedPickup },
    };

    private readonly IRandomizerEngine _randomizerEngine;
    private readonly IObjectFinder _objectFinder;
    private readonly ILogger _logger = new NullLogger();

    private Dictionary<string, IPatchAction> _patchActions = new Dictionary<string, IPatchAction>();

    public ShopReplacementPatcher(IRandomizerEngine randomizerEngine, IObjectFinder objectFinder, ILogger logger = null)
    {
        _randomizerEngine = randomizerEngine;
        _objectFinder = objectFinder;
        _logger = logger ?? new NullLogger();
    }

    public void OnEnteringGameLocation(string locationName)
    {
        if (!_randomizerEngine.IsRandomized())
        {
            return;
        }

        Patch();
    }

    public void OnExitingGame()
    {
        foreach (IPatchAction patchAction in _patchActions.Values)
        {
            patchAction.Dispose();
        }
        _patchActions.Clear();
    }

    private void Patch()
    {
        GameObject[] npcObjects = _objectFinder.FindObjects(new ByComponent(typeof(Npc)));

        foreach (GameObject npcObject in npcObjects)
        {
            NpcInteraction npcInteraction = npcObject.GetComponent<NpcInteraction>();
            if (npcInteraction != null && npcItemTypeMapping.ContainsKey(npcObject.gameObject.name) && !_patchActions.ContainsKey(npcObject.gameObject.name))
            {
                ReplaceComponentAction<NpcInteraction, RandomizerNpcTradingInteraction> replaceComponentAction = new ReplaceComponentAction<NpcInteraction, RandomizerNpcTradingInteraction>(npcObject);
                replaceComponentAction.OnComponentAdded += (RandomizerNpcTradingInteraction randomizerNpcTradingInteraction) =>
                {
                    randomizerNpcTradingInteraction.ItemType = npcItemTypeMapping[npcObject.gameObject.name];
                };
                LoggableAction loggableAction = new LoggableAction(replaceComponentAction, _logger);
                _patchActions.Add(npcObject.gameObject.name, loggableAction);
                loggableAction.Patch();
            }
        }
    }
}
