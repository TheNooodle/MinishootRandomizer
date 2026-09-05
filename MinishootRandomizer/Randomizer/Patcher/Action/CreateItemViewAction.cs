using UnityEngine;

namespace MinishootRandomizer;

public class CreateItemViewAction : IPatchAction
{
    private ItemView _itemView = null;
    private IItemViewFactory _itemViewFactory;

    private GameObject _itemViewObject = null;

    public CreateItemViewAction(ItemView itemView, IItemViewFactory itemViewFactory)
    {
        _itemView = itemView;
        _itemViewFactory = itemViewFactory;
    }

    public void Dispose()
    {
        if (_itemViewObject != null)
        {
            GameObject.Destroy(_itemViewObject);
            _itemViewObject = null;
        }
    }

    public void Patch()
    {
        _itemViewObject = _itemViewFactory.CreateItemViewObject(_itemView);
    }

    public void Unpatch()
    {
        // no-op
    }
}
