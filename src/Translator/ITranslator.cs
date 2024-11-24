namespace MinishootRandomizer;

public interface ITranslator
{
    public string Translate(string key, Language language = Language.Default);
    public string Translate(TranslatedMessage message, Language language = Language.Default);
    public void SetLanguage(Language language);
}
