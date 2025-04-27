using UnityEngine;

namespace MinishootRandomizer;

public class RandomizerBossComponent : MonoBehaviour
{
    private IRandomizerEngine _randomizerEngine;
    private EventDestroyableComponent _eventDestroyableComponent;

    public Goals CorrespondingGoal;

    void Awake()
    {
        _randomizerEngine = Plugin.ServiceContainer.Get<IRandomizerEngine>();
        _eventDestroyableComponent = GetComponent<EventDestroyableComponent>();

        _eventDestroyableComponent.Destroyed += HandleCompleteGoal;
    }

    void OnDestroy()
    {
        _eventDestroyableComponent.Destroyed -= HandleCompleteGoal;
    }

    private void HandleCompleteGoal()
    {
        _randomizerEngine.CompleteGoal(CorrespondingGoal);
    }
}
