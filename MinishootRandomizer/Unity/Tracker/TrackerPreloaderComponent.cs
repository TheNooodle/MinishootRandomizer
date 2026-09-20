using UnityEngine;

namespace MinishootRandomizer;

public class TrackerPreloaderComponent : MonoBehaviour
{
    private TrackerPreloader _trackerPreloader;

    void Awake()
    {
        _trackerPreloader = Plugin.ServiceContainer.Get<TrackerPreloader>();
    }

    void Update()
    {
        if (_trackerPreloader != null)
        {
            _trackerPreloader.Tick(Time.unscaledDeltaTime);
        }
    }
}
