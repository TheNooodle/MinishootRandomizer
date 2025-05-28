using UnityEngine;

namespace MinishootRandomizer;

public interface ITextGameObjectFactory
{
    /// <summary>
    /// Creates a GameObject with a Text component, using the game's font.
    /// </summary>
    /// <param name="text">The text to display.</param>
    /// <returns>A GameObject containing a Text component with the specified text.</returns>
    GameObject CreateTextGameObject(string text);
}
