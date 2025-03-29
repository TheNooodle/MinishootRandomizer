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
    private ITranslator _translator;
    private ILogger _logger = new NullLogger();

    private string _serverUri = "archipelago.gg:12345";
    private string _slotName = "Minishoot";
    private string _password = "";
    private bool _activateDeathlink = false;
    private int _lineCount = 0;
    private bool _haveError = false;

    void Awake()
    {
        _archipelagoClient = Plugin.ServiceContainer.Get<IArchipelagoClient>();
        _translator = Plugin.ServiceContainer.Get<ITranslator>();
        _logger = Plugin.ServiceContainer.Get<ILogger>() ?? new NullLogger();
    }

    void OnGUI()
    {
        if (GameManager.State != GameState.Title)
        {
            return;
        }

        _lineCount = 0;
        string versionLabel = $"v{Plugin.RandomizerVersion}";
        GUI.Box(new Rect(_xOrigin, _yOrigin, 350, 150), "Randomizer Menu " + versionLabel + (Plugin.IsDebug ? " (DEBUG MODE ACTIVE)" : ""));
        _serverUri = AddTextLine("Server URI: ", _serverUri);
        _slotName = AddTextLine("Slot name: ", _slotName);
        _password = AddPasswordLine("Password: ", _password);
        // @TODO: Implement deathlink
        // _activateDeathlink = AddToggle("Activate Deathlink", _activateDeathlink);

        if (AddButton("Connect"))
        {
            try
            {
                ArchipelagoOptions options = new ArchipelagoOptions(
                    _serverUri.Trim(),
                    _slotName.Trim(),
                    _password.Trim(),
                    false
                );
                _archipelagoClient.Connect(options);
                if (_archipelagoClient.IsConnected())
                {
                    _haveError = false;
                }
                else
                {
                    _haveError = true;
                }
            }
            catch (ArchipelagoLoginException)
            {
                _haveError = true;
            }
        }

        if (AddButton("Disconnect", true))
        {
            _archipelagoClient.Disconnect();
        }

        AddStatusLine();
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
        GUI.Label(new Rect(x, y, 200, _lineHeight), status, style);
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
