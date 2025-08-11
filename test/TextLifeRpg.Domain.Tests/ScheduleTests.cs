namespace TextLifeRpg.Domain.Tests;

public class ScheduleTests
{
  #region MyRegion

  [Fact]
  public void Create_ShouldSortEntriesByStartHour()
  {
    // Arrange
    var characterId = Guid.NewGuid();
    var locationEntry1 = Guid.NewGuid();
    var locationEntry2 = Guid.NewGuid();
    var entries = new List<ScheduleEntry>
    {
      new(DayOfWeek.Monday, new TimeSpan(8, 0, 0), new TimeSpan(9, 0, 0), locationEntry1, null),
      new(DayOfWeek.Tuesday, new TimeSpan(14, 0, 0), new TimeSpan(15, 0, 0), locationEntry2, null)
    };

    // Act
    var schedule = Schedule.Create(characterId, entries);

    // Assert
    Assert.Equal(characterId, schedule.CharacterId);
    Assert.Collection(
      schedule.Entries, e => Assert.Equal(locationEntry1, e.LocationId), e => Assert.Equal(locationEntry2, e.LocationId)
    );
  }

  [Theory]
  [InlineData("08:30", "08:00")] // during first entry
  [InlineData("09:00", "08:00")] // right at end of first entry
  [InlineData("10:00", "08:00")] // after first entry
  [InlineData("07:59", null)] // before any entries
  public void GetCurrentEntry_ShouldReturnCorrectEntryOrNull(string timeString, string? expectedStart)
  {
    // Arrange
    var characterId = Guid.NewGuid();
    var locationId = Guid.NewGuid();

    var entries = new List<ScheduleEntry>
    {
      new(DayOfWeek.Monday, new TimeSpan(8, 0, 0), new TimeSpan(9, 0, 0), locationId, null),
      new(DayOfWeek.Monday, new TimeSpan(14, 0, 0), new TimeSpan(15, 0, 0), locationId, null)
    };

    var schedule = Schedule.Create(characterId, entries);
    var currentTime = TimeSpan.Parse(timeString);

    // Act
    var result = schedule.GetCurrentEntry(DayOfWeek.Monday, currentTime);

    // Assert
    if (expectedStart is null)
    {
      Assert.Null(result);
    }
    else
    {
      Assert.NotNull(result);
      Assert.Equal(TimeSpan.Parse(expectedStart), result.StartHour);
    }
  }

  #endregion
}
