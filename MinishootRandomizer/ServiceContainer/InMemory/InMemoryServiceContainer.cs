using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MinishootRandomizer;

/// <summary>
/// Implementation of service container using a simple in-memory DI container
/// </summary>
public class InMemoryServiceContainer : IServiceContainer, IBuildable
{
    private class Registration
    {
        public ServiceDefinition Definition { get; }
        public object Instance { get; private set; }
        public bool IsResolved { get; private set; }

        public Registration(ServiceDefinition definition)
        {
            Definition = definition;
        }

        public void SetResolved(object instance)
        {
            Instance = instance;
            IsResolved = true;
        }
    }

    private readonly IServiceDefinitionProvider _definitionProvider;
    private readonly Dictionary<Type, Registration> _registrations = new();
    private readonly HashSet<Type> _resolving = new();
    private bool _isBuilt;

    public InMemoryServiceContainer(
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

        // Register the service definitions (last registration wins)
        foreach (ServiceDefinition definition in _definitionProvider.GetServiceDefinitions())
        {
            _registrations[definition.ServiceType] = new Registration(definition);
        }

        _isBuilt = true;

        // Execute post-build actions
        foreach (PostBuildAction action in _definitionProvider.GetPostBuildActions())
        {
            action.Action(this);
        }
    }

    public T Get<T>() where T : class
    {
        var service = Resolve(typeof(T)) as T;
        if (service == null)
        {
            throw new ServiceNotFoundException($"Service of type {typeof(T).Name} not found");
        }
        return service;
    }

    public bool Has<T>() where T : class
    {
        return _registrations.ContainsKey(typeof(T));
    }

    private object Resolve(Type serviceType)
    {
        if (!_registrations.TryGetValue(serviceType, out var registration))
        {
            return null;
        }

        if (registration.IsResolved)
        {
            return registration.Instance;
        }

        if (!_resolving.Add(serviceType))
        {
            throw new ServiceCircularDependencyException(
                $"Circular dependency detected while resolving service of type {serviceType.Name}");
        }

        try
        {
            var instance = CreateInstance(registration.Definition);
            registration.SetResolved(instance);
            return instance;
        }
        finally
        {
            _resolving.Remove(serviceType);
        }
    }

    private object CreateInstance(ServiceDefinition definition)
    {
        if (definition.Instance != null)
        {
            // Instance registration
            return definition.Instance;
        }

        if (definition.FactoryMethod != null)
        {
            // Factory registration
            return definition.FactoryMethod(this);
        }

        if (definition.ImplementationType != null)
        {
            // Implementation type registration with constructor injection
            return CreateInstanceFromType(definition.ImplementationType);
        }

        throw new InvalidOperationException(
            $"Service definition for type {definition.ServiceType.Name} has no instance, factory method, or implementation type");
    }

    private object CreateInstanceFromType(Type implementationType)
    {
        var constructors = implementationType
            .GetConstructors(BindingFlags.Public | BindingFlags.Instance)
            .OrderByDescending(constructor => constructor.GetParameters().Length)
            .ToList();

        foreach (var constructor in constructors)
        {
            var parameters = constructor.GetParameters();
            var arguments = new object[parameters.Length];
            var canResolve = true;

            for (var i = 0; i < parameters.Length; i++)
            {
                if (!_registrations.ContainsKey(parameters[i].ParameterType))
                {
                    canResolve = false;
                    break;
                }

                arguments[i] = Resolve(parameters[i].ParameterType);
            }

            if (canResolve)
            {
                return constructor.Invoke(arguments);
            }
        }

        throw new InvalidOperationException(
            $"None of the public constructors of type {implementationType.Name} could be satisfied by the container");
    }
}

/// <summary>
/// Thrown when a circular dependency is detected during service resolution
/// </summary>
public class ServiceCircularDependencyException : Exception
{
    public ServiceCircularDependencyException(string message) : base(message)
    {
    }
}
