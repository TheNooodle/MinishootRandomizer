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
        if (_transitions == null)
        {
            LoadTransitions();
        }

        List<Transition> transitions = new List<Transition>();
        foreach (Transition transition in _transitions.Values)
        {
            if (transition.From == region.Name)
            {
                transitions.Add(transition);
            }
        }

        return transitions;
    }

    public Transition Get(string identifier)
    {
        if (_transitions == null)
        {
            LoadTransitions();
        }

        if (!_transitions.ContainsKey(identifier))
        {
            throw new TransitionNotFoundException(identifier);
        }

        return _transitions[identifier];
    }

    private void LoadTransitions()
    {
        _transitions = new Dictionary<string, Transition>();

        using Stream stream = StreamFactory.CreateStream(_csvPath);
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
