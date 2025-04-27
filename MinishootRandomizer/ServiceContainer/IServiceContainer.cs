using System;

namespace MinishootRandomizer;

public interface IServiceContainer
{
    T Get<T>();
    bool Has<T>();
}

public interface IBuildable
{
    void Build();
}

public class ServiceNotFoundException : Exception
{
    public ServiceNotFoundException(string message) : base(message)
    {
    }
}
