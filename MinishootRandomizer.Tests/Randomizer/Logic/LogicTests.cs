using Moq;

namespace MinishootRandomizer.Tests;

public class LogicTests
{
    private readonly ITestSuiteProvider _testSuiteProvider;
    private readonly IRegionLogicChecker _regionLogicChecker;
    private readonly ILocationLogicChecker _locationLogicChecker;

    public LogicTests()
    {
        // Initialize repositories
        IItemFactory itemFactory = new DictionaryItemFactory();
        IItemRepository itemRepository = new CsvItemRepository(
            itemFactory,
            "../../../../MinishootRandomizer/Resources/items.csv"
        );
        // For locations, we must mock the factory, as the original factory (wrongly) uses Unity's GameObject system.
        var locationFactoryStub = new Mock<ILocationFactory>();
        // We always create PickupLocations, as the location type should not matter for logic tests.
        locationFactoryStub.Setup(f => f.CreateLocation(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<LocationPool>()))
            .Returns((string id, string name, LocationPool pool) => new PickupLocation(
                id,
                name,
                pool,
                new ByNull() // Mocking selector for simplicity
            ));
        ILocationRepository locationRepository = new CsvLocationRepository(
            "../../../../MinishootRandomizer/Resources/locations.csv",
            locationFactoryStub.Object
        );
        IRegionRepository regionRepository = new CsvRegionRepository(
            "../../../../MinishootRandomizer/Resources/regions.csv"
        );
        ITransitionRepository transitionRepository = new CsvTransitionRepository(
            "../../../../MinishootRandomizer/Resources/transitions.csv"
        );

        // Initialize test suite provider
        _testSuiteProvider = new YamlTestSuiteProvider(
            "../../../Randomizer/Logic/logic_tests.yaml",
            itemRepository,
            locationRepository,
            regionRepository
        );

        // Initialize logic checker
        ILogicParser logicParser = new CoreLogicParser(
            itemRepository,
            regionRepository
        );
        _regionLogicChecker = new CoreRegionLogicChecker(
            regionRepository,
            transitionRepository,
            logicParser
        );
        _locationLogicChecker = new CoreLocationLogicChecker(
            logicParser,
            _regionLogicChecker,
            regionRepository,
            locationRepository
        );
    }

    [Fact]
    public void AllLogicTestsShouldPass()
    {
        LogicTestSuite testSuite = _testSuiteProvider.GetLogicTestSuite();
        foreach (var testDataWithLogicState in testSuite.GetTestDataWithLogicState())
        {
            LogicTestData testData = testDataWithLogicState.Item1;
            LogicState logicState = testDataWithLogicState.Item2;

            foreach (var assertion in testData.Assertions)
            {
                if (assertion is LocationAccessibilityAssertion locationAssertion)
                {
                    var location = locationAssertion.Location;
                    var expectedAccessibility = locationAssertion.ExpectedAccessibility;
                    var actualAccessibility = _locationLogicChecker.CheckLocationLogic(logicState, location);
                    Assert.True(
                        expectedAccessibility == actualAccessibility,
                        $"Location '{location.Identifier}' for test '{testData.Name}' failed accessibility check : Expected {expectedAccessibility}, but got {actualAccessibility}."
                    );
                }
                else if (assertion is RegionAccessibilityAssertion regionAssertion)
                {
                    var region = regionAssertion.Region;
                    var expectedAccessibility = regionAssertion.ExpectedAccessibility;
                    var actualAccessibility = _regionLogicChecker.GetRegionAccessibility(region, logicState);
                    Assert.True(
                        expectedAccessibility == actualAccessibility,
                        $"Region '{region.Name}' for test '{testData.Name}' failed accessibility check : Expected {expectedAccessibility}, but got {actualAccessibility}."
                    );
                }
                else
                {
                    throw new NotImplementedException($"Assertion type {assertion.GetType()} is not implemented.");
                }
            }
        }
    }
}
