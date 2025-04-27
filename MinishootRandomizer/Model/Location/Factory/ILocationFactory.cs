using System;

namespace MinishootRandomizer;

public interface ILocationFactory
{
    Location CreateLocation(string name, string logicRule, LocationPool pool);
}

public class InvalidLocationException : Exception
{
    public InvalidLocationException(string message) : base(message)
    {
    }
}
