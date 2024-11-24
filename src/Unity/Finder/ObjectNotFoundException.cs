using System;

namespace MinishootRandomizer;

public class ObjectNotFoundException : Exception
{
    public ObjectNotFoundException(string message) : base(message)
    {
    }
}
