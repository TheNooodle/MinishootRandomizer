using System;
using System.Collections.Generic;
using System.Linq;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.Models;
using Archipelago.MultiClient.Net.Packets;

namespace MinishootRandomizer;

public class MultiClient : IArchipelagoClient
{
    private const float TimeoutSeconds = 5f;

    private readonly IItemCounter _itemCounter;
    private readonly ILogger _logger = new NullLogger();

    private IArchipelagoSession _session;
    private ArchipelagoOptions _options;
    private LoginSuccessful _loginResult = null;

    private Dictionary<long, string> _locationIdToName = new();
    private Dictionary<string, ArchipelagoItemData> _locationNameToItemData = new();
    private Dictionary<string, object> _dataStorage = new();

    public IArchipelagoSession Session => _session;
    public LoginSuccessful LoginResult => _loginResult;

    public delegate void ItemReceivedHandler(ArchipelagoItemData itemData);
    public event ItemReceivedHandler ItemReceived;

    public MultiClient(IItemCounter itemCounter, ILogger logger = null)
    {
        _itemCounter = itemCounter;
        _logger = logger ?? new NullLogger();
    }

    public void Connect(ArchipelagoOptions options)
    {
        _options = options;
        Disconnect();
        LoginResult result;

        try
        {
            _logger.LogInfo($"Connecting to {_options.Uri} as {_options.SlotName}");
            _session = ArchipelagoSessionFactory.CreateSession(_options.Uri);
            List<string> flags = new List<string>();
            if (_options.IsDeathLink)
            {
                flags.Add("DeathLink");
            }
            result = _session.TryConnectAndLogin(
                "Minishoot Adventures",
                _options.SlotName,
                ItemsHandlingFlags.IncludeStartingInventory,
                null,
                flags.ToArray(),
                null,
                _options.Password,
                true
            );
        }
        catch (Exception e)
        {
            throw new ArchipelagoLoginException("Failed to connect to Archipelago.", e);
        }

        if (!result.Successful)
        {
            LoginFailure failure = (LoginFailure)result;
            string errorMessage = $"Failed to Connect to {_options.Uri} as {_options.SlotName}:";
            foreach (string error in failure.Errors)
            {
                errorMessage += $"\n    {error}";
            }
            foreach (ConnectionRefusedError error in failure.ErrorCodes)
            {
                errorMessage += $"\n    {error}";
            }

            throw new ArchipelagoLoginException(errorMessage, null);
        }

        _logger.LogInfo($"Connected to {_options.Uri} as {_options.SlotName}");
        _loginResult = (LoginSuccessful)result;
        
        _dataStorage.Clear();
        _locationIdToName.Clear();
        _locationNameToItemData.Clear();
        GetAllLocationNames();
        ScoutItems();
        _session.Items.ItemReceived -= OnItemsReceived;
        _session.Items.ItemReceived += OnItemsReceived;
        _dataStorage = _session.DataStorage.GetSlotData();
    }

    public void Disconnect()
    {
        if (_session != null)
        {
            _session.Socket.DisconnectAsync();
        }

        _session = null;
        _loginResult = null;
    }

    private List<string> GetLocationNames(ICollection<long> locationIds)
    {
        var locationNames = new List<string>();
        foreach (var locationId in locationIds)
        {
            if (!_locationIdToName.ContainsKey(locationId))
            {
                var locationName = _session.Locations.GetLocationNameFromId(locationId);
                _locationIdToName.Add(locationId, locationName);
            }
            locationNames.Add(_locationIdToName[locationId]);
        }

        return locationNames;
    }

    private string GetLocationName(long locationId)
    {
        if (!_locationIdToName.ContainsKey(locationId))
        {
            _locationIdToName.Add(locationId, _session.Locations.GetLocationNameFromId(locationId));
        }

        return _locationIdToName[locationId];
    }

    public List<string> GetAllLocationNames()
    {
        return GetLocationNames(_session.Locations.AllLocations);
    }

    public List<string> GetCheckedLocationNames()
    {
        return GetLocationNames(_session.Locations.AllLocationsChecked);
    }

    public void ScoutItems()
    {
        if (_session == null)
        {
            _logger.LogWarning("Session is null, reconnecting.");
            Connect(_options);
        }

        _logger.LogInfo("Scouting items");
        if (_locationIdToName.Count == 0)
        {
            GetAllLocationNames();
        }
        bool success = _session.Locations.ScoutLocationsAsync(_locationIdToName.Keys.ToArray<long>()).ContinueWith(resultDict =>
        {
            foreach (long locationId in resultDict.Result.Keys)
            {
                ItemInfo itemInfo = resultDict.Result[locationId];
                ArchipelagoItemData itemData = GetArchipelagoItemData(itemInfo);
                _locationNameToItemData.Add(GetLocationName(locationId), itemData);
            }
        }).Wait(TimeSpan.FromSeconds(TimeoutSeconds));
        CheckConnection();

        if (!success || !IsConnected())
        {
            throw new ArchipelagoConnectionException("Failed to scout items : Timeout");
        }
    }

    private ArchipelagoItemData GetArchipelagoItemData(ItemInfo item)
    {
        return new ArchipelagoItemData(
            item.ItemId,
            item.ItemName,
            MapItemFlagsToCategory(item.Flags),
            item.Player.Name,
            item.Player.Slot == _loginResult.Slot
        );
    }

    private ArchipelagoItemCategory MapItemFlagsToCategory(ItemFlags flags)
    {
        return flags switch
        {
            ItemFlags.Advancement => ArchipelagoItemCategory.Progression,
            ItemFlags.NeverExclude => ArchipelagoItemCategory.Useful,
            ItemFlags.Trap => ArchipelagoItemCategory.Trap,
            ItemFlags.None or _ => ArchipelagoItemCategory.Filler,
        };
    }

    public ArchipelagoItemData GetItemData(string locationName)
    {
        if (_locationNameToItemData.ContainsKey(locationName))
        {
            return _locationNameToItemData[locationName];
        }

        throw new ArchipelagoLogicException($"Location {locationName} not found in the scout results.");
    }

    public void CheckLocations(List<string> locationNames)
    {
        if (_session == null)
        {
            _logger.LogWarning("Session is null, reconnecting.");
            Connect(_options);
        }

        _logger.LogInfo("Checking locations");
        List<long> locationIds = new List<long>();
        foreach (var locationName in locationNames)
        {
            if (!_locationIdToName.ContainsValue(locationName))
            {
                throw new ArchipelagoLogicException($"Location {locationName} not found in the scout results.");
            }

            locationIds.Add(_locationIdToName.First(x => x.Value == locationName).Key);
        }
        bool isSuccess = _session.Locations.CompleteLocationChecksAsync(locationIds.ToArray<long>()).Wait(TimeSpan.FromSeconds(TimeoutSeconds));
        CheckConnection();
        if (!isSuccess || !IsConnected())
        {
            throw new ArchipelagoConnectionException("Failed to check locations : Timeout");
        }
    }

    private void CheckConnection()
    {
        if (!_session.Socket.Connected)
        {
            _logger.LogWarning("Connection lost, disconnecting properly.");
            Disconnect();            
        }
    }

    private void OnItemsReceived(IReceivedItemsHelper receivedItemsHelper)
    {
        _logger.LogInfo("Received items");
        int localCount = _itemCounter.GetCount();
        for (int i = 0; i < receivedItemsHelper.Index; i++)
        {
            if (i < localCount)
            {
                receivedItemsHelper.DequeueItem();
                continue;
            }
            else
            {
                ItemInfo itemInfo = receivedItemsHelper.AllItemsReceived[i];
                _logger.LogInfo($"Received item {itemInfo.ItemName}");
                ArchipelagoItemData itemData = GetArchipelagoItemData(itemInfo);
                ItemReceived?.Invoke(itemData);
                receivedItemsHelper.DequeueItem();
                _itemCounter.Increment();
            }
        }
    }

    public object GetDataStorageValue(string key)
    {
        if (_dataStorage.ContainsKey(key))
        {
            return _dataStorage[key];
        }

        throw new ArchipelagoLogicException($"Key {key} not found in the data storage.");
    }

    public void SendCompletion()
    {
        if (_session == null)
        {
            _logger.LogWarning("Session is null, reconnecting.");
            Connect(_options);
        }

        _logger.LogInfo("Sending completion");
        StatusUpdatePacket statusUpdatePacket = new StatusUpdatePacket();
        statusUpdatePacket.Status = ArchipelagoClientState.ClientGoal;
        bool isSuccess = _session.Socket.SendPacketAsync(statusUpdatePacket).Wait(TimeSpan.FromSeconds(TimeoutSeconds));
        CheckConnection();
        if (!isSuccess || !IsConnected())
        {
            throw new ArchipelagoConnectionException("Failed to check locations : Timeout");
        }
    }

    public void ReceiveItems()
    {
        if (_session == null)
        {
            _logger.LogWarning("Session is null, cannot receive items.");
            return;
        }

        OnItemsReceived(_session.Items);
    }

    public bool IsConnected()
    {
        return _loginResult != null;
    }
}
