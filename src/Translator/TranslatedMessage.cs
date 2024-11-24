using System.Collections.Generic;

namespace MinishootRandomizer;

public class TranslatedMessage
{
    private string _key;
    private Dictionary<string, string> _parameters;

    public string Key => _key;
    public Dictionary<string, string> Parameters => _parameters;

    public TranslatedMessage(string key, Dictionary<string, string> parameters)
    {
        _key = key;
        _parameters = parameters;
    }
}
