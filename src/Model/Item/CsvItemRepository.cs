using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using CsvHelper;

namespace MinishootRandomizer;

public class CsvItemRepository : IItemRepository
{
    private readonly IItemFactory _itemFactory;
    private readonly string _csvPath;
    private Dictionary<string, Item> _items = null;

    public CsvItemRepository(IItemFactory itemFactory, string csvPath)
    {
        _itemFactory = itemFactory;
        _csvPath = csvPath;
    }

    public Item Get(string identifier)
    {
        if (_items == null)
        {
            LoadItems();
        }

        if (!_items.ContainsKey(identifier))
        {
            throw new ItemNotFoundException(identifier);
        }

        return _items[identifier];
    }

    private void LoadItems()
    {
        var assembly = Assembly.GetExecutingAssembly();
        _items = new Dictionary<string, Item>();

        using Stream stream = assembly.GetManifestResourceStream(_csvPath);
        using StreamReader reader = new(stream);
        using CsvReader csv = new(reader, CultureInfo.InvariantCulture);
        csv.Read();
        csv.ReadHeader();
        while (csv.Read())
        {
            Item item = _itemFactory.CreateItem(
                csv.GetField("Name"),
                (ItemCategory)Enum.Parse(typeof(ItemCategory), csv.GetField<string>("Category"))
            );
            _items[item.Identifier] = item;
        }
    }
}
