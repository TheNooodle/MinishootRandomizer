using Moq;

namespace MinishootRandomizer.Tests;

public class YamlTestSuiteProviderTest
{
    [Fact]
    public void GetLogicTestSuite_ShouldReturnCorrectLogicTestSuite()
    {
        // Arrange
        var yamlFilePath = "../../../Randomizer/Logic/TestSuite/sample.yaml";
        var itemRepositoryStub = new Mock<IItemRepository>();
        itemRepositoryStub.Setup(repo => repo.Get("Super Crystals x5"))
            .Returns((string id) => new SuperCrystalsItem(id, ItemCategory.Filler, 5));
        itemRepositoryStub.Setup(repo => repo.Get("Progressive Dash"))
            .Returns((string id) => new ProgressiveDashItem(id, ItemCategory.Progression));
        var locationRepositoryStub = new Mock<ILocationRepository>();
        locationRepositoryStub.Setup(repo => repo.Get(It.IsAny<string>()))
            .Returns((string id) => new PickupLocation(id, "true", LocationPool.Default, new Mock<ISelector>().Object));
        var regionRepositoryStub = new Mock<IRegionRepository>();
        regionRepositoryStub.Setup(repo => repo.Get(It.IsAny<string>()))
            .Returns((string name) => new Region(name));
        var yamlTestSuiteProvider = new YamlTestSuiteProvider(
            yamlFilePath,
            itemRepositoryStub.Object,
            locationRepositoryStub.Object,
            regionRepositoryStub.Object
        );

        // Act
        LogicTestSuite logicTestSuite = yamlTestSuiteProvider.GetLogicTestSuite();

        // Assert
        Assert.NotNull(logicTestSuite);
        Assert.Equal(14, logicTestSuite.DefaultSettings.Count);
        var npcSanitySetting = logicTestSuite.DefaultSettings[0];
        Assert.IsType<NpcSanity>(npcSanitySetting);
        Assert.True(((NpcSanity)npcSanitySetting).Enabled);

        Assert.Single(logicTestSuite.DefaultItemCounts);
        var superCrystalItem = logicTestSuite.DefaultItemCounts.Keys.First();
        Assert.IsType<SuperCrystalsItem>(superCrystalItem);
        Assert.Equal("Super Crystals x5", superCrystalItem.Identifier);
        Assert.Equal(5, ((SuperCrystalsItem)superCrystalItem).CrystalsAmount);
        Assert.Equal(2, logicTestSuite.DefaultItemCounts[superCrystalItem]);

        Assert.Equal(2, logicTestSuite.TestData.Count);
        Assert.Equal("Test Dash", logicTestSuite.TestData[0].Name);
        Assert.Empty(logicTestSuite.TestData[0].OverrideSettings);
        Assert.Single(logicTestSuite.TestData[0].AddItemCounts);
        Assert.Equal(3, logicTestSuite.TestData[0].Assertions.Count);

        Assert.IsType<LocationAccessibilityAssertion>(logicTestSuite.TestData[0].Assertions[0]);
        var locationAssertion = (LocationAccessibilityAssertion)logicTestSuite.TestData[0].Assertions[0];
        Assert.Equal("Forest Grotto - After ramp", locationAssertion.Location.Identifier);
        Assert.Equal(LogicAccessibility.InLogic, locationAssertion.ExpectedAccessibility);
        var locationAssertion2 = (LocationAccessibilityAssertion)logicTestSuite.TestData[0].Assertions[1];
        Assert.Equal("Green - Grove near button", locationAssertion2.Location.Identifier);
        Assert.Equal(LogicAccessibility.OutOfLogic, locationAssertion2.ExpectedAccessibility);

        Assert.IsType<RegionAccessibilityAssertion>(logicTestSuite.TestData[0].Assertions[2]);
        var regionAssertion = (RegionAccessibilityAssertion)logicTestSuite.TestData[0].Assertions[2];
        Assert.Equal("Family House Cave - After Jumps", regionAssertion.Region.Name);
        Assert.Equal(LogicAccessibility.OutOfLogic, regionAssertion.ExpectedAccessibility);

        List<Tuple<LogicTestData, LogicState>> testDataWithLogicState = logicTestSuite.GetTestDataWithLogicState();
        Assert.Equal(2, testDataWithLogicState.Count);
        Assert.Equal("Test Dash with Boostless ramps", testDataWithLogicState[1].Item1.Name);
        Assert.False(testDataWithLogicState[1].Item2.GetSetting<BoostlessSpiritRaces>().Enabled);
        Assert.True(testDataWithLogicState[1].Item2.GetSetting<BoostlessSpringboards>().Enabled);
        Assert.True(testDataWithLogicState[1].Item2.HasItem(superCrystalItem, 2));
        Assert.True(testDataWithLogicState[1].Item2.HasItem(new ProgressiveDashItem("Progressive Dash", ItemCategory.Progression), 1));
        Assert.Equal(3, testDataWithLogicState[1].Item1.Assertions.Count);
        Assert.IsType<LocationAccessibilityAssertion>(testDataWithLogicState[1].Item1.Assertions[0]);
        var locationAssertion3 = (LocationAccessibilityAssertion)testDataWithLogicState[1].Item1.Assertions[0];
        Assert.Equal("Forest Grotto - After ramp", locationAssertion3.Location.Identifier);
        Assert.Equal(LogicAccessibility.InLogic, locationAssertion3.ExpectedAccessibility);
        var regionAssertion2 = (RegionAccessibilityAssertion)testDataWithLogicState[1].Item1.Assertions[2];
        Assert.Equal("Family House Cave - After Jumps", regionAssertion2.Region.Name);
        Assert.Equal(LogicAccessibility.InLogic, regionAssertion2.ExpectedAccessibility);
    }
}
