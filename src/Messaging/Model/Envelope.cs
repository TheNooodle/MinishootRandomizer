using System.Collections.Generic;

namespace MinishootRandomizer;

public class Envelope
{
    private IMessage _message;
    private List<IStamp> _stamps = new();

    public IMessage Message => _message;
    public List<IStamp> Stamps => _stamps;

    public Envelope(IMessage message)
    {
        _message = message;
    }

    public void AddStamp(IStamp stamp)
    {
        _stamps.Add(stamp);
    }

    public void RemoveStamp(IStamp stamp)
    {
        _stamps.Remove(stamp);
    }

    public bool HasStamp<T>() where T : IStamp
    {
        return _stamps.Exists(s => s.GetType() == typeof(T));
    }

    public T GetStamp<T>() where T : IStamp
    {
        return (T)_stamps.Find(s => s.GetType() == typeof(T));
    }
}
