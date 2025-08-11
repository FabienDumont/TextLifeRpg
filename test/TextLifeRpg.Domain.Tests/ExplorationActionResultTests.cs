namespace TextLifeRpg.Domain.Tests;

public class ExplorationActionResultTests
{
  #region Methods

  [Fact]
  public void Create_ShouldInitializeWithGeneratedId()
  {
    // Arrange
    var explorationActionId = Guid.NewGuid();

    // Act
    var result = ExplorationActionResult.Create(explorationActionId, false, null, null);

    // Assert
    Assert.NotNull(result);
    Assert.NotEqual(Guid.Empty, result.Id);
    Assert.Equal(explorationActionId, result.ExplorationActionId);
  }

  [Fact]
  public void Load_ShouldInitializeWithGivenValues()
  {
    // Arrange
    var id = Guid.NewGuid();
    var explorationActionId = Guid.NewGuid();
    const int energyChange = -2;
    const int moneyChange = 200;

    // Act
    var result = ExplorationActionResult.Load(id, explorationActionId, true, energyChange, moneyChange);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(id, result.Id);
    Assert.Equal(explorationActionId, result.ExplorationActionId);
    Assert.True(result.AddMinutes);
    Assert.Equal(energyChange, result.EnergyChange);
    Assert.Equal(moneyChange, result.MoneyChange);
  }

  #endregion
}
