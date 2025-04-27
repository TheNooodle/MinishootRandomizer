namespace MinishootRandomizer;

public class RandomizerEngineManager
{
    private readonly IRandomizerEngine _engine;
    
    public RandomizerEngineManager(IRandomizerEngine engine)
    {
        _engine = engine;
    }

    public void OnLoadingSaveFile(bool isNewGame)
    {
        _engine.Initialize();
    }

    public void OnExitingGame()
    {
        _engine.Dispose();
    }
}
