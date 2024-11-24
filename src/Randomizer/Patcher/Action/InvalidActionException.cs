using System;

namespace MinishootRandomizer;

public class InvalidActionException: Exception
{
    public InvalidActionException(string message): base(message)
    {
    }
}
