using System.Collections.Generic;
using UnityEngine;

namespace MinishootRandomizer;

public class KeyCounterComponent : MonoBehaviour
{
    public int DungeonId;
    public SmallKeyItem SmallKeyItem;
    public BossKeyItem BossKeyItem;

    private List<KeyCounterSpriteComponent> _smallKeyCounterSprites = new List<KeyCounterSpriteComponent>();
    private KeyCounterSpriteComponent _bossKeyCounterSprite;

    void Update()
    {
        UpdateKeySprites();
    }

    public void UpdateKeySprites()
    {
        // For small keys, we calculate the number of keys spent and the number of keys remaining.
        int collectedSmallKeyCount = SmallKeyItem.GetOwnedQuantity();
        int availableSmallKeyCount = PlayerState.DungeonKeys[SmallKeyItem.DungeonId];
        int spentSmallKeyCount = collectedSmallKeyCount - availableSmallKeyCount;
        for (int i = 0; i < _smallKeyCounterSprites.Count; i++)
        {
            if (i < spentSmallKeyCount)
            {
                _smallKeyCounterSprites[i].SetStatus(KeyStatus.Used);
            }
            else if (i < collectedSmallKeyCount)
            {
                _smallKeyCounterSprites[i].SetStatus(KeyStatus.Available);
            }
            else
            {
                _smallKeyCounterSprites[i].SetStatus(KeyStatus.Missing);
            }
        }

        // For the boss key, we check if it is collected, spent or missing.
        int collectedBossKeyCount = BossKeyItem.GetOwnedQuantity();
        int availableBossKeyCount = PlayerState.DungeonBossKeys[BossKeyItem.DungeonId];
        if (collectedBossKeyCount > 0)
        {
            if (availableBossKeyCount > 0)
            {
                _bossKeyCounterSprite.SetStatus(KeyStatus.Available);
            }
            else
            {
                _bossKeyCounterSprite.SetStatus(KeyStatus.Used);
            }
        }
        else
        {
            _bossKeyCounterSprite.SetStatus(KeyStatus.Missing);
        }
    }

    public void AddSmallKeySprite(KeyCounterSpriteComponent sprite)
    {
        _smallKeyCounterSprites.Add(sprite);
    }

    public void SetBossKeySprite(KeyCounterSpriteComponent sprite)
    {
        _bossKeyCounterSprite = sprite;
    }
}
