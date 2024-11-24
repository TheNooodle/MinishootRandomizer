using UnityEngine;

namespace MinishootRandomizer;

public class ReplaceComponentAction<T, U> : IPatchAction where T : MonoBehaviour where U : MonoBehaviour
{
    private readonly GameObject _gameObject;

    public delegate void OnComponentAddedHandler(U component);
    public event OnComponentAddedHandler OnComponentAdded;

    public ReplaceComponentAction(GameObject gameObject)
    {
        _gameObject = gameObject;
    }

    public void Dispose()
    {
        U replacementComponent = _gameObject.GetComponent<U>();
        if (replacementComponent != null)
        {
            GameObject.Destroy(replacementComponent);
        }

        T originalComponent = _gameObject.GetComponent<T>();
        if (originalComponent != null)
        {
            originalComponent.enabled = true;
        }
    }

    public void Patch()
    {
        T originalComponent = _gameObject.GetComponent<T>();
        if (originalComponent != null)
        {
            originalComponent.enabled = false;
        }

        U replacementComponent = _gameObject.GetComponent<U>();
        if (replacementComponent == null)
        {
            replacementComponent = _gameObject.AddComponent<U>();
            OnComponentAdded?.Invoke(replacementComponent);
        }
    }

    public void Unpatch()
    {
        // no-op
    }
}
