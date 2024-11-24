using System;

namespace MinishootRandomizer;

public interface IItemFactory
{
    Item CreateItem(string identifier, ItemCategory category);
}

public class InvalidItemException : Exception
{
}
