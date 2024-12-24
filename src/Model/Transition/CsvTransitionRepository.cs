using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using CsvHelper;

namespace MinishootRandomizer;

public class CsvTransitionRepository : ITransitionRepository
{
    private readonly string _csvPath;
    private readonly ILogger _logger;

    private Dictionary<string, Transition> _transitions = null;

    public CsvTransitionRepository(string csvPath, ILogger logger = null)
    {
        _csvPath = csvPath;
        _logger = logger ?? new NullLogger();
    }

    public List<Transition> GetFromOriginRegion(Region region)
    {
        throw new System.NotImplementedException();
    }

    public Transition GetTransition(string name)
    {
        throw new System.NotImplementedException();
    }

    private void LoadTransitions()
    {
        var assembly = Assembly.GetExecutingAssembly();
        _transitions = new Dictionary<string, Transition>();

        using Stream stream = assembly.GetManifestResourceStream(_csvPath);
        using StreamReader reader = new(stream);
        using CsvReader csv = new(reader, CultureInfo.InvariantCulture);
        csv.Read();
        csv.ReadHeader();
        while (csv.Read())
        {
            try {
                Transition transition = new Transition(
                    csv.GetField("Name"),
                    csv.GetField("Origin Region"),
                    csv.GetField("Destination Region"),
                    csv.GetField("Logic rule")
                );
                _transitions.Add(transition.Name, transition);
            } catch (InvalidLocationException e) {
                _logger.LogWarning(e.Message);
            }
        }
    }
}
