using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MinishootRandomizer;

public class RandomizerMapComponent : MonoBehaviour
{
    private IRandomizerEngine _randomizerEngine;
    private IProgressionStorage _progressionStorage;
    private IObjectFinder _objectFinder;
    private ITrackerMapProvider _trackerMapProvider;
    private ITrackerMapAssetsInitializer _assetsInitializer;
    private CurrentMapHandler _currentMapHandler;
    private ILogger _logger = new NullLogger();

    private GameObject _contentGameObject = null;
    private GameObject _playerViewGameObject = null;
    private GameObject _mapTitleLayoutObject = null;
    private TextMeshProUGUI _mapTitleShadowText = null;
    private TextMeshProUGUI _mapTitleText = null;
    private TextMeshProUGUI _progressShadowText = null;
    private TextMeshProUGUI _progressText = null;
    private GameObject _goalGameObject = null;
    private TextMeshProUGUI _goalText = null;
    private bool _isInitialized = false;

    private static TrackerMap currentMap = null;
    public static TrackerMap CurrentMap => currentMap;
    public static Vector3 DebugPosition = Vector3.zero;
    public static float DebugScale = 0.0f;
    public static string ForceCurrentMapIdentifier = null;

    void Awake()
    {
        if (!_isInitialized)
        {
            Initialize();
        }
    }

    // We separate initialization from the Unity lifecycle methods to allow event handling.
    private void Initialize()
    {
        _randomizerEngine = Plugin.ServiceContainer.Get<IRandomizerEngine>();
        _progressionStorage = Plugin.ServiceContainer.Get<IProgressionStorage>();
        _objectFinder = Plugin.ServiceContainer.Get<IObjectFinder>();
        _trackerMapProvider = Plugin.ServiceContainer.Get<ITrackerMapProvider>();
        _assetsInitializer = Plugin.ServiceContainer.Get<ITrackerMapAssetsInitializer>();
        _currentMapHandler = Plugin.ServiceContainer.Get<CurrentMapHandler>();
        _logger = Plugin.ServiceContainer.Get<ILogger>() ?? new NullLogger();
        _isInitialized = true;

        PlayerInputs.PowerSlow += TryPreviousMap;
        PlayerInputs.PowerBomb += TryNextMap;
    }

    void OnDestroy()
    {
        PlayerInputs.PowerSlow -= TryPreviousMap;
        PlayerInputs.PowerBomb -= TryNextMap;
        currentMap = null;
    }
    
    public void TryPreviousMap()
    {
        TrySwitchMap(-1);
    }

    public void TryNextMap()
    {
        TrySwitchMap(1);
    }

    private void TrySwitchMap(int direction)
    {
        if (!gameObject.activeSelf)
        {
            return;
        }

        IReadOnlyList<string> mapIdentifiers = _assetsInitializer.MapIdentifiers;
        int wantedIndex;
        int currentIndex = -1;
        if (currentMap == null)
        {
            // If the current map is null, we start from the "Overworld" map.
            for (int i = 0; i < mapIdentifiers.Count; i++)
            {
                if (mapIdentifiers[i] == "Overworld")
                {
                    currentIndex = i;
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < mapIdentifiers.Count; i++)
            {
                if (mapIdentifiers[i] == currentMap.Identifier)
                {
                    currentIndex = i;
                    break;
                }
            }
        }
        if (currentIndex == -1)
        {
            wantedIndex = 0;
        }
        else
        {
            wantedIndex = (currentIndex + direction + mapIdentifiers.Count) % mapIdentifiers.Count;
        }

        TrackerMap wantedMap = _trackerMapProvider.GetTrackerMap(mapIdentifiers[wantedIndex]);
        if (wantedMap == null)
        {
            _logger.LogError("Could not find map with identifier " + mapIdentifiers[wantedIndex]);
            return;
        }
        if (wantedMap != currentMap)
        {
            SetCurrentMap(wantedMap);
            Sounds.Play(Sfx.MenuNavigation, 1f, null, 0f, null);
        }
    }

    public void OnMapOpened()
    {
        if (!_isInitialized)
        {
            Initialize();
        }

        TrackerMap map = _currentMapHandler.GetCurrentMap();
        SetCurrentMap(map);
    }

    public void OnAfterMapOpened()
    {
        // Update the input prompts
        HandleMapTitleLayout();

        // Update the progress
        HandleProgress();

        // Update the goal
        HandleGoal();
    }

    private void SetCurrentMap(TrackerMap map)
    {
        currentMap = map;

        // If the map has not been initialized yet, we create its objects (map image + markers).
        // Assets may already be fully or partially created by the background preloader:
        // InitializeNow resumes from the units already processed.
        if (map != null && !_assetsInitializer.IsInitialized(map))
        {
            _assetsInitializer.InitializeNow(map);
        }
        else if (map == null)
        {
            // Edge case: if the map is null and the Overworld map has not been initialized yet,
            // we initialize it now as it will be the map shown in this case.
            TrackerMap overworldMap = _trackerMapProvider.GetTrackerMap("Overworld");
            _assetsInitializer.InitializeNow(overworldMap);
        }

        // We handle the Overworld map special case (hiding/showing the original map elements).
        HandleOverworldMap(map == null || map.Identifier == "Overworld");

        // We hide the player sprite when not on the map the player is currently on.
        GameObject playerViewGameObject = GetPlayerViewGameObject();
        TrackerMap currentPlayerMap = _currentMapHandler.GetCurrentMap();
        if (playerViewGameObject != null)
        {
            playerViewGameObject.SetActive(currentPlayerMap != null && map != null && currentPlayerMap.Identifier == map.Identifier);
        }

        // We update the map title.
        TextMeshProUGUI mapTitleShadowText = GetMapTitleShadowText();
        TextMeshProUGUI mapTitleText = GetMapTitleText();
        if (mapTitleShadowText != null && mapTitleText != null)
        {
            string mapTitle = map != null ? (map.Name + " map") : "Overworld map";
            mapTitleShadowText.text = mapTitle.ToUpper();
            mapTitleText.text = mapTitle.ToUpper();
        }
    }

    private GameObject GetContentGameObject()
    {
        if (_contentGameObject == null)
        {
            foreach (Transform child in transform)
            {
                if (child.name == "Content")
                {
                    _contentGameObject = child.gameObject;
                    break;
                }
            }
        }

        return _contentGameObject;
    }

    private GameObject GetPlayerViewGameObject()
    {
        GameObject contentGameObject = GetContentGameObject();
        if (_playerViewGameObject == null && contentGameObject != null)
        {
            foreach (Transform child in contentGameObject.transform)
            {
                if (child.name == "PlayerView")
                {
                    _playerViewGameObject = child.gameObject;
                    break;
                }
            }
        }

        return _playerViewGameObject;
    }

    private void HandleOverworldMap(bool isOverworld)
    {
        // To handle the already existing Overworld map, we hide it when we show our own map.
        List<string> gameObjectNamesToHide = new List<string>()
        {
            "Border",
            "Fragments",
            "Connections",
            "Icons"
        };
        GameObject contentGameObject = GetContentGameObject();
        if (contentGameObject == null)
        {
            _logger.LogError("Could not find Content object in Map");
            return;
        }

        foreach (Transform child in contentGameObject.transform)
        {
            if (gameObjectNamesToHide.Contains(child.name))
            {
                // If the current map is the Overworld, we show the original map objects.
                // Otherwise, we hide them.
                child.gameObject.SetActive(isOverworld);
            }
        }
    }

    private GameObject GetMapTitleLayoutObject()
    {
        if (_mapTitleLayoutObject == null)
        {
            _mapTitleLayoutObject = _objectFinder.FindObject(new ByName("MapTitleLayout"));
            if (_mapTitleLayoutObject == null)
            {
                _logger.LogError("Could not find MapTitleLayout object");
            }
        }

        return _mapTitleLayoutObject;
    }

    private TextMeshProUGUI GetMapTitleShadowText()
    {
        if (_mapTitleShadowText == null)
        {
            GameObject mapTitleLayoutObject = GetMapTitleLayoutObject();
            if (mapTitleLayoutObject != null)
            {
                foreach (Transform child in mapTitleLayoutObject.transform)
                {
                    if (child.name == "TitleShadow")
                    {
                        _mapTitleShadowText = child.GetComponent<TextMeshProUGUI>();
                        break;
                    }
                }
            }
        }

        if (_mapTitleShadowText == null)
        {
            _logger.LogError("Could not find TitleShadow text component");
        }

        return _mapTitleShadowText;
    }

    private TextMeshProUGUI GetMapTitleText()
    {
        if (_mapTitleText == null)
        {
            TextMeshProUGUI mapTitleShadowText = GetMapTitleShadowText();
            if (mapTitleShadowText != null)
            {
                foreach (Transform child in mapTitleShadowText.transform)
                {
                    if (child.name == "Title")
                    {
                        _mapTitleText = child.GetComponent<TextMeshProUGUI>();
                        break;
                    }
                }
            }
        }

        if (_mapTitleText == null)
        {
            _logger.LogError("Could not find Title text component");
        }

        return _mapTitleText;
    }

    private TextMeshProUGUI GetProgressShadowText()
    {
        if (_progressShadowText == null)
        {
            GameObject progressShadowObject = _objectFinder.FindObject(new ByName("ProgressShadow"));
            if (progressShadowObject != null)
            {
                _progressShadowText = progressShadowObject.GetComponent<TextMeshProUGUI>();
            }
            else
            {
                _logger.LogError("Could not find ProgressShadow object");
            }
        }

        return _progressShadowText;
    }

    private TextMeshProUGUI GetProgressText()
    {
        if (_progressText == null)
        {
            GameObject progressObject = _objectFinder.FindObject(new ByName("Progress"));
            if (progressObject != null)
            {
                _progressText = progressObject.GetComponent<TextMeshProUGUI>();
            }
            else
            {
                _logger.LogError("Could not find Progress object");
            }
        }

        return _progressText;
    }

    private const float GoalYOffset = 50.0f;

    // We duplicate the Progress text object to get the same visual (font, color)
    // for the goal text, placed at the top center of the viewport.
    private GameObject GetGoalGameObject()
    {
        if (_goalGameObject == null)
        {
            GameObject progressObject = _objectFinder.FindObject(new ByName("Progress"));
            if (progressObject != null)
            {
                _goalGameObject = Instantiate(progressObject, transform);
                _goalGameObject.name = "Goal";

                RectTransform rectTransform = _goalGameObject.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    rectTransform.anchorMin = new Vector2(0.5f, 1.0f);
                    rectTransform.anchorMax = new Vector2(0.5f, 1.0f);
                    rectTransform.pivot = new Vector2(0.5f, 1.0f);
                    rectTransform.anchoredPosition = new Vector2(0.0f, -GoalYOffset);
                    // Make sure the rectangle is wide enough for the goal text.
                    rectTransform.sizeDelta = new Vector2(Mathf.Max(rectTransform.sizeDelta.x, 600.0f), rectTransform.sizeDelta.y);
                }
            }
            else
            {
                _logger.LogError("Could not find Progress object to create the goal text");
            }
        }

        return _goalGameObject;
    }

    private TextMeshProUGUI GetGoalText()
    {
        if (_goalText == null)
        {
            GameObject goalGameObject = GetGoalGameObject();
            if (goalGameObject != null)
            {
                _goalText = goalGameObject.GetComponent<TextMeshProUGUI>();
            }
        }

        if (_goalText == null)
        {
            _logger.LogError("Could not find Goal text component");
        }

        return _goalText;
    }

    private void HandleMapTitleLayout()
    {
        GameObject mapTitleLayoutObject = GetMapTitleLayoutObject();

        if (mapTitleLayoutObject == null)
        {
            return;
        }
        InputPrompt[] prompts = mapTitleLayoutObject.GetComponentsInChildren<InputPrompt>(true);
        foreach (InputPrompt prompt in prompts)
        {
            // Only show the prompt if the randomizer is active.
            if (_randomizerEngine.IsRandomized())
            {
                prompt.gameObject.SetActive(true);
                prompt.UpdateView();
            }
            else
            {
                prompt.gameObject.SetActive(false);
            }
        }
    }

    private void HandleProgress()
    {
        TextMeshProUGUI progressShadowText = GetProgressShadowText();
        TextMeshProUGUI progressText = GetProgressText();

        if (progressShadowText == null || progressText == null)
        {
            return;
        }
        int randomizedLocationsCount = _randomizerEngine.GetRandomizedLocations().Count;
        int foundLocations = _progressionStorage.GetLocationCheckedCount();
        string progressString = $"{foundLocations}/{randomizedLocationsCount}";
        progressShadowText.text = progressString;
        progressText.text = progressString;
    }

    private void HandleGoal()
    {
        if (!_randomizerEngine.IsRandomized())
        {
            if (_goalGameObject != null)
            {
                _goalGameObject.SetActive(false);
            }
            return;
        }

        TextMeshProUGUI goalText = GetGoalText();

        if (goalText == null)
        {
            return;
        }
        Goals goal = _randomizerEngine.GetSetting<CompletionGoals>().Goal;
        goalText.text = "Objective : " + GetGoalDisplayName(goal);
    }

    private static string GetGoalDisplayName(Goals goal)
    {
        return goal switch
        {
            Goals.Dungeon5 => "Dungeon 5",
            Goals.Snow => "Snow",
            Goals.Dungeon5AndSnow => "Dungeon 5 & Snow",
            Goals.SpiritTower => "Spirit Tower",
            _ => goal.ToString()
        };
    }
}
