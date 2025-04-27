using System;

namespace MinishootRandomizer;

public class TransitionNotFoundException : Exception
{
    public TransitionNotFoundException(string identifier) : base($"Transition not found: {identifier}")
    {
    }
}
