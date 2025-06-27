using TextLifeRpg.Infrastructure.EfDataModels;

namespace TextLifeRpg.Infrastructure.Tests.EfDataModels;

public class LocationOpeningHoursDataModelTests
{
  #region Methods

  [Fact]
  public void Instantiation_ShouldInitializeWithGivenValues()
  {
    // Arrange
    var roomId = Guid.NewGuid();
    var locationId = Guid.NewGuid();
    var opensAt = new TimeSpan(8, 0, 0);
    var closesAt = new TimeSpan(10, 0, 0);

    var location = new LocationDataModel
    {
      Id = locationId,
      Name = "Home"
    };

    // Act
    var openingHours = new LocationOpeningHoursDataModel
    {
      Id = roomId,
      LocationId = locationId,
      DayOfWeek = DayOfWeek.Monday,
      OpensAt = opensAt,
      ClosesAt = closesAt,
      Location = location
    };

    // Assert
    Assert.Equal(roomId, openingHours.Id);
    Assert.Equal(locationId, openingHours.LocationId);
    Assert.Equal(opensAt, openingHours.OpensAt);
    Assert.Equal(closesAt, openingHours.ClosesAt);
    Assert.Equal(location, openingHours.Location);
  }

  #endregion
}
