using UnityEngine;

namespace MinishootRandomizer;

public interface IItemViewFactory
{
    GameObject CreateItemViewObject(ItemView itemView);
}
