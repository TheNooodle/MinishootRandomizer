using UnityEngine;

namespace MinishootRandomizer;

public interface IObjectFinder
{
    GameObject FindObject(ISelector selector);
    GameObject[] FindObjects(ISelector selector);
}
