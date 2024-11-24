namespace MinishootRandomizer;

public interface IProgressionStorage
{
    public void SetLocationChecked(Location location, bool isChecked = true);
    public bool IsLocationChecked(Location location);
    public bool IsGoalCompleted(Goals goal);
}
