namespace MinishootRandomizer;

abstract public class AbstractMarker
{
    public const float IN_LOGIC_ANIMATION_AMPLITUDE = 5f;
    public const float OUT_OF_LOGIC_ANIMATION_AMPLITUDE = 2.5f;

    abstract public void ComputeVisibility(IRandomizerEngine engine, ILogicChecker logicChecker);
    abstract public bool MustShow();
    abstract public MarkerSpriteInfo GetSpriteInfo();
    abstract public int GetSortIndex();
    abstract public float GetAnimationAmplitude();
}
