using System;
using System.Collections.Generic;

namespace MinishootRandomizer;

public class InMemoryTrapDialogProvider : ITrapDialogProvider
{
    private static List<string> _dialogs = new List<string>
    {
        "Have you heard of the critically acclaimed MMORPG Final Fantasy XIV?\n With an expanded free trial which you can play through the entirety of A Realm Reborn\n and the award-winning Heavensward, and thrilling Stormblood\n expansions up to level 70 for free with no restrictions on playtime.",
        "Yo, Minishoot! How're you doing? This is Joey, got a minute?\nHow are your Pokémon doing? My Rattata's looking sharper than before!\nI doubt there's a Pokémon as cool as this guy in your party!\nLet's get together and battle! I promise things will be different!\nRoute 30's where I'll be",
        "AHHHHAHAHAHAHAHAHAHA!! FOOOOOOOOOOOOOOOOOOOOOOOOL!!\nYou blew it! You've totally screwed yourself!\nNobody enters my home and leaves in one piece! Tell you what though!\nHang on a minute! You get to live! That's right! Aren't you lucky?!",
        "Yes, indeed. It is called Lothric, where the transitory lands of the Lords of Cinder converge.\nIn venturing north, the pilgrims discover the truth of the old words:\nThe fire fades and the lords go without thrones.\nWhen the link of fire is threatened, the bell tolls,\nunearthing the old Lords of Cinder from their graves...",
        "In wilds beyond they speak your name with reverence and regret,\nFor none could tame our savage souls yet you the challenge met,\nUnder palest watch, you taught, we changed, base instincts were redeemed,\nA world you gave to bug and beast as they had never dreamed.",
        "Many years ago prince Darkness Ganon stole one of the Triforce with power.\nPrincess Zelda had one of the Triforce with wisdom.\nShe divided it into eight units to hide it from Ganon before she was captured.\nGo find the eight units Link to save her.",
        "Episode III A Link to the Past Randomizer\nAfter mostly disregarding what happened in the first two games.\nLink awakens to his uncle leaving the house. He just runs out the door, into the rainy night.\nGanon has moved around all the items in Hyrule. This is your chance to be a hero.",
        "Against all the evil that Hell can conjure,\nall the wickedness that mankind can produce,\nwe will send unto them... only you.\nRip and tear, until it is done.",
        "I plan to give you a taste of my revenge, once all the seven Chaos Emeralds are collected.\n Once I initiate this program, it cannot be disabled.\n All of you ungrateful humans, who took everything away from me...\n will feel my loss, and despair!",
        "We don't know why the Sunken Temple always welcomes its visitors at the south entrance, \njust that this is true. As a child, I considered such unknowns sinister.\nNow, though, I understand they bear no ill will. The universe is, and we are.\nI am ready.",
        "Long ago, two races ruled over Earth: HUMANS and MONSTERS.\nOne day, war broke out between the two races.\nAfter a long battle, the humans were victorious.\nThey sealed the monsters underground with a magic spell.",
        "Oh, Navi the fairy...\nListen to my words, the words of the Deku Tree...\nDost thou sense it? The climate of evil descending upon this realm...\nMalevolent forces even now are mustering to attack our land of Hyrule...",
        "There was a red prince, Who sat on a red throne.\n He had a red crown made of rubies, And a red castle made of stones.\nThe prince had a love, For all things red.\n It's the only true color! The prince often said."
    };

    private Random _random = new Random();

    public string GetDialog()
    {
        return _dialogs[_random.Next(_dialogs.Count)];
    }
}
