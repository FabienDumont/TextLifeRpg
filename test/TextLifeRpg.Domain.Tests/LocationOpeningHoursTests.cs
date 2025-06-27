using TextLifeRpg.Domain;

namespace TextLifeRpg.Domain.Tests;

public class LocationOpeningHoursTests
{
  #region Methods

  [Fact]
  public void Create_ShouldInitializeWithGeneratedId()
  {
    // Arrange
    var locationId = Guid.NewGuid();
    var dayOfWeek = DayOfWeek.Friday;
    var opensAt = new TimeSpan(18, 0, 0);
    var closesAt = new TimeSpan(23, 59, 0);

    // Act
    var openingHours = LocationOpeningHours.Create(locationId, dayOfWeek, opensAt, closesAt);

    // Assert
    Assert.NotNull(openingHours);
    Assert.NotEqual(Guid.Empty, openingHours.Id);
    Assert.Equal(locationId, openingHours.LocationId);
    Assert.Equal(dayOfWeek, openingHours.DayOfWeek);
    Assert.Equal(opensAt, openingHours.OpensAt);
    Assert.Equal(closesAt, openingHours.ClosesAt);
  }

  [Fact]
  public void Load_ShouldInitializeWithGivenValues()
  {
    // Arrange
    var id = Guid.NewGuid();
    var locationId = Guid.NewGuid();
    var dayOfWeek = DayOfWeek.Saturday;
    var opensAt = new TimeSpan(22, 0, 0);
    var closesAt = new TimeSpan(2, 0, 0);

    // Act
    var openingHours = LocationOpeningHours.Load(id, locationId, dayOfWeek, opensAt, closesAt);

    // Assert
    Assert.NotNull(openingHours);
    Assert.Equal(id, openingHours.Id);
    Assert.Equal(locationId, openingHours.LocationId);
    Assert.Equal(dayOfWeek, openingHours.DayOfWeek);
    Assert.Equal(opensAt, openingHours.OpensAt);
    Assert.Equal(closesAt, openingHours.ClosesAt);
  }

  #endregion
}
