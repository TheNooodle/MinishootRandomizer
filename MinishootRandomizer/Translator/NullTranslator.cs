namespace MinishootRandomizer;

// Translator that does nothing.
public class NullTranslator : ITranslator
{
    public string Translate(string key, Language language = Language.Default)
    {
        return key;
    }

    public string Translate(TranslatedMessage message, Language language = Language.Default)
    {
        return message.Key;
    }

    public void SetLanguage(Language language)
    {
        // no-op
    }
}
