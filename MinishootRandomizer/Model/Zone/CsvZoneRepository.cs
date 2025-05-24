using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using CsvHelper;

namespace MinishootRandomizer
{
    public class CsvZoneRepository : IZoneRepository
    {
        private readonly string _csvPath;
        private readonly IZoneFactory _zoneFactory;

        private Dictionary<string, Zone> _zones = null;

        public CsvZoneRepository(string csvPath, IZoneFactory zoneFactory)
        {
            _csvPath = csvPath;
            _zoneFactory = zoneFactory;
        }

        public Zone Get(string identifier)
        {
            if (_zones == null)
            {
                LoadZones();
            }

            if (!_zones.ContainsKey(identifier))
            {
                throw new ZoneNotFoundException(identifier);
            }

            return _zones[identifier];
        }

        public List<Zone> GetByGameLocationName(string gameLocationName)
        {
            if (_zones == null)
            {
                LoadZones();
            }

            List<Zone> zones = new();
            foreach (Zone zone in _zones.Values)
            {
                if (zone.GameLocationName == gameLocationName)
                {
                    zones.Add(zone);
                }
            }

            return zones;
        }

        private void LoadZones()
        {
            _zones = new Dictionary<string, Zone>();

            using Stream stream = StreamFactory.CreateStream(_csvPath);
            using StreamReader reader = new(stream);
            using CsvReader csv = new(reader, CultureInfo.InvariantCulture);
            csv.Read();
            csv.ReadHeader();
            while (csv.Read())
            {
                Zone zone = _zoneFactory.Create(csv.GetField("Name"), csv.GetField("Regions"));
                _zones.Add(zone.Name, zone);
            }
        }
    }
}
