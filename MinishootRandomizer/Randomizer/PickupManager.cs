using System.Collections.Generic;

namespace MinishootRandomizer;

public class PickupManager
{
    private List<RandomizerPickup> pickups = new List<RandomizerPickup>();

    public void AddPickup(RandomizerPickup pickup)
    {
        pickups.Add(pickup);
    }

    public void RemovePickup(RandomizerPickup pickup)
    {
        pickups.Remove(pickup);
    }

    public void OnLoadingSaveFile(bool isNewGame)
    {
        HandleOnLoadingSaveFile(isNewGame);
    }

    public void OnNpcFreed()
    {
        HandleCheckActivation();
    }

    public void OnPlayerStatsChanged()
    {
        HandleCheckActivation();
    }

    public void OnEnteringEncounter()
    {
        HandleLockDuringEncounter();
    }

    public void OnExitingEncounter()
    {
        HandleUnlockDuringEncounter();
    }

    private void HandleOnLoadingSaveFile(bool isNewGame)
    {
        foreach (RandomizerPickup pickup in pickups)
        {
            pickup.OnLoadingSaveFile(isNewGame);
        }
    }

    private void HandleCheckActivation()
    {
        foreach (RandomizerPickup pickup in pickups)
        {
            pickup.CheckActivation();
        }
    }

    private void HandleLockDuringEncounter()
    {
        foreach (RandomizerPickup pickup in pickups)
        {
            pickup.SetLockedDuringEncounter(true);
        }
    }

    private void HandleUnlockDuringEncounter()
    {
        foreach (RandomizerPickup pickup in pickups)
        {
            pickup.SetLockedDuringEncounter(false);
        }
    }
}
