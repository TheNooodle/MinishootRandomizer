namespace MinishootRandomizer;

abstract public class AbstractMarker
{
    abstract public void ComputeVisibility(IRandomizerEngine engine, ILogicChecker logicChecker);
    abstract public bool MustShow();
    abstract public MarkerSpriteInfo GetSpriteInfo();
    abstract public int GetSortIndex();
}
