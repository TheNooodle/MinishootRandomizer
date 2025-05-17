using System.Collections.Generic;

namespace MinishootRandomizer;

public class LogicState
{
    private Dictionary<string, int> _itemCount = new();
    private List<ISetting> _settings = new();

    public void AddItemCount(Item item, int count = 1)
    {
        if (_itemCount.TryGetValue(item.Identifier, out int owned))
        {
            _itemCount[item.Identifier] = owned + count;
        }
        else
        {
            _itemCount.Add(item.Identifier, count);
        }
    }

    public void SetItemCount(Item item, int count = 1)
    {
        _itemCount[item.Identifier] = count;
    }

    public virtual bool HasItem(Item item, int count = 1)
    {
        return _itemCount.TryGetValue(item.Identifier, out int owned) && owned >= count;
    }

    public void SetSetting(ISetting setting)
    {
        ISetting existingSetting = _settings.Find(s => s.GetType() == setting.GetType());
        if (existingSetting != null)
        {
            _settings.Remove(existingSetting);
        }

        _settings.Add(setting);
    }

    public virtual T GetSetting<T>() where T : ISetting
    {
        foreach (ISetting setting in _settings)
        {
            if (setting is T typedSetting)
            {
                return typedSetting;
            }
        }

        return default;
    }

    public virtual string GetCacheKey()
    {
        return LogicTolerance.Strict.ToString();
    }
}
