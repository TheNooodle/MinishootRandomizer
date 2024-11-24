using System.Collections.Generic;

namespace MinishootRandomizer;

public interface IArchipelagoClient
{
    void Connect(ArchipelagoOptions options);
    void Disconnect();
    bool IsConnected();
    List<string> GetAllLocationNames();
    List<string> GetCheckedLocationNames();
    void ScoutItems();
    void ReceiveItems();
    ArchipelagoItemData GetItemData(string locationName);
    void CheckLocations(List<string> locationNames);
    object GetDataStorageValue(string key);
    void SendCompletion();
}
