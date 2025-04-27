using System;

namespace MinishootRandomizer;

public class InvalidArchipelagoVersionException : ArchipelagoLoginException
{
    public VersionNumber ClientVersion { get; }
    public VersionNumber ServerVersion { get; }
    public string Constraint { get; }

    public InvalidArchipelagoVersionException(VersionNumber clientVersion, VersionNumber serverVersion, string constraint, Exception e) : base(
        $"Invalid Archipelago version. Client version: {clientVersion}, Server version: {serverVersion}, Client Constraint: {constraint}", e
    )
    {
        ClientVersion = clientVersion;
        ServerVersion = serverVersion;
        Constraint = constraint;
    }
}
