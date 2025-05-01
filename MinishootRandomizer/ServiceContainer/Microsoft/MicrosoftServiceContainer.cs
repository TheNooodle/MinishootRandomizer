using System;
using BepInEx.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace MinishootRandomizer;

/// <summary>
/// Implementation of service container using Microsoft's DI container
/// </summary>
public class MicrosoftServiceContainer : IServiceContainer, IBuildable
{
    private readonly IServiceDefinitionProvider _definitionProvider;
    private IServiceProvider _serviceProvider;
    private bool _isBuilt;
    
    public MicrosoftServiceContainer(
        IServiceDefinitionProvider definitionProvider
    ) {
        _definitionProvider = definitionProvider;
    }
    
    public void Build()
    {
        if (_isBuilt)
        {
            return;
        }
            
        var serviceCollection = new ServiceCollection();
        
        // Register the service definitions
        foreach (ServiceDefinition definition in _definitionProvider.GetServiceDefinitions())
        {
            RegisterServiceDefinition(serviceCollection, definition);
        }
        
        // Build the service provider
        _serviceProvider = serviceCollection.BuildServiceProvider();
        
        // Execute post-build actions
        foreach (PostBuildAction action in _definitionProvider.GetPostBuildActions())
        {
            action.Action(this);
        }
        
        _isBuilt = true;
    }
    
    public T Get<T>() where T : class
    {
        var service = _serviceProvider.GetService(typeof(T)) as T;
        if (service == null)
        {
            throw new ServiceNotFoundException($"Service of type {typeof(T).Name} not found");
        }
        return service;
    }
    
    public bool Has<T>() where T : class
    {
        return _serviceProvider.GetService(typeof(T)) != null;
    }
    
    private void RegisterServiceDefinition(IServiceCollection services, ServiceDefinition definition)
    {
        if (definition.Instance != null)
        {
            // Instance registration
            services.AddSingleton(definition.ServiceType, definition.Instance);
        }
        else if (definition.FactoryMethod != null)
        {
            // Factory registration
            services.AddSingleton(
                definition.ServiceType,
                sp => definition.FactoryMethod(this)
            );
        }
        else if (definition.ImplementationType != null)
        {
            // Implementation type registration
            services.AddSingleton(definition.ServiceType, definition.ImplementationType);
        }
    }
}
