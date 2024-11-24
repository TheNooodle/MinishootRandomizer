using System;

namespace MinishootRandomizer;

public class SpriteNotFound : Exception
{
    public SpriteNotFound(string message) : base(message)
    {
    }
}
