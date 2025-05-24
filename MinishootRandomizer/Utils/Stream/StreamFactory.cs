using System;
using System.IO;
using System.Reflection;

namespace MinishootRandomizer;

public class StreamFactory
{
    public static Stream CreateStream(string resourcePath)
    {
        if (string.IsNullOrEmpty(resourcePath))
        {
            throw new ArgumentException("Resource path cannot be null or empty.", nameof(resourcePath));
        }

        // If the resource path is a file path, create a FileStream
        if (File.Exists(resourcePath))
        {
            return new FileStream(resourcePath, FileMode.Open, FileAccess.Read);
        }

        // Otherwise, assume it's an embedded resource and create a Stream from it
        var assembly = Assembly.GetExecutingAssembly();
        return assembly.GetManifestResourceStream(resourcePath)
            ?? throw new InvalidOperationException($"Resource '{resourcePath}' not found in assembly.");
    }
}
