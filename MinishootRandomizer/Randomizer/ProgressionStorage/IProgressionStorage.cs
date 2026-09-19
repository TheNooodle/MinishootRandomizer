namespace MinishootRandomizer;

public interface IProgressionStorage
{
    public void SetLocationChecked(Location location, bool isChecked = true);
    public int GetLocationCheckedCount();
    public bool IsLocationChecked(Location location);
    public bool IsGoalCompleted(Goals goal);
}
