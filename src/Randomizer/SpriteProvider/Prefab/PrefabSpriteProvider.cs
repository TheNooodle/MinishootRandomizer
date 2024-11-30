using System;
using System.Collections.Generic;
using UnityEngine;

namespace MinishootRandomizer;

public class PrefabSpriteProvider : ISpriteProvider, IPrefabCollector
{
    private readonly IObjectFinder _objectFinder;
    private readonly ILogger _logger = new NullLogger();

    private List<IPrefabSpriteExtractionStrategy> _strategies = new List<IPrefabSpriteExtractionStrategy>();

    private readonly Dictionary<string, SpriteData> _sprites = new Dictionary<string, SpriteData>();

    private readonly Dictionary<string, Tuple<float, float>> _spriteParameters = new Dictionary<string, Tuple<float, float>>()
    {
        { "Xp Crystals", new Tuple<float, float>(0.9f, 0f) },
        { "Academician NPC", new Tuple<float, float>(0.7f, -50f) },
        { "Bard NPC", new Tuple<float, float>(0.7f, -50f) },
        { "Blacksmith NPC", new Tuple<float, float>(0.6f, -50f) },
        { "Explorer NPC", new Tuple<float, float>(0.7f, -50f) },
        { "Family Parent 1 NPC", new Tuple<float, float>(0.7f, -50f) },
        { "Family Parent 2 NPC", new Tuple<float, float>(0.7f, -50f) },
        { "Family Child NPC", new Tuple<float, float>(0.7f, -50f) },
        { "Healer NPC", new Tuple<float, float>(0.7f, -50f) },
        { "Mercant NPC", new Tuple<float, float>(0.5f, -50f) },
        { "Scarab Collector NPC", new Tuple<float, float>(0.7f, -50f) },
    };

    public PrefabSpriteProvider(IObjectFinder objectFinder, ILogger logger = null)
    {
        _objectFinder = objectFinder;
        _logger = logger ?? new NullLogger();
    }

    public void AddStrategy(IPrefabSpriteExtractionStrategy strategy)
    {
        _strategies.Add(strategy);
    }

    public void AddPrefab(string prefabIdentifier, ISelector selector, CloningType cloningType)
    {
        try
        {
            GameObject prefab = _objectFinder.FindObject(selector);

            foreach (IPrefabSpriteExtractionStrategy strategy in _strategies)
            {
                try
                {
                    Sprite sprite = strategy.ExtractSprite(prefabIdentifier, prefab);
                    _sprites[prefabIdentifier] = CreateSpriteData(prefabIdentifier, sprite);
                    return;
                }
                catch (CannotExtractSpriteException)
                {
                    continue;
                }
            }
        }
        catch (ObjectNotFoundException)
        {
            _logger.LogWarning($"Prefab not found for {prefabIdentifier} with selector {selector}");
            return;
        }
    }

    private SpriteData CreateSpriteData(string prefabIdentifier, Sprite sprite)
    {
        if (_spriteParameters.ContainsKey(prefabIdentifier))
        {
            return new SpriteData(sprite, _spriteParameters[prefabIdentifier].Item1, _spriteParameters[prefabIdentifier].Item2);
        }        

        return new SpriteData(sprite);
    }

    public SpriteData GetSprite(string identifier)
    {
        if (_sprites.ContainsKey(identifier))
        {
            return _sprites[identifier];
        }

        throw new SpriteNotFound($"Sprite not found for {identifier}");
    }
}
