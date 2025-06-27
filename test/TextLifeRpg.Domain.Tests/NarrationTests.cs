using TextLifeRpg.Domain;

namespace TextLifeRpg.Domain.Tests;

public class NarrationTests
{
  #region Methods

  [Fact]
  public void Create_ShouldInitializeWithGeneratedId()
  {
    // Arrange
    const string key = "intro_scene";
    const string text = "Welcome to the game.";

    // Act
    var narration = Narration.Create(key, text);

    // Assert
    Assert.NotNull(narration);
    Assert.NotEqual(Guid.Empty, narration.Id);
    Assert.Equal(key, narration.Key);
    Assert.Equal(text, narration.Text);
  }

  [Fact]
  public void Load_ShouldInitializeWithGivenValues()
  {
    // Arrange
    var id = Guid.NewGuid();
    const string key = "intro_scene";
    const string text = "Welcome back.";

    // Act
    var narration = Narration.Load(id, key, text);

    // Assert
    Assert.NotNull(narration);
    Assert.Equal(id, narration.Id);
    Assert.Equal(key, narration.Key);
    Assert.Equal(text, narration.Text);
  }

  #endregion
}
