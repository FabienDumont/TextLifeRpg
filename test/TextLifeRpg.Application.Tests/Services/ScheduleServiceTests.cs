using TextLifeRpg.Application.Abstraction;
using TextLifeRpg.Application.Services;
using TextLifeRpg.Domain;
using TextLifeRpg.Domain.Tests.Helpers;

namespace TextLifeRpg.Application.Tests.Services;

public class ScheduleServiceTests
{
  #region Fields

  private readonly ILocationService _locationService = A.Fake<ILocationService>();
  private readonly IRandomProvider _randomProvider = A.Fake<IRandomProvider>();
  private readonly ScheduleService _service;

  #endregion

  #region Ctors

  public ScheduleServiceTests()
  {
    _service = new ScheduleService(_locationService, _randomProvider);
  }

  #endregion

  #region Methods

  [Fact]
  public async Task GenerateSchedulesAsync_ShouldThrow_WhenStreetLocationIsNull()
  {
    // Arrange
    A.CallTo(() => _locationService.GetByNameAsync("Street", A<CancellationToken>._))
      .Returns(Task.FromResult<Location?>(null));

    // Act & Assert
    await Assert.ThrowsAsync<InvalidOperationException>(() =>
      _service.GenerateSchedulesAsync([], DateOnly.FromDateTime(DateTime.Today), CancellationToken.None)
    );
  }

  [Fact]
  public async Task GenerateSchedulesAsync_ShouldGenerateOneEntryPerCharacterPerDay_WhenTimeSlotsExist()
  {
    // Arrange
    var locationId = Guid.NewGuid();
    var streetLocation = Location.Load(
      locationId, "Street", [
        LocationOpeningHours.Create(locationId, DayOfWeek.Monday, new TimeSpan(8, 0, 0), new TimeSpan(9, 0, 0)),
        LocationOpeningHours.Create(locationId, DayOfWeek.Tuesday, new TimeSpan(8, 0, 0), new TimeSpan(9, 0, 0)),
        LocationOpeningHours.Create(locationId, DayOfWeek.Wednesday, new TimeSpan(8, 0, 0), new TimeSpan(9, 0, 0)),
        LocationOpeningHours.Create(locationId, DayOfWeek.Thursday, new TimeSpan(8, 0, 0), new TimeSpan(9, 0, 0)),
        LocationOpeningHours.Create(locationId, DayOfWeek.Friday, new TimeSpan(8, 0, 0), new TimeSpan(9, 0, 0)),
        LocationOpeningHours.Create(locationId, DayOfWeek.Saturday, new TimeSpan(8, 0, 0), new TimeSpan(9, 0, 0)),
        LocationOpeningHours.Create(locationId, DayOfWeek.Sunday, new TimeSpan(8, 0, 0), new TimeSpan(9, 0, 0))
      ]
    );
    A.CallTo(() => _locationService.GetByNameAsync("Street", A<CancellationToken>._))
      .Returns(Task.FromResult<Location?>(streetLocation));

    A.CallTo(() => _randomProvider.NextDouble()).Returns(0.5);

    var character = new CharacterBuilder().Build();

    // Act
    var result = await _service.GenerateSchedulesAsync(
      [character], DateOnly.FromDateTime(DateTime.Today), CancellationToken.None
    );

    // Assert
    Assert.Single(result);
    var schedule = result[0];
    Assert.Equal(character.Id, schedule.CharacterId);
    Assert.Equal(7, schedule.Entries.Count);
    Assert.All(
      schedule.Entries, entry =>
      {
        Assert.Equal(locationId, entry.LocationId);
        Assert.Equal(new TimeSpan(8, 0, 0), entry.StartHour);
        Assert.Equal(new TimeSpan(9, 0, 0), entry.EndHour);
      }
    );
  }

  [Fact]
  public async Task GenerateSchedulesAsync_ShouldSkipDays_WhenNoTimeSlotsReturned()
  {
    // Arrange
    var locationId = Guid.NewGuid();
    var streetLocation = Location.Load(
      locationId, "Street", [
        LocationOpeningHours.Create(locationId, DayOfWeek.Monday, new TimeSpan(8, 0, 0), new TimeSpan(9, 0, 0)),
        LocationOpeningHours.Create(locationId, DayOfWeek.Tuesday, new TimeSpan(8, 0, 0), new TimeSpan(9, 0, 0)),
        LocationOpeningHours.Create(locationId, DayOfWeek.Wednesday, new TimeSpan(8, 0, 0), new TimeSpan(9, 0, 0)),
        LocationOpeningHours.Create(locationId, DayOfWeek.Thursday, new TimeSpan(8, 0, 0), new TimeSpan(9, 0, 0)),
        LocationOpeningHours.Create(locationId, DayOfWeek.Friday, new TimeSpan(8, 0, 0), new TimeSpan(9, 0, 0))
      ]
    );
    A.CallTo(() => _locationService.GetByNameAsync("Street", A<CancellationToken>._))
      .Returns(Task.FromResult<Location?>(streetLocation));

    var slotlessDays = new HashSet<DayOfWeek> {DayOfWeek.Saturday, DayOfWeek.Sunday};

    var character = new CharacterBuilder().Build();

    // Act
    var result = await _service.GenerateSchedulesAsync(
      [character], DateOnly.FromDateTime(DateTime.Today), CancellationToken.None
    );

    // Assert
    Assert.Single(result);
    var entries = result[0].Entries;
    Assert.Equal(5, entries.Count); // 7 days – 2 skipped
    Assert.DoesNotContain(entries, e => slotlessDays.Contains(e.Day));
  }

  #endregion
}
