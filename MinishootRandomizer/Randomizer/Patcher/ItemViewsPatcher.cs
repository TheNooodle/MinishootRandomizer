using System.Collections.Generic;
using UnityEngine;

namespace MinishootRandomizer;

public class ItemViewsPatcher
{
    private readonly IRandomizerEngine _randomizerEngine;
    private readonly IItemViewFactory _itemViewFactory;
    private readonly ILogger _logger;

    bool _isPatched = false;
    private List<IPatchAction> _patchActions = new();

    private static readonly List<ItemView> _splitSurfItemViews = new()
    {
        new ItemView("PalmTree", Item.SurfNormal, new Vector2(-52, -27), new Vector2(30, 30), new ByName("SkillViewHover")),
        new ItemView("BlueCrystal", Item.SurfBlue, new Vector2(-52, 29), new Vector2(30, 30), new ByName("SkillViewHover")),
        new ItemView("BlobEnemy", Item.SurfSoiled, new Vector2(0, 57), new Vector2(30, 30), new ByName("SkillViewHover")),
        new ItemView("DarkCrystal", Item.SurfDungeon, new Vector2(52, 29), new Vector2(30, 30), new ByName("SkillViewHover")),
        new ItemView("CityBuildingTop", Item.SurfGold, new Vector2(52, -27), new Vector2(30, 30), new ByName("SkillViewHover"))
    };

    private static readonly List<ItemView> _splitSupershotItemViews = new()
    {
        new ItemView("Rock", Item.Blastshot, new Vector2(-52, 0), new Vector2(30, 30), new ByName("SkillViewSupershot")),
        new ItemView("TorchLit", Item.Flameshot, new Vector2(52, 0), new Vector2(30, 30), new ByName("SkillViewSupershot"))
    };

    public ItemViewsPatcher(IRandomizerEngine randomizerEngine, IItemViewFactory itemViewFactory, ILogger logger)
    {
        _randomizerEngine = randomizerEngine;
        _itemViewFactory = itemViewFactory;
        _logger = logger;
    }

    public void OnEnteringGameLocation(string locationName)
    {
        if (!_randomizerEngine.IsRandomized())
        {
            return;
        }

        if (!_isPatched)
        {
            Patch();
            _isPatched = true;
        }
    }

    public void OnExitingGame()
    {
        foreach (IPatchAction patchAction in _patchActions)
        {
            patchAction.Dispose();
        }
        _patchActions.Clear();
        _isPatched = false;
    }

    private void Patch()
    {
        // Split Surf icons
        SplitSurf splitSurf = _randomizerEngine.GetSetting<SplitSurf>();
        if (splitSurf.Enabled)
        {
            foreach (ItemView itemView in _splitSurfItemViews)
            {
                IPatchAction patchAction = new LoggableAction(
                    new CreateItemViewAction(itemView, _itemViewFactory),
                    _logger
                );
                patchAction.Patch();
                _patchActions.Add(patchAction);
            }
        }

        // Split Supershot icons
        SplitSupershot splitSupershot = _randomizerEngine.GetSetting<SplitSupershot>();
        if (splitSupershot.Enabled)
        {
            foreach (ItemView itemView in _splitSupershotItemViews)
            {
                IPatchAction patchAction = new LoggableAction(
                    new CreateItemViewAction(itemView, _itemViewFactory),
                    _logger
                );
                patchAction.Patch();
                _patchActions.Add(patchAction);
            }
        }
    }
}
