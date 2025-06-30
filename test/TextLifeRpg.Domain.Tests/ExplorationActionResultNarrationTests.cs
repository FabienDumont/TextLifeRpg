namespace TextLifeRpg.Domain.Tests;

public class ExplorationActionResultNarrationTests
{
  #region Methods

  [Fact]
  public void Create_ShouldInitializeWithGeneratedId()
  {
    // Arrange
    var resultId = Guid.NewGuid();
    const string text = "You lie down and close your eyes.";

    // Act
    var narration = ExplorationActionResultNarration.Create(resultId, text);

    // Assert
    Assert.NotNull(narration);
    Assert.NotEqual(Guid.Empty, narration.Id);
    Assert.Equal(resultId, narration.ExplorationActionResultId);
    Assert.Equal(text, narration.Text);
  }

  [Fact]
  public void Load_ShouldInitializeWithGivenValues()
  {
    // Arrange
    var id = Guid.NewGuid();
    var resultId = Guid.NewGuid();
    const string text = "You collapse onto the bed.";

    // Act
    var narration = ExplorationActionResultNarration.Load(id, resultId, text);

    // Assert
    Assert.NotNull(narration);
    Assert.Equal(id, narration.Id);
    Assert.Equal(resultId, narration.ExplorationActionResultId);
    Assert.Equal(text, narration.Text);
  }

  [Fact]
  public void Create_ShouldAllowNullBounds()
  {
    // Arrange
    var resultId = Guid.NewGuid();
    const string text = "You rest your head for a moment.";

    // Act
    var narration = ExplorationActionResultNarration.Create(resultId, text);

    // Assert
    Assert.Equal(text, narration.Text);
  }

  #endregion
}
