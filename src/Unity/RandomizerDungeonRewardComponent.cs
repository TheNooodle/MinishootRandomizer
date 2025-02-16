using UnityEngine;

namespace MinishootRandomizer;

public class RandomizerDungeonRewardComponent : MonoBehaviour
{
    private IRandomizerEngine _randomizerEngine = null;
    private GameEventDispatcher _gameEventDispatcher = null;

    public Location Location { get; set; }
    public Item Item { get; set; }

    void Awake()
    {
        _randomizerEngine = Plugin.ServiceContainer.Get<IRandomizerEngine>();
        _gameEventDispatcher = Plugin.ServiceContainer.Get<GameEventDispatcher>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !_randomizerEngine.IsLocationChecked(Location))
        {
            Item.Collect();
            _randomizerEngine.CheckLocation(Location);
            _gameEventDispatcher.DispatchItemCollected(Item);
        }
    }
}
