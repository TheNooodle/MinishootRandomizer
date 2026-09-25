using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MinishootRandomizer;

// Displays the names of the Locations handled by the tracker marker currently pointed
// at by the player. Every frame, it finds the hovered marker (mouse position with
// keyboard/mouse, screen center with gamepad), gets its randomized Locations grouped
// by logic state (white: in logic, yellow: out of logic, dark gray: inaccessible),
// excluding the checked ones, and shows their identifiers sorted within each group.
public class RandomizerLocationListComponent : MonoBehaviour
{
    // Line colors per display state (TMP rich-text hex, without alpha).
    private const string IN_LOGIC_COLOR = "FFFFFF";     // white
    private const string OUT_OF_LOGIC_COLOR = "FFD800"; // yellow
    private const string INACCESSIBLE_COLOR = "6E6E6E"; // dark gray

    // Group order in the displayed list: matches the enum declaration order.
    private enum DisplayState
    {
        InLogic,
        OutOfLogic,
        Inaccessible
    }

    private IRandomizerEngine _randomizerEngine;
    private ILocationLogicChecker _logicChecker;
    private ILogicStateProvider _logicStateProvider;
    private Map _map = null;
    private TextMeshProUGUI _text = null;
    private RandomizerTrackerMarkerComponent _hoveredMarker = null;

    void Awake()
    {
        _randomizerEngine = Plugin.ServiceContainer.Get<IRandomizerEngine>();
        _logicChecker = Plugin.ServiceContainer.Get<ILocationLogicChecker>();
        _logicStateProvider = Plugin.ServiceContainer.Get<ILogicStateProvider>();
        _text = GetComponent<TextMeshProUGUI>();
        _map = GetComponentInParent<Map>();

        if (_text != null)
        {
            _text.alignment = TextAlignmentOptions.TopRight;
            _text.color = Color.white;
            // Shrink the inherited Progress font size to 60% (rounded down).
            _text.fontSize = Mathf.FloorToInt(_text.fontSize * 0.6f);
            _text.text = "";
        }
    }

    void Update()
    {
        // The Map object is never deactivated (it only fades out), so the component
        // keeps running while the map is closed: we must not hover anything then.
        if (_map == null || !_map.IsOpen)
        {
            SetHoveredMarker(null);
            return;
        }

        SetHoveredMarker(FindHoveredMarker());
    }

    // Step 1: find the tracker marker currently under the pointer.
    private RandomizerTrackerMarkerComponent FindHoveredMarker()
    {
        Vector2 pointerPosition = GetPointerPosition();

        RandomizerTrackerMarkerComponent hoveredMarker = null;
        float closestDistance = float.MaxValue;

        foreach (RandomizerTrackerMarkerComponent marker in RandomizerTrackerMarkerComponent.GetInstances())
        {
            if (!marker.TryGetHoverableRect(out RectTransform rect, out Camera canvasCamera))
            {
                continue;
            }
            if (!RectTransformUtility.RectangleContainsScreenPoint(rect, pointerPosition, canvasCamera))
            {
                continue;
            }

            // On overlapping markers, keep the one whose center is closest to the pointer.
            Vector3 screenCenter = canvasCamera != null
                ? canvasCamera.WorldToScreenPoint(rect.position)
                : rect.position;
            float distance = (screenCenter - (Vector3)pointerPosition).sqrMagnitude;
            if (distance < closestDistance)
            {
                closestDistance = distance;
                hoveredMarker = marker;
            }
        }

        return hoveredMarker;
    }

    private static Vector2 GetPointerPosition()
    {
        if (DeviceManager.IsKeyboard)
        {
            // The game runs with the new Input System only: legacy Input.mousePosition
            // would throw. MouseHelper.Position is the game's own wrapper.
            return MouseHelper.Position;
        }

        // With a gamepad, the map is panned under a fixed screen-center selection point.
        return new Vector2(Screen.width / 2.0f, Screen.height / 2.0f);
    }

    // Steps 2 & 3: when the hovered marker changes, get its Location names and display them.
    private void SetHoveredMarker(RandomizerTrackerMarkerComponent marker)
    {
        if (marker == _hoveredMarker)
        {
            return;
        }

        _hoveredMarker = marker;

        if (_text == null)
        {
            return;
        }

        _text.text = BuildDisplayText(marker);
    }

    // Only the Locations whose pool is randomized are listed, excluding the checked ones,
    // grouped by logic state (in logic, out of logic, inaccessible) and sorted
    // alphabetically by identifier within each group. Called when the hovered marker
    // changes, not every frame.
    private string BuildDisplayText(RandomizerTrackerMarkerComponent marker)
    {
        if (marker == null)
        {
            return "";
        }

        IReadOnlyList<Location> locations = marker.Locations;
        if (locations == null || locations.Count == 0)
        {
            return "";
        }

        List<LocationPool> locationPools = _randomizerEngine.GetLocationPools();
        LogicState logicState = _logicStateProvider.GetLogicState();

        // Enum members are declared in display group order.
        Dictionary<DisplayState, List<string>> linesByState = new Dictionary<DisplayState, List<string>>()
        {
            { DisplayState.InLogic, new List<string>() },
            { DisplayState.OutOfLogic, new List<string>() },
            { DisplayState.Inaccessible, new List<string>() },
        };

        foreach (Location location in locations)
        {
            if (_randomizerEngine.IsLocationChecked(location))
            {
                continue;
            }
            if (!locationPools.Contains(location.Pool))
            {
                continue;
            }
            DisplayState state = GetDisplayState(location, logicState);
            linesByState[state].Add(location.Identifier);
        }

        List<string> lines = new List<string>();
        // Enum.GetValues follows the declaration order of DisplayState, which defines
        // the group order (a Dictionary enumeration order would not be guaranteed).
        foreach (DisplayState state in (DisplayState[])Enum.GetValues(typeof(DisplayState)))
        {
            List<string> group = linesByState[state];
            if (group.Count == 0)
            {
                continue;
            }
            group.Sort(StringComparer.Ordinal);
            string color = GetDisplayStateColor(state);
            foreach (string identifier in group)
            {
                lines.Add($"<color=#{color}>{identifier}</color>");
            }
        }

        return string.Join("\n", lines);
    }

    // Checked locations are filtered out before this is called (see BuildDisplayText).
    private DisplayState GetDisplayState(Location location, LogicState logicState)
    {
        LogicAccessibility accessibility = _logicChecker.CheckLocationLogic(logicState, location);
        return accessibility switch
        {
            LogicAccessibility.InLogic => DisplayState.InLogic,
            LogicAccessibility.OutOfLogic => DisplayState.OutOfLogic,
            _ => DisplayState.Inaccessible
        };
    }

    private static string GetDisplayStateColor(DisplayState state)
    {
        return state switch
        {
            DisplayState.InLogic => IN_LOGIC_COLOR,
            DisplayState.OutOfLogic => OUT_OF_LOGIC_COLOR,
            _ => INACCESSIBLE_COLOR
        };
    }
}
