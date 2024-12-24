using System;

namespace MinishootRandomizer;

public class UnknownRuleException : Exception
{
    public UnknownRuleException(string message) : base(message)
    {
    }
}
