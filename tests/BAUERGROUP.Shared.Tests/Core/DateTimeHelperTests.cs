using BAUERGROUP.Shared.Core.Extensions;

namespace BAUERGROUP.Shared.Tests.Core;

public class DateTimeHelperTests
{
    [Fact]
    public void ToUnixTimestamp_ConvertsCorrectly()
    {
        var dateTime = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var result = dateTime.ToUnixTimestamp(true);

        // Unix timestamp for 2000-01-01 00:00:00 UTC is 946684800
        result.Should().Be(946684800);
    }

    [Fact]
    public void FromUnixTimestamp_ConvertsCorrectly()
    {
        var dateTime = new DateTime();
        var result = dateTime.FromUnixTimestamp(946684800, true);

        result.Year.Should().Be(2000);
        result.Month.Should().Be(1);
        result.Day.Should().Be(1);
    }

    [Fact]
    public void ToUnixTimestamp_AndBack_RoundTrips()
    {
        var original = new DateTime(2024, 6, 15, 12, 30, 0, DateTimeKind.Utc);
        var timestamp = original.ToUnixTimestamp(true);
        var restored = new DateTime().FromUnixTimestamp(timestamp, true);

        restored.Should().BeCloseTo(original, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void ToUnixTimestamp_EpochDate_ReturnsZero()
    {
        var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var result = epoch.ToUnixTimestamp(true);

        result.Should().Be(0);
    }
}
