using TextLifeRpg.Domain;

namespace TextLifeRpg.Domain.Tests;

public class RoomTests
{
  #region Methods

  [Fact]
  public void Create_ShouldInitialize()
  {
    // Act
    var room = Room.Create(Guid.Empty, string.Empty, true);

    // Assert
    Assert.NotNull(room);
    Assert.NotEqual(Guid.Empty, room.Id);
    Assert.Equal(Guid.Empty, room.LocationId);
    Assert.Equal(string.Empty, room.Name);
    Assert.True(room.IsPlayerSpawn);
  }

  [Fact]
  public void Load_ShouldInitializeWithGivenValues()
  {
    // Arrange
    var id = Guid.NewGuid();
    var locationId = Guid.NewGuid();
    const string name = "Home";

    // Act
    var room = Room.Load(id, locationId, name, true);

    // Assert
    Assert.NotNull(room);
    Assert.Equal(id, room.Id);
    Assert.Equal(locationId, room.LocationId);
    Assert.Equal(name, room.Name);
    Assert.True(room.IsPlayerSpawn);
  }

  #endregion
}
