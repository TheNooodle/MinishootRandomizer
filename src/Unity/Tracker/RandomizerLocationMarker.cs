using System.Collections.Generic;
using UnityEngine;

namespace MinishootRandomizer;

public class RandomizerLocationMarker : MonoBehaviour, IActivationChecker
{
    private IRandomizerEngine _randomizerEngine;
    private ILogicChecker _logicChecker;

    private List<Location> _locations = new List<Location>();
    private GameObject _sprite = null;
    private bool _mustShow = false;

    void Awake()
    {
        _randomizerEngine = Plugin.ServiceContainer.Get<IRandomizerEngine>();
        _logicChecker = Plugin.ServiceContainer.Get<ILogicChecker>();
    }

    public void SetLocations(List<Location> locations)
    {
        _locations = locations;
    }

    public void SetSprite(GameObject sprite)
    {
        _sprite = sprite;
    }

    void Update()
    {
        _mustShow = false;
        foreach (Location location in _locations)
        {
            bool isChecked = _randomizerEngine.IsLocationChecked(location);
            bool isInLogic = _logicChecker.CheckLocationLogic(location) == LogicAccessibility.InLogic;
            if (!isChecked && isInLogic)
            {
                _mustShow = true;
                break;
            }
        }

        if (_mustShow)
        {
            ShowMarker();
        }
        else
        {
            HideMarker();
        }
    }

    private void HideMarker()
    {
        if (_sprite != null)
        {
            _sprite.SetActive(false);
        }
    }

    private void ShowMarker()
    {
        if (_sprite != null)
        {
            _sprite.SetActive(true);
        }
    }

    public bool CheckActivation()
    {
        // Location markers must always be activated because :
        // - Their visibility is controlled by an ancestor object
        // - The actual logic of the marker is controlled by the Update method, which controls the visibility of the child sprite object.
        return true;
    }
}
