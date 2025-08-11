namespace TextLifeRpg.Domain.Tests;

public class GameSettingsTests
{
  #region Methods

  [Fact]
  public void Create_ShouldInitializeCorrectly()
  {
    // Act
    var settings = GameSettings.Create(NpcDensity.Average);

    // Assert
    Assert.NotNull(settings);
    Assert.NotEqual(Guid.Empty, settings.Id);
    Assert.Equal(NpcDensity.Average, settings.NpcDensity);
  }

  [Fact]
  public void Load_ShouldInitializeCorrectly()
  {
    // Arrange
    var id = Guid.NewGuid();

    // Act
    var settings = GameSettings.Load(id, NpcDensity.Average);

    // Assert
    Assert.Equal(id, settings.Id);
    Assert.Equal(NpcDensity.Average, settings.NpcDensity);
  }

  [Theory]
  [InlineData(NpcDensity.VeryLow, 20)]
  [InlineData(NpcDensity.Low, 30)]
  [InlineData(NpcDensity.Average, 40)]
  [InlineData(NpcDensity.High, 50)]
  [InlineData(NpcDensity.VeryHigh, 60)]
  [InlineData((NpcDensity) 999, 40)]
  public void GetNpcCount_ShouldReturnCorrectValue(NpcDensity density, int expectedCount)
  {
    // Arrange
    var settings = GameSettings.Create(density);

    // Act
    var count = settings.GetNpcCount();

    // Assert
    Assert.Equal(expectedCount, count);
  }

  #endregion
}
