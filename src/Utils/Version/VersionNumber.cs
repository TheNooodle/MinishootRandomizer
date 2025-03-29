using System;
using System.Collections.Generic;
using System.Linq;

namespace MinishootRandomizer;

// Represents a version number with major, minor, patch, and label components.
public class VersionNumber
{
    private readonly int _major;
    private readonly int _minor;
    private readonly int _patch;
    private readonly string _label;

    public int Major => _major;
    public int Minor => _minor;
    public int Patch => _patch;
    public string Label => _label;

    public VersionNumber(int major, int minor, int patch, string label = "")
    {
        _major = major;
        _minor = minor;
        _patch = patch;
        _label = label;
    }

    public VersionNumber(string version)
    {
        string[] parts = version.Split('.');
        _major = int.Parse(parts[0]);
        _minor = int.Parse(parts.Length > 1 ? parts[1] : "0");
        string patch = parts.Length > 2 ? parts[2] : "0";
        string[] patchParts = patch.Split('-');
        _patch = int.Parse(patchParts[0]);
        _label = patchParts.Length > 1 ? patchParts[1] : "";
    }

    // Returns true if the version number satisfies the given constraint.
    // The constraint can be an exact version number or a version range.
    // The wildcard character '*' can be used to match any part of the version number.
    // The '~' character specifies a minimum version, but allows the last digit to go up.
    // Examples:
    // - "1.2.3" matches version number "1.2.3"
    // - "1.2.*" matches version numbers "1.2.0", "1.2.1", "1.2.2", etc.
    // - "~1.2" matches version numbers "1.2.0", "1.3.0", "1.4.0", etc.
    public bool Satisfy(string constraint)
    {
        if (constraint.First() == '~')
        {
            string[] parts = constraint.Substring(1).Split('.');
            if (parts.Length == 1)
            {
                return _major > int.Parse(parts[0]);
            }
            if (parts.Length == 2)
            {
                return _major == int.Parse(parts[0]) && _minor >= int.Parse(parts[1]);
            }
            if (parts.Length == 3)
            {
                return _major == int.Parse(parts[0]) && _minor == int.Parse(parts[1]) && _patch >= int.Parse(parts[2]);
            }

            throw new ArgumentException("Invalid version constraint");
        }
        else
        {
            string[] parts = constraint.Split('.');
            List<Func<bool>> checks = new List<Func<bool>>();
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i] == "*")
                {
                    checks.Add(() => true);
                }
                else
                {
                    int value = int.Parse(parts[i]);
                    checks.Add(() => value == new[] { _major, _minor, _patch }[i]);
                }
            }

            if (checks.Count == 0)
            {
                throw new ArgumentException("Invalid version constraint");
            }

            return checks.All(check => check());
        }
    }

    public override string ToString()
    {
        return $"{_major}.{_minor}.{_patch}{(_label != "" ? $"-{_label}" : "")}";
    }
}
