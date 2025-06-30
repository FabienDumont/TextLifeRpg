namespace TextLifeRpg.Domain.Tests;

public class GreetingTests
{
  #region Methods

  [Fact]
  public void Create_ShouldInitialize()
  {
    // Arrange
    const string spokenText = "Hello there";

    // Act
    var greeting = Greeting.Create(spokenText);

    // Assert
    Assert.NotNull(greeting);
    Assert.NotEqual(Guid.Empty, greeting.Id);
    Assert.Equal(spokenText, greeting.SpokenText);
  }

  [Fact]
  public void Load_ShouldInitializeWithGivenValues()
  {
    // Arrange
    var id = Guid.NewGuid();
    const string spokenText = "Hey!";

    // Act
    var greeting = Greeting.Load(id, spokenText);

    // Assert
    Assert.Equal(id, greeting.Id);
    Assert.Equal(spokenText, greeting.SpokenText);
  }

  #endregion
}
