using System;

namespace MinishootRandomizer;

/// <summary>
/// Represents a service definition that can be consumed by any DI container implementation
/// </summary>
public class ServiceDefinition
{
    /// <summary>
    /// The service type (usually an interface) that can be resolved from the container
    /// </summary>
    public Type ServiceType { get; }
    
    /// <summary>
    /// The implementation type that will be instantiated
    /// </summary>
    public Type ImplementationType { get; }
    
    /// <summary>
    /// Factory method to create the service instance
    /// </summary>
    public Func<IServiceContainer, object> FactoryMethod { get; }
    
    /// <summary>
    /// Pre-created instance for instance registrations
    /// </summary>
    public object Instance { get; }
    
    // Constructor for implementation type registration
    private ServiceDefinition(Type serviceType, Type implementationType)
    {
        ServiceType = serviceType;
        ImplementationType = implementationType;
        FactoryMethod = null;
        Instance = null;
    }
    
    // Constructor for factory method registration
    private ServiceDefinition(Type serviceType, Func<IServiceContainer, object> factoryMethod)
    {
        ServiceType = serviceType;
        FactoryMethod = factoryMethod;
        ImplementationType = null;
        Instance = null;
    }
    
    // Constructor for instance registration
    private ServiceDefinition(Type serviceType, object instance)
    {
        ServiceType = serviceType;
        Instance = instance;
        ImplementationType = null;
        FactoryMethod = null;
    }
    
    // Factory methods
    public static ServiceDefinition FromType(Type serviceType, Type implementationType)
    {
        return new ServiceDefinition(serviceType, implementationType);
    }
    
    public static ServiceDefinition FromFactory(Type serviceType, Func<IServiceContainer, object> factoryMethod)
    {
        return new ServiceDefinition(serviceType, factoryMethod);
    }
    
    public static ServiceDefinition FromInstance(Type serviceType, object instance)
    {
        return new ServiceDefinition(serviceType, instance);
    }
}
