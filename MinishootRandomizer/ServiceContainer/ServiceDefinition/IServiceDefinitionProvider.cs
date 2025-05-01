using System;
using System.Collections.Generic;

namespace MinishootRandomizer;

/// <summary>
/// Interface for providing service definitions to a DI container
/// </summary>
public interface IServiceDefinitionProvider
{
    /// <summary>
    /// Gets all service definitions that should be registered with the container
    /// </summary>
    IEnumerable<ServiceDefinition> GetServiceDefinitions();
    
    /// <summary>
    /// Gets all post-build actions that should be executed after the container is built
    /// </summary>
    IEnumerable<PostBuildAction> GetPostBuildActions();
}

/// <summary>
/// Represents an action to be executed after the container is built
/// </summary>
public class PostBuildAction
{
    /// <summary>
    /// The action to execute, which receives the service resolver interface
    /// </summary>
    public Action<IServiceContainer> Action { get; }
    
    public PostBuildAction(Action<IServiceContainer> action)
    {
        Action = action;
    }
}
