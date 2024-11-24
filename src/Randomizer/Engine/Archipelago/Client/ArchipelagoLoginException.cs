using System;

namespace MinishootRandomizer;

public class ArchipelagoLoginException : System.Exception
{
    public ArchipelagoLoginException(string message, Exception exception) : base(message, exception) { }
}
