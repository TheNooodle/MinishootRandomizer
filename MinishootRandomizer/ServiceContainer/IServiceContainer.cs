using System;

namespace MinishootRandomizer;

public interface IServiceContainer
{
    T Get<T>() where T : class;
    bool Has<T>() where T : class;
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
