using UnityEngine;

namespace MinishootRandomizer;

public class RandomizerDungeonRewardComponent : MonoBehaviour
{
    private IRandomizerEngine _randomizerEngine = null;

    public Location Location { get; set; }

    void Awake()
    {
        _randomizerEngine = Plugin.ServiceContainer.Get<IRandomizerEngine>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !_randomizerEngine.IsLocationChecked(Location))
        {
            _randomizerEngine.CheckLocation(Location);
        }
    }
}
