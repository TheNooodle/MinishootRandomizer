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
        // @TODO: refactor to use the GameEventDispatcher and the GoalListener, instead of replacing the component.
        _randomizerEngine.CompleteGoal(CorrespondingGoal);
    }
}
