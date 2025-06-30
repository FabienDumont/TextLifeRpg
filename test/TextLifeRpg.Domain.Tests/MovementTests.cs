namespace TextLifeRpg.Domain.Tests;

public class MovementTests
{
  #region Methods

  [Fact]
  public void Create_ShouldInitializeWithGeneratedId()
  {
    // Arrange
    var fromLocationId = Guid.NewGuid();
    var fromRoomId = Guid.NewGuid();
    var toLocationId = Guid.NewGuid();
    var toRoomId = Guid.NewGuid();
    var requiredItemId = Guid.NewGuid();

    // Act
    var movement = Movement.Create(fromLocationId, fromRoomId, toLocationId, toRoomId, requiredItemId);

    // Assert
    Assert.NotNull(movement);
    Assert.NotEqual(Guid.Empty, movement.Id);
    Assert.Equal(fromLocationId, movement.FromLocationId);
    Assert.Equal(fromRoomId, movement.FromRoomId);
    Assert.Equal(toLocationId, movement.ToLocationId);
    Assert.Equal(toRoomId, movement.ToRoomId);
    Assert.Equal(requiredItemId, movement.RequiredItemId);
  }

  [Fact]
  public void Load_ShouldInitializeWithGivenValues()
  {
    // Arrange
    var id = Guid.NewGuid();
    var fromLocationId = Guid.NewGuid();
    var fromRoomId = Guid.NewGuid();
    var toLocationId = Guid.NewGuid();
    var toRoomId = Guid.NewGuid();
    var requiredItemId = Guid.NewGuid();

    // Act
    var movement = Movement.Load(id, fromLocationId, fromRoomId, toLocationId, toRoomId, requiredItemId);

    // Assert
    Assert.NotNull(movement);
    Assert.Equal(id, movement.Id);
    Assert.Equal(fromLocationId, movement.FromLocationId);
    Assert.Equal(fromRoomId, movement.FromRoomId);
    Assert.Equal(toLocationId, movement.ToLocationId);
    Assert.Equal(toRoomId, movement.ToRoomId);
    Assert.Equal(requiredItemId, movement.RequiredItemId);
  }

  #endregion
}
