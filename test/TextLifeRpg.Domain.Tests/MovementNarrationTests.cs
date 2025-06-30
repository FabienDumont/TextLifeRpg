namespace TextLifeRpg.Domain.Tests;

public class MovementNarrationNarrationTests
{
  #region Methods

  [Fact]
  public void Create_ShouldInitializeWithGeneratedId()
  {
    // Arrange
    var movementId = Guid.NewGuid();
    var text = string.Empty;

    // Act
    var movement = MovementNarration.Create(movementId, text);

    // Assert
    Assert.NotNull(movement);
    Assert.NotEqual(Guid.Empty, movement.Id);
    Assert.Equal(movementId, movement.MovementId);
    Assert.Equal(text, movement.Text);
  }

  [Fact]
  public void Load_ShouldInitializeWithGivenValues()
  {
    // Arrange
    var id = Guid.NewGuid();
    var movementId = Guid.NewGuid();
    var text = string.Empty;

    // Act
    var movement = MovementNarration.Load(id, movementId, text);

    // Assert
    Assert.NotNull(movement);
    Assert.Equal(id, movement.Id);
    Assert.Equal(movementId, movement.MovementId);
    Assert.Equal(text, movement.Text);
  }

  #endregion
}
