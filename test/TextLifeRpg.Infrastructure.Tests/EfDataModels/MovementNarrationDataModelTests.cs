using TextLifeRpg.Infrastructure.EfDataModels;

namespace TextLifeRpg.Infrastructure.Tests.EfDataModels;

public class MovementNarrationDataModelTests
{
  #region Methods

  [Fact]
  public void Instantiation_ShouldInitializeWithGivenValues()
  {
    // Arrange
    var id = Guid.NewGuid();
    var movementId = Guid.NewGuid();
    var text = "You walk into the street.";

    var movement = new MovementDataModel
    {
      Id = movementId,
      FromLocationId = Guid.NewGuid(),
      FromRoomId = Guid.NewGuid(),
      ToLocationId = Guid.NewGuid(),
      ToRoomId = Guid.NewGuid(),
      RequiredItemId = Guid.NewGuid()
    };

    // Act
    var narration = new MovementNarrationDataModel
    {
      Id = id,
      MovementId = movementId,
      Text = text,
      Movement = movement
    };

    // Assert
    Assert.Equal(id, narration.Id);
    Assert.Equal(movementId, narration.MovementId);
    Assert.Equal(text, narration.Text);
    Assert.Equal(movement, narration.Movement);
  }

  #endregion
}
