namespace MinishootRandomizer;

public class ObjectiveMarkerData
{
    private readonly string _objectiveLocationIdentifier;
    private readonly Goals _objectiveGoal;

    public string ObjectiveLocationIdentifier => _objectiveLocationIdentifier;
    public Goals ObjectiveGoal => _objectiveGoal;

    public ObjectiveMarkerData(string objectiveLocationIdentifier, Goals objectiveGoal)
    {
        _objectiveLocationIdentifier = objectiveLocationIdentifier;
        _objectiveGoal = objectiveGoal;
    }
}
