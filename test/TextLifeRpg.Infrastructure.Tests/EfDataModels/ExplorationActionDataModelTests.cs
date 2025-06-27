using TextLifeRpg.Infrastructure.EfDataModels;

namespace TextLifeRpg.Infrastructure.Tests.EfDataModels;

public class ExplorationActionDataModelTests
{
  #region Methods

  [Fact]
  public void Instantiation_ShouldInitializeWithGivenValues()
  {
    // Arrange
    var actionId = Guid.NewGuid();
    var locationId = Guid.NewGuid();
    var roomId = Guid.NewGuid();
    const string label = "Sleep";
    const int neededMinutes = 480;

    var location = new LocationDataModel
    {
      Id = locationId,
      Name = "Home"
    };

    var room = new RoomDataModel
    {
      Id = roomId,
      Name = "Bedroom"
    };

    // Act
    var action = new ExplorationActionDataModel
    {
      Id = actionId,
      LocationId = locationId,
      RoomId = roomId,
      Label = label,
      Location = location,
      Room = room,
      NeededMinutes = neededMinutes
    };

    // Assert
    Assert.Equal(actionId, action.Id);
    Assert.Equal(locationId, action.LocationId);
    Assert.Equal(roomId, action.RoomId);
    Assert.Equal(label, action.Label);
    Assert.Equal(location, action.Location);
    Assert.Equal(room, action.Room);
    Assert.Equal(neededMinutes, action.NeededMinutes);
  }

  [Fact]
  public void Instantiation_ShouldAllowNullRoomId()
  {
    // Arrange
    var action = new ExplorationActionDataModel
    {
      Id = Guid.NewGuid(),
      LocationId = Guid.NewGuid(),
      RoomId = null,
      Label = "Sleep",
      NeededMinutes = 480,
      Location = null
    };

    // Act & Assert
    Assert.Null(action.RoomId);
    Assert.Null(action.Location);
  }

  #endregion
}
