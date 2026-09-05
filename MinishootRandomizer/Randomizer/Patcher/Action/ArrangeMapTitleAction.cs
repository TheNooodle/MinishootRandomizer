namespace MinishootRandomizer;

public class ArrangeMapTitleAction : IPatchAction
{
    private readonly ITitleArranger _mapTitleArranger;

    public ArrangeMapTitleAction(ITitleArranger mapTitleArranger)
    {
        _mapTitleArranger = mapTitleArranger;
    }

    public void Dispose()
    {
        // no-op, as the end result should be "transparent", even for non-randomized games.
    }

    public void Patch()
    {
        if (!_mapTitleArranger.IsTitleArranged())
        {
            _mapTitleArranger.ArrangeMapTitle();
        }
    }

    public void Unpatch()
    {
        // no-op
    }
}
