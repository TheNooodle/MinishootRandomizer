using System;
using System.Collections.Generic;

namespace MinishootRandomizer;

public class CoreMessageConsumer : IMessageConsumer
{
    private readonly IEnvelopeStorage _storage;
    private readonly IMessageProcessor _messageProcessor;
    private readonly ILogger _logger = new NullLogger();

    private readonly Dictionary<Type, IMessageHandler> _handlers = new Dictionary<Type, IMessageHandler>();

    public CoreMessageConsumer(IEnvelopeStorage storage, IMessageProcessor processor, ILogger logger = null)
    {
        _storage = storage;
        _messageProcessor = processor;
        _logger = logger ?? new NullLogger();
    }

    public void AddHandler<T>(IMessageHandler handler)
    {
        _handlers.Add(typeof(T), handler);
    }

    public void Consume()
    {
        List<Envelope> envelopes = _storage.GetAll();
        List<Envelope> envelopesToRemove = new();
        foreach (Envelope envelope in envelopes)
        {
            Type messageType = envelope.Message.GetType();
            if (!_handlers.ContainsKey(messageType))
            {
                _logger.LogError($"No handler found for message type: {messageType}");
                envelopesToRemove.Add(envelope);
                continue;
            }
            if (CanHandleEnvelope(envelope))
            {
                IMessageHandler handler = _handlers[messageType];
                MessageProcessingResult result = _messageProcessor.ProcessMessage(envelope, handler);
                if (result.Success)
                {
                    envelopesToRemove.Add(envelope);
                }
                else
                {
                    _logger.LogError($"Failed to process message: {result.ErrorMessage}");
                }
            }
        }

        foreach (Envelope envelope in envelopesToRemove)
        {
            _storage.Remove(envelope);
        }
    }

    private bool CanHandleEnvelope(Envelope envelope)
    {
        foreach (IStamp stamp in envelope.Stamps)
        {
            if (!stamp.CanHandle(envelope.Message))
            {
                return false;
            }
        }
        return true;
    }
}
