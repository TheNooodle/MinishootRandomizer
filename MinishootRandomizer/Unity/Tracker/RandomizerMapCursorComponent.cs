using UnityEngine;
using UnityEngine.UI;

namespace MinishootRandomizer;

// Shows the game's custom cursor at the center of the screen when the map is open and
// the active input device is not keyboard/mouse (the cursor is then used as the map
// selection point). Otherwise, it stays out of the way: the game's own CustomCursor
// component takes back control on the next frame by itself.
public class RandomizerMapCursorComponent : MonoBehaviour
{
    private IObjectFinder _objectFinder;
    private ILogger _logger = new NullLogger();
    private Map _map = null;

    private bool _cursorSearched = false;
    private Image _cursorVisual = null;
    private Camera _cursorCamera = null;
    private Vector3 _cursorOffset = Vector3.zero;

    void Awake()
    {
        _objectFinder = Plugin.ServiceContainer.Get<IObjectFinder>();
        _logger = Plugin.ServiceContainer.Get<ILogger>() ?? new NullLogger();
        _map = GetComponentInParent<Map>();
    }

    // LateUpdate runs after CustomCursor.Update in the same frame, so our position and
    // visibility overrides are applied on top of the game's own logic.
    void LateUpdate()
    {
        if (DeviceManager.IsKeyboard)
        {
            return;
        }
        if (_map == null || !_map.IsOpen)
        {
            return;
        }

        if (!_cursorSearched)
        {
            FindCustomCursor();
        }

        if (_cursorVisual == null)
        {
            return;
        }

        _cursorVisual.enabled = true;

        // Same world-plane placement logic as the game's CustomCursor (z is flattened to 0).
        Vector3 worldCenter = _cursorCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0.0f));
        _cursorVisual.transform.position = new Vector3(worldCenter.x, worldCenter.y, 0.0f) + _cursorOffset;
    }

    private void FindCustomCursor()
    {
        _cursorSearched = true;

        GameObject cursorObject = _objectFinder.FindObject(new ByComponent(typeof(CustomCursor)));
        if (cursorObject == null)
        {
            _logger.LogError("Could not find CustomCursor object for the map cursor");
            return;
        }

        CustomCursor customCursor = cursorObject.GetComponent<CustomCursor>();
        _cursorVisual = ReflectionHelper.GetPrivateFieldValue<Image>(customCursor, "cursorVisual");
        _cursorCamera = ReflectionHelper.GetPrivateFieldValue<Camera>(customCursor, "cam");
        _cursorOffset = ReflectionHelper.GetPrivateFieldValue<Vector3>(customCursor, "offset");

        if (_cursorVisual == null)
        {
            _logger.LogError("Could not find cursorVisual field on CustomCursor");
            return;
        }

        if (_cursorCamera == null)
        {
            _cursorCamera = Camera.main;
        }

        if (_cursorCamera == null)
        {
            _logger.LogError("Could not resolve the CustomCursor camera");
            _cursorVisual = null;
            return;
        }

        // The cursor was found: we will never need to search again.
        enabled = true;
    }
}
