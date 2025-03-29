using System;
using System.Collections.Generic;
using System.Linq;

namespace MinishootRandomizer;

public class TrapItemPresentationProvider : IItemPresentationProvider
{
    private readonly IItemPresentationProvider _innerProvider;
    private readonly IRandomizerEngine _randomizerEngine;

    private Random _random = new Random();

    public TrapItemPresentationProvider(
        IItemPresentationProvider innerProvider,
        IRandomizerEngine randomizerEngine
    ) {
        _innerProvider = innerProvider;
        _randomizerEngine = randomizerEngine;
    }

    public ItemPresentation GetItemPresentation(Item item)
    {
        if (item.Category != ItemCategory.Trap)
        {
            return _innerProvider.GetItemPresentation(item);
        }

        Item randomItem = GetRandomItemFromPool();
        ItemPresentation falseItemPresentation = _innerProvider.GetItemPresentation(randomItem);
        string misspelledName = Misspell(falseItemPresentation.Name);
        ItemPresentation trueItemPresentation = _innerProvider.GetItemPresentation(item);

        TrapItemPresentation itemPresentation = new TrapItemPresentation(randomItem, misspelledName, falseItemPresentation.Description, falseItemPresentation.SpriteData);
        itemPresentation.SetTrueItemPresentation(trueItemPresentation);

        return itemPresentation;
    }

    private Item GetRandomItemFromPool()
    {
        TrapItemsAppearanceValue trapItemsAppearance = _randomizerEngine.GetSetting<TrapItemsAppearance>().Value;
        List<Location> locations = _randomizerEngine.GetRandomizedLocations();
        List<Location> elligibleLocations = new List<Location>();

        foreach (Location location in locations)
        {
            Item item = _randomizerEngine.PeekLocation(location);
            if (item.Category == ItemCategory.Trap || item is ArchipelagoItem)
            {
                continue;
            }
            if (trapItemsAppearance == TrapItemsAppearanceValue.Anything)
            {
                elligibleLocations.Add(location);
            }
            else
            {
                if (
                    trapItemsAppearance == TrapItemsAppearanceValue.MajorItems &&
                    (item.Category == ItemCategory.Progression || item.Category == ItemCategory.Token)
                ) {
                    elligibleLocations.Add(location);
                }
                else if (
                    trapItemsAppearance == TrapItemsAppearanceValue.JunkItems &&
                    (item.Category == ItemCategory.Filler || item.Category == ItemCategory.Helpful)
                ) {
                    elligibleLocations.Add(location);
                }
            }
        }

        if (elligibleLocations.Count == 0)
        {
            throw new ItemNotFoundException("No elligible locations found");
        }

        Location randomLocation = elligibleLocations[_random.Next(elligibleLocations.Count)];

        return _randomizerEngine.PeekLocation(randomLocation);
    }

    private string Misspell(string input)
    {
        // We replace a random vowel with a random other vowel, and a random consonant with a random other consonant.
        char[] vowels = { 'a', 'e', 'i', 'o', 'u' };
        char[] consonants = { 'b', 'c', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'm', 'n', 'p', 'q', 'r', 's', 't', 'v', 'w', 'x', 'y', 'z' };
        Dictionary<int, Func<char>> replacements = new Dictionary<int, Func<char>>();

        // First, we build a dictionary of possible replacements, depending on the character type.
        char[] inputArray = input.ToCharArray();
        for (int i = 0; i < inputArray.Length; i++)
        {
            char c = inputArray[i];
            if (char.IsWhiteSpace(c))
            {
                continue;
            }
            if (char.IsLetter(c))
            {
                if (vowels.Contains(char.ToLower(c)))
                {
                    replacements.Add(i, () =>  {
                        char newVowel = vowels[_random.Next(vowels.Length)];
                        return char.IsUpper(c) ? char.ToUpper(newVowel) : newVowel;
                    });
                }
                else if (consonants.Contains(char.ToLower(c)))
                {
                    replacements.Add(i, () =>  {
                        char newConsonant = consonants[_random.Next(consonants.Length)];
                        return char.IsUpper(c) ? char.ToUpper(newConsonant) : newConsonant;
                    });
                }
            }
            else if (char.IsDigit(c))
            {
                replacements.Add(i, () =>  {
                    return (char)_random.Next(10);
                });
            }
        }

        // Then, we randomly select two replacements and apply them to the input string.
        for (int i = 0; i < 2; i++)
        {
            if (replacements.Count == 0)
            {
                break;
            }

            int randomIndex = _random.Next(replacements.Count);
            KeyValuePair<int, Func<char>> replacement = replacements.ElementAt(randomIndex);
            inputArray[replacement.Key] = replacement.Value();
            replacements.Remove(replacement.Key);
        }

        return new string(inputArray);
    }
}
