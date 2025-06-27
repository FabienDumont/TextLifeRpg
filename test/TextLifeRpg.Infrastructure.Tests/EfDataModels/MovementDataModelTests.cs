using TextLifeRpg.Infrastructure.EfDataModels;

namespace TextLifeRpg.Infrastructure.Tests.EfDataModels;

public class MovementDataModelTests
{
  #region Methods

  [Fact]
  public void Instantiation_ShouldInitializeWithGivenValues()
  {
    // Arrange
    var id = Guid.NewGuid();
    var fromLocationId = Guid.NewGuid();
    var fromRoomId = Guid.NewGuid();
    var toLocationId = Guid.NewGuid();
    var toRoomId = Guid.NewGuid();
    var requiredItemId = Guid.NewGuid();

    var fromLocation = new LocationDataModel
    {
      Id = fromLocationId,
      Name = string.Empty
    };

    var fromRoom = new RoomDataModel
    {
      Id = fromRoomId,
      Name = string.Empty
    };

    var toLocation = new LocationDataModel
    {
      Id = toLocationId,
      Name = string.Empty
    };

    var toRoom = new RoomDataModel
    {
      Id = toRoomId,
      Name = string.Empty
    };

    // Act
    var movement = new MovementDataModel
    {
      Id = id,
      FromLocationId = fromLocationId,
      FromRoomId = fromRoomId,
      ToLocationId = toLocationId,
      ToRoomId = toRoomId,
      RequiredItemId = requiredItemId,
      FromLocation = fromLocation,
      FromRoom = fromRoom,
      ToLocation = toLocation,
      ToRoom = toRoom
    };

    // Assert
    Assert.Equal(id, movement.Id);
    Assert.Equal(fromLocationId, movement.FromLocationId);
    Assert.Equal(fromRoomId, movement.FromRoomId);
    Assert.Equal(toLocationId, movement.ToLocationId);
    Assert.Equal(toRoomId, movement.ToRoomId);
    Assert.Equal(requiredItemId, movement.RequiredItemId);
    Assert.Equal(fromLocation, movement.FromLocation);
    Assert.Equal(fromRoom, movement.FromRoom);
    Assert.Equal(toLocation, movement.ToLocation);
    Assert.Equal(toRoom, movement.ToRoom);
  }

  #endregion
}
