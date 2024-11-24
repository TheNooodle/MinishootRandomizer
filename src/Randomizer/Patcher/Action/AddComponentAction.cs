using UnityEngine;

namespace MinishootRandomizer;

public class AddComponentAction<T> : IPatchAction where T : MonoBehaviour
{
    private readonly GameObject _gameObject;

    public delegate void OnComponentAddedHandler(T component);
    public event OnComponentAddedHandler OnComponentAdded;

    public AddComponentAction(GameObject gameObject)
    {
        _gameObject = gameObject;
    }

    public void Dispose()
    {
        T component = _gameObject.GetComponent<T>();
        if (component != null)
        {
            GameObject.Destroy(component);
        }
    }

    public void Patch()
    {
        T component = _gameObject.GetComponent<T>();
        if (component == null)
        {
            component = _gameObject.AddComponent<T>();
            OnComponentAdded?.Invoke(component);
        }
    }

    public void Unpatch()
    {
        // no-op
    }
}
