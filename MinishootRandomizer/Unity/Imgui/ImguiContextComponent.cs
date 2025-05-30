using UnityEngine;

namespace MinishootRandomizer;

public class ImguiContextComponent : MonoBehaviour
{
    private const float _xOrigin = 10;
    private const float _yOrigin = 40;
    private const float _lineHeight = 20;
    private const float _inputWidth = 200;
    private const float _labelWidth = 120;

    private IArchipelagoClient _archipelagoClient;

    private string _serverUri = "archipelago.gg:12345";
    private string _slotName = "Minishoot";
    private string _password = "";
    private bool _activateDeathlink = false;
    private int _lineCount = 0;
    private bool _haveError = false;
    private string _errorMessage = "";

    void Awake()
    {
        _archipelagoClient = Plugin.ServiceContainer.Get<IArchipelagoClient>();
        // Load saved values from PlayerPrefs
        _serverUri = PlayerPrefs.GetString("ArchipelagoServerUri", _serverUri);
        _slotName = PlayerPrefs.GetString("ArchipelagoSlotName", _slotName);
    }

    void OnGUI()
    {
        if (GameManager.State != GameState.Title)
        {
            return;
        }

        _lineCount = 0;
        string versionLabel = $"v{Plugin.RandomizerVersion}";
        GUI.Box(new Rect(_xOrigin, _yOrigin, 350, 180), "Randomizer Menu " + versionLabel + (Plugin.IsDebug ? " (DEBUG MODE ACTIVE)" : ""));
        _serverUri = AddTextLine("Server URI: ", _serverUri);
        _slotName = AddTextLine("Slot name: ", _slotName);
        _password = AddPasswordLine("Password: ", _password);
        // @TODO: Implement deathlink
        // _activateDeathlink = AddToggle("Activate Deathlink", _activateDeathlink);

        if (AddButton("Connect"))
        {
            try
            {
                string newServerUri = _serverUri.Trim();
                string newSlotName = _slotName.Trim();

                // Save the server URI and slot name to PlayerPrefs
                PlayerPrefs.SetString("ArchipelagoServerUri", newServerUri);
                PlayerPrefs.SetString("ArchipelagoSlotName", newSlotName);
                PlayerPrefs.Save();

                // Connect to the Archipelago server
                ArchipelagoOptions options = new ArchipelagoOptions(
                    newServerUri,
                    newSlotName,
                    _password.Trim(),
                    false
                );
                _archipelagoClient.Connect(options);
                if (_archipelagoClient.IsConnected())
                {
                    _haveError = false;
                    _errorMessage = "";
                }
                else
                {
                    _haveError = true;
                    _errorMessage = "Failed to connect to Archipelago server.";
                }
            }
            catch (ArchipelagoLoginException e)
            {
                _haveError = true;
                _errorMessage = e.Message;
            }
        }

        if (AddButton("Disconnect", true))
        {
            _archipelagoClient.Disconnect();
        }

        AddStatusLine();
        AddErrorMessageLine();
    }

    public RandomizerContext GetContext()
    {
        if (_archipelagoClient.IsConnected())
        {
            return new ArchipelagoContext(
                _serverUri,
                _slotName,
                _password,
                _activateDeathlink
            );
        }
        else
        {
            return new VanillaContext();
        }
    }

    private void AddStatusLine()
    {
        _lineCount++;
        float x = _xOrigin + 10;
        float y = _yOrigin + _lineHeight + _lineCount * _lineHeight;
        string status = "Status: ";
        GUIStyle style = new GUIStyle();
        if (_archipelagoClient.IsConnected())
        {
            status += "Connected";
            style.normal.textColor = Color.green;
        }
        else if (_haveError)
        {
            status += "Error";
            style.normal.textColor = Color.red;
        }
        else
        {
            status += "Not connected";
            style.normal.textColor = Color.white;
        }
        GUI.Label(new Rect(x, y, 330, _lineHeight), status, style);
    }

    private void AddErrorMessageLine()
    {
        if (_haveError)
        {
            _lineCount++;
            float x = _xOrigin + 10;
            float y = _yOrigin + _lineHeight + _lineCount * _lineHeight;
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.red;
            style.wordWrap = true;
            GUI.Label(new Rect(x, y, 330, _lineHeight), _errorMessage, style);
        }
    }

    private string AddTextLine(string label, string value)
    {
        _lineCount++;
        float x = _xOrigin + 10;
        float y = _yOrigin + _lineHeight + _lineCount * _lineHeight;
        GUI.Label(new Rect(x, y, _labelWidth, _lineHeight), label);
        return GUI.TextField(new Rect(x + _labelWidth + 10, y, _inputWidth, _lineHeight), value);
    }

    private string AddPasswordLine(string label, string value)
    {
        _lineCount++;
        float x = _xOrigin + 10;
        float y = _yOrigin + _lineHeight + _lineCount * _lineHeight;
        GUI.Label(new Rect(x, y, _labelWidth, _lineHeight), label);
        return GUI.PasswordField(new Rect(x + _labelWidth + 10, y, _inputWidth, _lineHeight), value, '*');
    }

    private bool AddToggle(string label, bool value)
    {
        _lineCount++;
        float x = _xOrigin + 10;
        float y = _yOrigin + _lineHeight + _lineCount * _lineHeight;
        return GUI.Toggle(new Rect(x, y, _labelWidth, _lineHeight), value, label);
    }

    private bool AddButton(string label, bool onTheRight = false)
    {
        if (!onTheRight)
        {
            _lineCount++;
        }

        float x = _xOrigin + 10 + (onTheRight ? _labelWidth + 20 : 0);
        float y = _yOrigin + _lineHeight + _lineCount * _lineHeight;
        return GUI.Button(new Rect(x, y, _labelWidth, _lineHeight), label);
    }
}
