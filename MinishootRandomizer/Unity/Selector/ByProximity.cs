using System;
using UnityEngine;

namespace MinishootRandomizer;

public class ByProximity : ISelector
{
    private Type _type;
    private Vector3 _position;
    private float _radius;
    private bool _includeInactive = true;

    public Type Type => _type;
    public Vector3 Position => _position;
    public float Radius => _radius;
    public bool IncludeInactive => _includeInactive;

    public ByProximity(Type type, Vector3 position, float radius, bool includeInactive = true)
    {
        _type = type;
        _position = position;
        _radius = radius;
        _includeInactive = includeInactive;
    }

    public override string ToString()
    {
        return $"Proximity to {_position.x} {_position.y} {_position.z} of type {_type}";
    }
}
