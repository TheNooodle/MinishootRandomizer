using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using CsvHelper;

namespace MinishootRandomizer
{
    public class CsvRegionRepository : IRegionRepository
    {
        private readonly string _csvPath;

        private Dictionary<string, Region> _regions = null;

        public CsvRegionRepository(string csvPath)
        {
            _csvPath = csvPath;
        }

        public Region Get(string identifier)
        {
            if (_regions == null)
            {
                LoadRegions();
            }

            if (!_regions.ContainsKey(identifier))
            {
                throw new RegionNotFoundException(identifier);
            }

            return _regions[identifier];
        }

        private void LoadRegions()
        {
            var assembly = Assembly.GetExecutingAssembly();
            _regions = new Dictionary<string, Region>();

            using Stream stream = assembly.GetManifestResourceStream(_csvPath);
            using StreamReader reader = new(stream);
            using CsvReader csv = new(reader, CultureInfo.InvariantCulture);
            csv.Read();
            csv.ReadHeader();
            while (csv.Read())
            {
                Region region = new Region(csv.GetField("Name"));
                foreach (string zone in csv.GetField("Locations").Split(','))
                {
                    region.AddLocationName(zone);
                }
                _regions.Add(region.Name, region);
            }
        }
    }
}
