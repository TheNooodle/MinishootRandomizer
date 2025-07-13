namespace MinishootRandomizer;

public class EmoteListener
{
    public void OnItemCollected(Item item)
    {
        switch (item.Category)
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
}
