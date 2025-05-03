namespace MinishootRandomizer.Tests;

public class VersionNumberTest
{
    [Fact]
    public void Constructor_WithComponents_ShouldCreateCorrectVersion()
    {
        // Arrange & Act
        var version1 = new VersionNumber(1, 2, 3);
        var version2 = new VersionNumber(2, 0, 5, "beta");

        // Assert
        Assert.Equal(1, version1.Major);
        Assert.Equal(2, version1.Minor);
        Assert.Equal(3, version1.Patch);
        Assert.Equal("", version1.Label);

        Assert.Equal(2, version2.Major);
        Assert.Equal(0, version2.Minor);
        Assert.Equal(5, version2.Patch);
        Assert.Equal("beta", version2.Label);
    }

    [Theory]
    [InlineData("1.2.3", 1, 2, 3, "")]
    [InlineData("1.2.3-alpha", 1, 2, 3, "alpha")]
    [InlineData("1.2", 1, 2, 0, "")]
    [InlineData("1", 1, 0, 0, "")]
    [InlineData("5.0.1-rc1", 5, 0, 1, "rc1")]
    public void Constructor_FromString_ShouldParseCorrectly(string versionString, int expectedMajor, int expectedMinor, int expectedPatch, string expectedLabel)
    {
        // Arrange & Act
        var version = new VersionNumber(versionString);

        // Assert
        Assert.Equal(expectedMajor, version.Major);
        Assert.Equal(expectedMinor, version.Minor);
        Assert.Equal(expectedPatch, version.Patch);
        Assert.Equal(expectedLabel, version.Label);
    }

    [Theory]
    [InlineData("1.2.3", "1.2.3", true)]
    [InlineData("1.2.3", "1.2.4", false)]
    [InlineData("2.0.0", "2.0", true)]
    [InlineData("1.2.3-beta", "1.2.3", true)] // Label is not considered in exact matching
    public void Satisfy_ExactVersion_ShouldMatchCorrectly(string versionString, string constraint, bool expectedResult)
    {
        // Arrange
        var version = new VersionNumber(versionString);

        // Act
        var result = version.Satisfy(constraint);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData("1.2.3", "1.2.*", true)]
    [InlineData("1.2.0", "1.2.*", true)]
    [InlineData("1.3.0", "1.2.*", false)]
    [InlineData("2.1.0", "2.*", true)]
    [InlineData("2.5.3", "2.*.*", true)]
    [InlineData("1.2.3", "1.*.3", true)]
    [InlineData("1.5.3", "1.*.3", true)]
    public void Satisfy_Wildcard_ShouldMatchCorrectly(string versionString, string constraint, bool expectedResult)
    {
        // Arrange
        var version = new VersionNumber(versionString);

        // Act
        var result = version.Satisfy(constraint);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData("1.2.0", "~1", true)]
    [InlineData("2.0.0", "~1", true)]
    [InlineData("0.9.0", "~1", false)]
    [InlineData("1.2.3", "~1.2", true)]
    [InlineData("1.3.0", "~1.2", true)]
    [InlineData("1.1.9", "~1.2", false)]
    [InlineData("2.0.0", "~1.2", false)]
    [InlineData("1.2.3", "~1.2.3", true)]
    [InlineData("1.2.4", "~1.2.3", true)]
    [InlineData("1.2.2", "~1.2.3", false)]
    public void Satisfy_TildeOperator_ShouldMatchCorrectly(string versionString, string constraint, bool expectedResult)
    {
        // Arrange
        var version = new VersionNumber(versionString);

        // Act
        var result = version.Satisfy(constraint);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void Satisfy_InvalidConstraint_ShouldThrowArgumentException()
    {
        // Arrange
        var version = new VersionNumber("1.2.3");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => version.Satisfy(""));
        Assert.Throws<ArgumentException>(() => version.Satisfy("~"));
        Assert.Throws<ArgumentException>(() => version.Satisfy("~1.2.3.4"));
    }

    [Theory]
    [InlineData(1, 2, 3, "", "1.2.3")]
    [InlineData(1, 0, 0, "", "1.0.0")]
    [InlineData(2, 1, 5, "alpha", "2.1.5-alpha")]
    [InlineData(3, 0, 1, "beta2", "3.0.1-beta2")]
    public void ToString_ShouldReturnCorrectStringRepresentation(int major, int minor, int patch, string label, string expected)
    {
        // Arrange
        var version = new VersionNumber(major, minor, patch, label);

        // Act
        var result = version.ToString();

        // Assert
        Assert.Equal(expected, result);
    }
}
