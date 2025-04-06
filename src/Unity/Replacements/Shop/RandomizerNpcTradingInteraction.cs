using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MinishootRandomizer;

public class RandomizerNpcTradingInteraction : MiniBehaviour
{
    private IRandomizerEngine _randomizerEngine;
    private IItemPresentationProvider _itemPresentationProvider;
    private GameEventDispatcher _gameEventDispatcher;
    private ILogger _logger = new NullLogger();

    private Vector2 _dialogOffset = Vector2.zero;
    private float _interactionDelay = 0.35f;
    private bool _active = false;
    private float _interactionDelayCnt;

    private DialogBubble _dialog;
    private Npc _npc;
    private List<RandomizedShopSlot> _shopSlots = new List<RandomizedShopSlot>();
    private RandomizedShopSlot _currentSlot = null;
    private RandomizerPickup _currentPickup = null;

    public ItemType ItemType { get; set; } = ItemType.NextPickup;

    private bool HasInteractionLeft
    {
        get
        {
            if (ItemType == ItemType.SelectedPickup)
            {
                return true;
            }
            else if (ItemType == ItemType.NextPickup)
            {
                foreach (RandomizedShopSlot shopSlot in _shopSlots)
                {
                    if (!_randomizerEngine.IsLocationChecked(shopSlot.Location))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        _npc = GetComponent<Npc>();
        _dialog = Instantiate(Resources.Load<DialogBubble>("DialogBubbleWide"));
        _dialog.gameObject.SetActive(value: false);
        PlayerInputs.Interact += TryInteract;

        _randomizerEngine = Plugin.ServiceContainer.Get<IRandomizerEngine>();
        _itemPresentationProvider = Plugin.ServiceContainer.Get<IItemPresentationProvider>();
        _gameEventDispatcher = Plugin.ServiceContainer.Get<GameEventDispatcher>();
        _logger = Plugin.ServiceContainer.Get<ILogger>();
    }

    protected override void OnGameStateLoaded()
    {
        Hide();
    }

    protected override void OnPlayerRestored()
    {
        Hide();
    }

    protected override void MiniUpdate()
    {
        if (!Player.Instance.Active || ItemType == ItemType.SelectedPickup)
        {
            return;
        }

        float num = Vector2.Distance(Player.Position, base.transform.position);
        if (!_active && num < _npc.DistanceGreet)
        {
            _interactionDelayCnt += SGTime.DeltaTime;
            if (_interactionDelayCnt >= _interactionDelay)
            {
                _interactionDelayCnt = 0f;
                Show();
            }
        }

        if (_active && HasInteractionLeft)
        {
            UpdatePromptPos();
            if (num > _npc.DistanceGreet * 1.05f)
            {
                Hide();
            }
        }
    }

    public void AddShopSlot(RandomizedShopSlot shopSlot, int slotIndex = 0)
    {
        if (slotIndex < 0 || slotIndex > _shopSlots.Count)
        {
            slotIndex = _shopSlots.Count;
        }

        if (slotIndex == _shopSlots.Count)
        {
            _shopSlots.Add(shopSlot);
        }
        else
        {
            _shopSlots.Insert(slotIndex, shopSlot);
        }
    }

    public void Show(RandomizerPickup pickup = null)
    {
        _active = true;

        RandomizedShopSlot shopSlot = null;
        ItemPresentation itemPresentation = null;
        if (pickup == null)
        {
            shopSlot = _shopSlots.FirstOrDefault(slot => !_randomizerEngine.IsLocationChecked(slot.Location));
            if (shopSlot == null)
            {
                _logger.LogError("No shop slot found for " + name + " trading interaction.");

                return;
            }
            else
            {
                itemPresentation = _itemPresentationProvider.GetItemPresentation(shopSlot.Item);
            }
        }
        else
        {
            shopSlot = pickup.ShopSlot;
            itemPresentation = pickup.ItemPresentation;
        }

        _currentSlot = shopSlot;
        _currentPickup = pickup;

        if (HasInteractionLeft)
        {
            Player.Emote.Play(Emotes.Question);
            PopDialog(itemPresentation);

            // @TODO: import DOTween
            // Player.Instance.InputPrompt.PopOut();
            UpdatePromptPos(forcePop: true);
        }
    }

    private void UpdatePromptPos(bool forcePop = false)
    {
        // @TODO: import DOTween
        // bool flag = Player.Position.x < npc.transform.position.x - 0.4f;
        // bool flag2 = Player.Position.x > npc.transform.position.x + 0.4f;
        // if (forcePop || (flag && !Player.Instance.InputPromptLiteLeft.isActiveAndEnabled) || (flag2 && !Player.Instance.InputPromptLiteRight.isActiveAndEnabled))
        // {
        //     Player.Instance.InputPromptLiteLeft.PopOut();
        //     Player.Instance.InputPromptLiteRight.PopOut();
        //     (flag ? Player.Instance.InputPromptLiteLeft : Player.Instance.InputPromptLiteRight).PopIn("Interact", overridePromptText);
        // }
    }

    private void PopDialog(ItemPresentation itemPresentation)
    {
        ShowCurrency();
        try
        {
            string title = itemPresentation.Name;
            string desc = itemPresentation.Description;
            Sprite icon = itemPresentation.SpriteData.Sprite;
            _dialog.Pop(transform.position, ItemType, Interaction.Trade, title, desc, icon, _currentSlot.Price, _currentSlot.Currency, false, _dialogOffset);
        }
        catch (SpriteNotFound)
        {
            _logger.LogWarning($"Sprite not found for {_currentSlot.Item.Identifier}");
        }
    }

    public void Hide()
    {
        _active = false;
        HideCurrency();
        _dialog.PopOut();
        _currentSlot = null;
        _currentPickup = null;
    }

    private void TryInteract()
    {
        if (_active && Player.Instance.Active && !PauseManager.IsPaused && _npc.Freed && HasInteractionLeft)
        {
            TryTrade();
        }
    }

    private void TryTrade()
    {
        bool isItemSelected = _currentSlot != null;
        if (PlayerState.Currency[_currentSlot.Currency] >= _currentSlot.Price && isItemSelected)
        {
            PlayerState.SetCurrency(_currentSlot.Currency, -_currentSlot.Price);

            if (_currentPickup != null)
            {
                // The randomizer engine and the event dispatcher are called within the pickup's Collect method.
                _currentPickup.Collect();
            }
            else
            {
                _currentSlot.Item.Collect();
                _randomizerEngine.CheckLocation(_currentSlot.Location);
                _gameEventDispatcher.DispatchItemCollected(_currentSlot.Item);
            }
            Fx.PickupBuy();

            Player.Emote.Play(Emotes.Ok);
            Hide();
        }
        else
        {
            // @TODO: import DOTween
            // NpcFx.Sad(npc.Tweenable, npc.Id);
            Player.Emote.Play(Emotes.Shameful, 0.8f);
            if (isItemSelected)
            {
                _dialog.TradeDenied();
                UIManager.HUD.ItemViewHud.CurrenciesView[_currentSlot.Currency].CantBuy();
            }
        }
    }

    private void ShowCurrency()
    {
        if (_currentSlot != null)
        {
            UIManager.HUD.ItemViewHud.CurrenciesView[_currentSlot.Currency].Appear(popOutAuto: false);
        }
    }

    private void HideCurrency()
    {
        if (_currentSlot != null)
        {
            UIManager.HUD.ItemViewHud.CurrenciesView[_currentSlot.Currency].Disappear(withDelay: false);
        }
    }
}
