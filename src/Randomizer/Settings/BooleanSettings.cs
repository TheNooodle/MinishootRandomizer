namespace MinishootRandomizer;

abstract public class BooleanSetting : ISetting
{
    private bool _enabled;

    public bool Enabled => _enabled;

    public BooleanSetting(bool enabled)
    {
        _enabled = enabled;
    }
}
