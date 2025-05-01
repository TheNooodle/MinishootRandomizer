namespace MinishootRandomizer.Tests;

public class MicrosoftServiceContainerTests
{
    #region Test Classes and Interfaces
    
    // Basic service interfaces and implementations
    public interface ITestService
    {
        string GetValue();
        void Initialize();
    }

    public class TestService : ITestService
    {
        public bool WasInitialized { get; private set; }

        public string GetValue() => "Original";

        public void Initialize()
        {
            WasInitialized = true;
        }
    }

    public class DecoratedTestService : ITestService
    {
        private readonly ITestService _inner;

        public DecoratedTestService(ITestService inner)
        {
            _inner = inner;
        }

        public string GetValue() => $"Decorated({_inner.GetValue()})";

        public void Initialize()
        {
            _inner.Initialize();
        }
    }

    // Event handling classes
    public class EventSource
    {
        public event Action? TestEvent;
        
        public void RaiseEvent()
        {
            TestEvent?.Invoke();
        }
    }

    public class EventListener
    {
        public int EventCallCount { get; private set; }

        public void OnTestEvent()
        {
            EventCallCount++;
        }
    }
    
    #endregion

    #region Tests
    
    [Fact]
    public void Can_Register_And_Resolve_Basic_Service()
    {
        // Arrange
        var provider = new TestServiceDefinitionProvider();
        
        provider.AddSingleton<ITestService, TestService>();
        
        var container = new MicrosoftServiceContainer(provider);
        container.Build();

        // Act
        var service = container.Get<ITestService>();

        // Assert
        Assert.NotNull(service);
        Assert.Equal("Original", service.GetValue());
    }

    [Fact]
    public void Can_Register_Instance()
    {
        // Arrange
        var provider = new TestServiceDefinitionProvider();
        var instance = new TestService();
        
        provider.AddSingleton<ITestService>(instance);
        
        var container = new MicrosoftServiceContainer(provider);
        container.Build();

        // Act
        var resolvedService = container.Get<ITestService>();

        // Assert
        Assert.Same(instance, resolvedService);
    }

    [Fact]
    public void Can_Decorate_Service()
    {
        // Arrange
        var provider = new TestServiceDefinitionProvider();
        
        // Register concrete implementation first
        provider.AddSingleton<TestService>();
        
        // Register interface with decorator
        provider.AddSingleton<ITestService>(sp => 
            new DecoratedTestService(sp.Get<TestService>()));
        
        var container = new MicrosoftServiceContainer(provider);
        container.Build();

        // Act
        var service = container.Get<ITestService>();

        // Assert
        Assert.NotNull(service);
        Assert.Equal("Decorated(Original)", service.GetValue());
    }

    [Fact]
    public void Executes_Post_Build_Actions()
    {
        // Arrange
        var provider = new TestServiceDefinitionProvider();
        
        provider.AddSingleton<TestService>();
        
        bool actionExecuted = false;
        provider.AddPostBuildAction(sp => {
            var service = sp.Get<TestService>();
            service.Initialize();
            actionExecuted = true;
        });
        
        var container = new MicrosoftServiceContainer(provider);

        // Act
        container.Build();
        var service = container.Get<TestService>();

        // Assert
        Assert.True(actionExecuted, "Post-build action was not executed");
        Assert.True(service.WasInitialized, "Service was not initialized");
    }

    [Fact]
    public void Can_Subscribe_To_Events_In_Post_Build()
    {
        // Arrange
        var provider = new TestServiceDefinitionProvider();
        
        provider.AddSingleton<EventSource>();
        provider.AddSingleton<EventListener>();
        
        provider.AddPostBuildAction(sp => {
            var source = sp.Get<EventSource>();
            var listener = sp.Get<EventListener>();
            source.TestEvent += listener.OnTestEvent;
        });
        
        var container = new MicrosoftServiceContainer(provider);

        // Act
        container.Build();
        var source = container.Get<EventSource>();
        var listener = container.Get<EventListener>();
        
        source.RaiseEvent();

        // Assert
        Assert.Equal(1, listener.EventCallCount);
    }

    [Fact]
    public void HasService_Returns_True_For_Registered_Service()
    {
        // Arrange
        var provider = new TestServiceDefinitionProvider();
        
        provider.AddSingleton<ITestService, TestService>();
        
        var container = new MicrosoftServiceContainer(provider);
        container.Build();

        // Act & Assert
        Assert.True(container.Has<ITestService>());
        Assert.False(container.Has<EventSource>());
    }

    [Fact]
    public void Build_Only_Executes_Once()
    {
        // Arrange
        var provider = new TestServiceDefinitionProvider();
        
        int buildActionCallCount = 0;
        provider.AddPostBuildAction(_ => buildActionCallCount++);
        
        var container = new MicrosoftServiceContainer(provider);

        // Act
        container.Build();
        container.Build(); // Call build twice
        
        // Assert
        Assert.Equal(1, buildActionCallCount);
    }
    
    [Fact]
    public void Get_Throws_For_Missing_Service()
    {
        // Arrange
        var provider = new TestServiceDefinitionProvider();
        
        var container = new MicrosoftServiceContainer(provider);
        container.Build();
        
        // Act & Assert
        Assert.Throws<ServiceNotFoundException>(() => container.Get<ITestService>());
    }
    
    #endregion
}

#region Test Service Definition Provider

public class TestServiceDefinitionProvider : IServiceDefinitionProvider
{
    private readonly List<ServiceDefinition> _definitions = new();
    private readonly List<PostBuildAction> _postBuildActions = new();
    
    public IEnumerable<ServiceDefinition> GetServiceDefinitions() => _definitions;
    public IEnumerable<PostBuildAction> GetPostBuildActions() => _postBuildActions;
    
    public TestServiceDefinitionProvider AddSingleton<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService
    {
        _definitions.Add(ServiceDefinition.FromType(typeof(TService), typeof(TImplementation)));
        return this;
    }
    
    public TestServiceDefinitionProvider AddSingleton<TService>(Func<IServiceContainer, TService> factory)
        where TService : class
    {
        _definitions.Add(ServiceDefinition.FromFactory(typeof(TService), sp => factory(sp)));
        return this;
    }
    
    public TestServiceDefinitionProvider AddSingleton<TService>(TService instance)
        where TService : class
    {
        _definitions.Add(ServiceDefinition.FromInstance(typeof(TService), instance));
        return this;
    }
    
    public TestServiceDefinitionProvider AddSingleton<TService>()
        where TService : class
    {
        return AddSingleton<TService, TService>();
    }
    
    public TestServiceDefinitionProvider AddPostBuildAction(Action<IServiceContainer> action)
    {
        _postBuildActions.Add(new PostBuildAction(action));
        return this;
    }
}

#endregion
