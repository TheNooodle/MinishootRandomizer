namespace MinishootRandomizer;

public class CompletionGoals : ISetting
{
    public Goals Goal { get; set; }

    public CompletionGoals(Goals goal)
    {
        Goal = goal;
    }
}
