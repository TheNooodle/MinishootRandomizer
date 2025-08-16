namespace MinishootRandomizer;

abstract public class NumericSettings : ISetting
{
    private int _value;

    public int Value => _value;
    
    public NumericSettings(int value)
    {
        _value = value;
    }
}
