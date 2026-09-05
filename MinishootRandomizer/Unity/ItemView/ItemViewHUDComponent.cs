using UnityEngine;

namespace MinishootRandomizer;

public class ItemViewHUDComponent : MonoBehaviour
{
    private Item _item;

    private CanvasRenderer _canvasRenderer;

    void Start()
    {
        _canvasRenderer = GetComponent<CanvasRenderer>();
    }

    void Update()
    {
        if (_item == null)
        {
            return;
        }

        int itemCount = _item.GetOwnedQuantity();
        if (itemCount > 0)
        {
            _canvasRenderer.SetColor(Color.white);
        }
        else
        {
            _canvasRenderer.SetColor(Color.black);
        }
    }

    public void SetItem(Item item)
    {
        _item = item;
    }
}
