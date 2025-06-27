using TextLifeRpg.Infrastructure.EfDataModels;

namespace TextLifeRpg.Infrastructure.Tests.EfDataModels;

public class RoomDataModelTests
{
  #region Methods

  [Fact]
  public void Instantiation_ShouldInitializeWithGivenValues()
  {
    // Arrange
    var roomId = Guid.NewGuid();
    var locationId = Guid.NewGuid();
    const string roomName = "Bedroom";

    var location = new LocationDataModel
    {
      Id = locationId,
      Name = "Home"
    };

    // Act
    var room = new RoomDataModel
    {
      Id = roomId,
      LocationId = locationId,
      Name = roomName,
      IsPlayerSpawn = false,
      Location = location
    };

    // Assert
    Assert.Equal(roomId, room.Id);
    Assert.Equal(roomName, room.Name);
    Assert.Equal(locationId, room.LocationId);
    Assert.False(room.IsPlayerSpawn);
    Assert.Equal(location, room.Location);
  }

  #endregion
}
