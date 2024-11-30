using System;
using UnityEngine;

namespace MinishootRandomizer;

public class EventDestroyableComponent : MonoBehaviour
{
    private BaseDestroyable _baseDestroyable = null;
    private Destroyable _destroyable = null;
    private CoconutPile _coconutPile = null;
    private bool _isDestroyed = false;

    public event Action Destroyed;
    public event Action Restored;
    public bool IsDestroyed => _isDestroyed;

    void Awake()
    {
        _destroyable = GetComponent<Destroyable>();
        _coconutPile = GetComponent<CoconutPile>();
    }

    void Start()
    {
        if (_destroyable != null)
        {
            _destroyable.Destroyed += HandleDestroyed;
        }
        else
        {
            _baseDestroyable = GetComponent<BaseDestroyable>();
        }
    }

    void OnDestroy()
    {
        if (_destroyable != null)
        {
            _destroyable.Destroyed -= HandleDestroyed;
        }
    }

    private void HandleDestroyed()
    {
        _isDestroyed = true;
        Destroyed?.Invoke();
    }

    private void HandleRestore()
    {
        _isDestroyed = false;
        Restored?.Invoke();
    }

    void Update()
    {
        if (_baseDestroyable && _baseDestroyable.IsDestroyed && !_isDestroyed)
        {
            HandleDestroyed();
        }
        else if (_destroyable && !_destroyable.IsDestroyed && _isDestroyed)
        {
            HandleRestore();
        }
        else if (_baseDestroyable && !_baseDestroyable.IsDestroyed && _isDestroyed)
        {
            HandleRestore();
        }
        else if (_coconutPile)
        {
            bool areFloorCleared = true;
            CoconutFloor[] floors = GetComponentsInChildren<CoconutFloor>();
            foreach (CoconutFloor floor in floors)
            {
                if (!floor.IsCleared)
                {
                    areFloorCleared = false;
                    break;
                }
            }

            if (areFloorCleared && !_isDestroyed)
            {
                HandleDestroyed();
            }
            else if (!areFloorCleared && _isDestroyed)
            {
                HandleRestore();
            }
        }
    }
}
