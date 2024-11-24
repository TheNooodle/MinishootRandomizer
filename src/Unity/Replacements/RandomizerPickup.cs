using System;
using UnityEngine;

namespace MinishootRandomizer;

public class RandomizerPickup : MonoBehaviour, IActivationChecker
{
    private IRandomizerEngine _randomizerEngine = Plugin.ServiceContainer.Get<IRandomizerEngine>();
    private ILogger _logger = Plugin.ServiceContainer.Get<ILogger>();

    private Collider2D _collectCollider;
    private EventDestroyableComponent _destroyable = null;

    private bool _checkForFamilyReunion = false;

    public PickupType PickupType;

    public Rarity Rarity;

    public bool Animated = true;

    public bool ShineEffectAllowed = true;

    public SpriteRenderer[] AdditionalSprites;

    public SoundLoopPlayer Sound;

    public ParticleSystem ParticlesBeam { get; private set; }

    public ParticleSystem ParticlesPop { get; protected set; }

    public ParticleSystem ParticlesEpic { get; private set; }

    public SpriteRenderer Sprite { get; private set; }

    public SpriteMask ShineEffect { get; set; }

    public Vector2 Origin { get; private set; }

    public bool Owned => !IsPrefab && Location != null && _randomizerEngine.IsLocationChecked(Location);

    public bool IsForSale
    {
        get
        {
            return ShopSlot != null;
        }
    }

    public event Action EndedCollect;

    public Item Item { get; set; }
    public Location Location { get; private set; } = null;
    public bool IsPrefab { get; set; } = true;
    public Npc Owner { get; set; } = null;
    public RandomizedShopSlot ShopSlot { get; set; } = null;
    public EventDestroyableComponent Destroyable => _destroyable;

    private bool IsFamilyReunited
    {
        get
        {
            return WorldState.Get("Familly1") && WorldState.Get("Familly2") && WorldState.Get("Familly3");
        }
    }

    public virtual void InitializeFromPickup(Pickup pickup)
    {
        Rarity = pickup.Rarity;
        PickupType = pickup.Type;
        ParticlesBeam = pickup.ParticlesBeam;
        ParticlesPop = pickup.ParticlesPop;
        ParticlesEpic = pickup.ParticlesEpic;
        Sprite = pickup.Sprite;
        ShineEffect = pickup.ShineEffect;
        Origin = pickup.Origin;
    }

    void Start()
    {
        Origin = base.transform.position;
        Sound = GetComponentInChildren<SoundLoopPlayer>();
        ParticlesBeam = base.gameObject.GetComponentNamed<ParticleSystem>("PickupBeam");
        ParticlesPop = base.gameObject.GetComponentNameStartWith<ParticleSystem>("PickupPop");
        ParticlesEpic = base.gameObject.GetComponentNameStartWith<ParticleSystem>("EpicRays");
        Sprite = GetComponentInChildren<SpriteRenderer>();

        // if (animated)
        // {
        //     Fx.PickupLoopAnim(this);
        //     ShineEffect.gameObject.SetActive(shineEffectAllowed);
        // }

        Collider2D collectCollider = GetCollectCollider();
        if (collectCollider)
        {
            collectCollider.enabled = true;
            if (IsForSale && Owner && collectCollider is BoxCollider2D boxCollider)
            {
                CircleCollider2D circleCollider = gameObject.AddComponent<CircleCollider2D>();
                circleCollider.radius = 0.4f;
                circleCollider.offset = new Vector2(0, Owner.name == "MercantHub" ? 0.8f : -1.2f);
                circleCollider.isTrigger = true;
                circleCollider.enabled = true;
                circleCollider.tag = boxCollider.tag;
                Destroy(boxCollider);
            }
        }
    }

    public void OnLoadingSaveFile(bool isNewGame)
    {
        CheckActivation();
        Restore();

        if (IsPrefab)
        {
            transform.position = new Vector3(-3000, -3000, -3000);
        }
    }

    public void SetLocation(Location location)
    {
        Location = location;

        // Edge case for the family reunion.
        if (location.Identifier == "Family House Cave - Reunited Family")
        {
            transform.position = new Vector3(-668f, 7f, 0f);
            _checkForFamilyReunion = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (IsForSale && Owner != null)
            {
                RandomizerNpcTradingInteraction interaction = Owner.GetComponent<RandomizerNpcTradingInteraction>();
                if (interaction != null && interaction.ItemType == ItemType.SelectedPickup)
                {
                    interaction.Show(this);
                }
                else
                {
                    _logger.LogWarning($"RandomizerPickup: Pickup {name} is for sale but owner {Owner.name} does not have RandomizerNpcTradingInteraction component.");
                }
            }
            else
            {
                Collect();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && IsForSale && Owner != null)
        {
            RandomizerNpcTradingInteraction interaction = Owner.GetComponent<RandomizerNpcTradingInteraction>();
            if (interaction != null && interaction.ItemType == ItemType.SelectedPickup)
            {
                interaction.Hide();
            }
            else
            {
                _logger.LogWarning($"RandomizerPickup: Pickup {name} is for sale but owner {Owner.name} does not have RandomizerNpcTradingInteraction component.");
            }
        }
    }

    private void OnDestroy()
    {
        if (_destroyable != null)
        {
            _destroyable.Destroyed -= delegate
            {
                CheckActivation();
            };
            _destroyable.Restored -= delegate
            {
                CheckActivation();
            };
        }
    }

    public void AssignDestroyable(EventDestroyableComponent destroyable)
    {
        if (IsPrefab)
        {
            return;
        }
        if (_destroyable != null)
        {
            _logger.LogWarning($"RandomizerPickup: Pickup {name} already has Destroyable assigned.");
            return;
        }
        _destroyable = destroyable;
        _destroyable.Destroyed += delegate
        {
            CheckActivation();
        };
        _destroyable.Restored += delegate
        {
            CheckActivation();
        };
    }

    public virtual void Collect()
    {
        //IL_008f: Unknown result type (might be due to invalid IL or missing references)
        //IL_0099: Expected O, but got Unknown
        // Sprite.sortingLayerName = SortingLayer.op_Implicit(SRSortingLayers.UI);
        // SpriteRenderer[] array = additionalSprites;
        // for (int i = 0; i < array.Length; i++)
        // {
        //     array[i].sortingLayerName = SortingLayer.op_Implicit(SRSortingLayers.UI);
        // }

        // ParticleSystem[] componentsInChildren = GetComponentsInChildren<ParticleSystem>(includeInactive: true);
        // for (int i = 0; i < componentsInChildren.Length; i++)
        // {
        //     componentsInChildren[i].GetComponent<Renderer>().sortingLayerName = SortingLayer.op_Implicit(SRSortingLayers.UI);
        // }

        Player.Instance.SetActive(active: false, setFreezeUnfreezeGame: true);
        GetCollectCollider().enabled = false;
        if (Item != null)
        {
            Item.Collect();
            _randomizerEngine.CheckLocation(Location);
        }
        EndCollect();
        // TweenSettingsExtensions.OnComplete<Sequence>(Fx.Pickup(this), new TweenCallback(EndCollect));
    }

    protected virtual void EndCollect()
    {
        CameraManager.ForwardTiltToggled = true;
        FireEndedCollect();
        Player.Instance.SetActive(active: true, setFreezeUnfreezeGame: true);
        CheckActivation();

        // Make minishoot a happy (or sad) little lad.
        switch (Item.Category)
        {
            case ItemCategory.Progression:
                Player.Emote.Play(Emotes.Happy, 0.8f);
                break;
            case ItemCategory.Trap:
                Player.Emote.Play(Emotes.Shameful, 0.8f);
                break;
            default:
                Player.Emote.Play(Emotes.Ok, 0.8f);
                break;
        }
    }

    protected void FireEndedCollect()
    {
        this.EndedCollect?.Invoke();
    }

    public bool CheckActivation()
    {
        bool active = !IsPrefab && !Owned && (!IsForSale || Owner.Freed) && (!_destroyable || _destroyable.IsDestroyed) && (!_checkForFamilyReunion || IsFamilyReunited);
        base.gameObject.SetActive(active);
        if (Sound != null)
        {
            Sound.SetActive(active);
        }

        return active;
    }

    protected virtual void Restore()
    {
        if (!IsForSale)
        {
            GetCollectCollider().enabled = true;
        }

        // Sprite.sortingLayerName = SortingLayer.op_Implicit(SRSortingLayers.Block);
        // SpriteRenderer[] array = additionalSprites;
        // for (int i = 0; i < array.Length; i++)
        // {
        //     array[i].sortingLayerName = SortingLayer.op_Implicit(SRSortingLayers.Block);
        // }

        // ParticleSystem[] componentsInChildren = GetComponentsInChildren<ParticleSystem>(includeInactive: true);
        // for (int i = 0; i < componentsInChildren.Length; i++)
        // {
        //     componentsInChildren[i].GetComponent<Renderer>().sortingLayerName = SortingLayer.op_Implicit(SRSortingLayers.Block);
        // }

        if ((bool)ParticlesBeam)
        {
            ParticlesBeam.Play();
        }

        // ShortcutExtensions.DOKill((Component)base.transform, false);
    }

    public void SetLockedDuringEncounter(bool locked)
    {
        //IL_00b6: Unknown result type (might be due to invalid IL or missing references)
        //IL_00c1: Expected O, but got Unknown
        if (Rarity == Rarity.Scarab || Owned)
        {
            return;
        }

        // DOTweenModuleSprite.DOFade(Sprite, (!locked) ? 1f : 0f, 0.3f);
        // SpriteRenderer[] array = additionalSprites;
        // for (int i = 0; i < array.Length; i++)
        // {
        //     DOTweenModuleSprite.DOFade(array[i], (!locked) ? 1f : 0.15f, 0.3f);
        // }

        if (ShineEffect != null)
        {
            ShineEffect.gameObject.SetActive(!locked && ShineEffectAllowed);
        }

        if (!IsForSale)
        {
            if (locked)
            {
                GetCollectCollider().enabled = false;
            }
            else
            {
                // TweenSettingsExtensions.SetUpdate<Tween>(DOVirtual.DelayedCall(1.5f, (TweenCallback)delegate
                // {
                    GetCollectCollider().enabled = true;
                // }, true), true);
            }
        }

        if (ParticlesBeam != null)
        {
            if (locked)
            {
                ParticlesBeam.Stop();
            }
            else
            {
                ParticlesBeam.Play();
            }
        }

        if (Sprite != null)
        {
            Sprite.enabled = !locked;
        }
    }

    protected Collider2D GetCollectCollider()
    {
        if (_collectCollider == null)
        {
            _collectCollider = GetComponent<CircleCollider2D>();
        }
        if (_collectCollider == null)
        {
            _collectCollider = GetComponent<BoxCollider2D>();
        }
        if (_collectCollider == null)
        {
            _collectCollider = GetComponent<Collider2D>();
        }

        return _collectCollider;
    }
}
