using TextLifeRpg.Infrastructure.EfDataModels;

namespace TextLifeRpg.Infrastructure.Tests.EfDataModels;

public class ExplorationActionResultDataModelTests
{
  #region Methods

  [Fact]
  public void Instantiation_ShouldInitializeWithGivenValues()
  {
    // Arrange
    var resultId = Guid.NewGuid();
    var actionId = Guid.NewGuid();

    const int energyChange = -5;
    const int moneyChange = 10;

    var action = new ExplorationActionDataModel
    {
      Id = actionId,
      LocationId = Guid.NewGuid(),
      RoomId = null,
      Label = "Search the area",
      NeededMinutes = 15
    };

    // Act
    var result = new ExplorationActionResultDataModel
    {
      Id = resultId,
      ExplorationActionId = actionId,
      AddMinutes = true,
      EnergyChange = energyChange,
      MoneyChange = moneyChange,
      ExplorationAction = action
    };

    // Assert
    Assert.Equal(resultId, result.Id);
    Assert.Equal(actionId, result.ExplorationActionId);
    Assert.True(result.AddMinutes);
    Assert.Equal(energyChange, result.EnergyChange);
    Assert.Equal(moneyChange, result.MoneyChange);
    Assert.Equal(action, result.ExplorationAction);
  }

  [Fact]
  public void Instantiation_ShouldAllowNullOptionalValues()
  {
    // Arrange
    var result = new ExplorationActionResultDataModel
    {
      Id = Guid.NewGuid(),
      ExplorationActionId = Guid.NewGuid()
    };

    // Assert
    Assert.Null(result.EnergyChange);
    Assert.Null(result.MoneyChange);
    Assert.Null(result.ExplorationAction);
  }

  #endregion
}
