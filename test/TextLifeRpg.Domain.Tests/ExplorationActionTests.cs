using TextLifeRpg.Domain;

namespace TextLifeRpg.Domain.Tests;

public class ExplorationActionTests
{
  #region Methods

  [Fact]
  public void Create_ShouldInitializeWithGeneratedId()
  {
    // Arrange
    var locationId = Guid.NewGuid();
    var roomId = Guid.NewGuid();
    const string label = "Sleep";

    // Act
    var action = ExplorationAction.Create(locationId, roomId, label, 480);

    // Assert
    Assert.NotNull(action);
    Assert.NotEqual(Guid.Empty, action.Id);
    Assert.Equal(locationId, action.LocationId);
    Assert.Equal(roomId, action.RoomId);
    Assert.Equal(label, action.Label);
  }

  [Fact]
  public void Load_ShouldInitializeWithGivenValues()
  {
    // Arrange
    var id = Guid.NewGuid();
    var locationId = Guid.NewGuid();
    var roomId = Guid.NewGuid();
    const string label = "Sleep";
    const int neededMinutes = 480;

    // Act
    var action = ExplorationAction.Load(id, locationId, roomId, label, neededMinutes);

    // Assert
    Assert.NotNull(action);
    Assert.Equal(id, action.Id);
    Assert.Equal(locationId, action.LocationId);
    Assert.Equal(roomId, action.RoomId);
    Assert.Equal(label, action.Label);
    Assert.Equal(neededMinutes, action.NeededMinutes);
  }

  #endregion
}
