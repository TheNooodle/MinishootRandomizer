namespace MinishootRandomizer;

public class TriggerTrapDialogHandler : IMessageHandler
{
    private readonly ITrapDialogProvider _trapDialogProvider;

    public TriggerTrapDialogHandler(ITrapDialogProvider trapDialogProvider)
    {
        _trapDialogProvider = trapDialogProvider;
    }

    public void Handle(IMessage message)
    {
        if (message is not TriggerTrapDialogMessage)
        {
            return;
        }

        string dialog = _trapDialogProvider.GetDialog();
        TextMessage textMessage = UIManager.TextMessage;
        ReflectionHelper.InvokePublicMethod(textMessage, "Pop", new object[] { dialog, null, true, 1, true, true });
    }
}
