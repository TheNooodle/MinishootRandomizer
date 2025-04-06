using System.Collections.Generic;

namespace MinishootRandomizer;

public class TrapManager
{
    private readonly IMessageDispatcher _messageDispatcher;

    public TrapManager(IMessageDispatcher messageDispatcher)
    {
        _messageDispatcher = messageDispatcher;
    }

    public void OnItemCollected(Item item)
    {
        if (item is TrapItem || (item is ReceivedItem && ((ReceivedItem)item).InnerItem is TrapItem))
        {
            _messageDispatcher.Dispatch(new TriggerTrapDialogMessage(), new List<IStamp>{
                new MustBeInGameStamp()
            });
        }
    }
}
