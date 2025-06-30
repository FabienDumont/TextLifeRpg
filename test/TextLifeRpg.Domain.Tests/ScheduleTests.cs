namespace TextLifeRpg.Domain.Tests;

public class ScheduleTests
{
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
}
