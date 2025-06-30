using MockQueryable.FakeItEasy;
using TextLifeRpg.Domain;
using TextLifeRpg.Domain.Tests.Helpers;
using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.EfRepositories;

namespace TextLifeRpg.Infrastructure.Tests.EfRepositories;

public class ExplorationActionResultRepositoryTests
{
  #region Methods

  [Fact]
  public async Task GetByExplorationActionIdAsync_ShouldReturnMatchingResult_WhenConditionsAreMet()
  {
    // Arrange
    var explorationActionId = Guid.NewGuid();
    var matchingResultId = Guid.NewGuid();

    var character = new CharacterBuilder().Build();
    character.Energy = 50;
    character.Money = 200;
    var world = World.Create(DateTime.Now, [character]);

    var gameContext = new GameContext
    {
      Actor = character,
      World = world
    };

    var results = new List<ExplorationActionResultDataModel>
    {
      new()
      {
        Id = matchingResultId,
        ExplorationActionId = explorationActionId,
        AddMinutes = true,
        EnergyChange = 10,
        MoneyChange = 20
      },
      new()
      {
        Id = Guid.NewGuid(),
        ExplorationActionId = explorationActionId,
        AddMinutes = true,
        EnergyChange = 5,
        MoneyChange = 0
      }
    };

    var conditions = new List<ConditionDataModel>
    {
      new()
      {
        Id = Guid.NewGuid(),
        ContextType = ContextType.ExplorationActionResult,
        ContextId = matchingResultId,
        ConditionType = ConditionType.ActorEnergy,
        Operator = ">=",
        OperandRight = "50",
        Negate = false
      },
      new()
      {
        Id = Guid.NewGuid(),
        ContextType = ContextType.ExplorationActionResult,
        ContextId = matchingResultId,
        ConditionType = ConditionType.ActorMoney,
        Operator = ">",
        OperandRight = "100",
        Negate = false
      }
    };

    var resultDbSet = results.AsQueryable().BuildMockDbSet();
    var conditionDbSet = conditions.AsQueryable().BuildMockDbSet();

    var context = A.Fake<ApplicationContext>();
    A.CallTo(() => context.ExplorationActionResults).Returns(resultDbSet);
    A.CallTo(() => context.Conditions).Returns(conditionDbSet);

    var repo = new ExplorationActionResultRepository(context);

    // Act
    var result = await repo.GetByExplorationActionIdAsync(explorationActionId, gameContext, CancellationToken.None);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(matchingResultId, result.Id);
  }

  [Fact]
  public async Task GetByExplorationActionIdAsync_ShouldThrow_WhenNoMatchingResultExists()
  {
    // Arrange
    var explorationActionId = Guid.NewGuid();
    var resultId = Guid.NewGuid();

    var character = new CharacterBuilder().Build();
    character.Energy = 10;
    character.Money = 5;
    var world = World.Create(DateTime.Now, [character]);

    var gameContext = new GameContext
    {
      Actor = character,
      World = world
    };

    var results = new List<ExplorationActionResultDataModel>
    {
      new()
      {
        Id = resultId,
        ExplorationActionId = explorationActionId,
        AddMinutes = true,
        EnergyChange = 10,
        MoneyChange = 20
      }
    };

    var conditions = new List<ConditionDataModel>
    {
      new()
      {
        Id = Guid.NewGuid(),
        ContextType = ContextType.ExplorationActionResult,
        ContextId = resultId,
        ConditionType = ConditionType.ActorEnergy,
        Operator = ">=",
        OperandRight = "50",
        Negate = false
      }
    };

    var resultDbSet = results.AsQueryable().BuildMockDbSet();
    var conditionDbSet = conditions.AsQueryable().BuildMockDbSet();

    var context = A.Fake<ApplicationContext>();
    A.CallTo(() => context.ExplorationActionResults).Returns(resultDbSet);
    A.CallTo(() => context.Conditions).Returns(conditionDbSet);

    var repo = new ExplorationActionResultRepository(context);

    // Act & Assert
    var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
      repo.GetByExplorationActionIdAsync(explorationActionId, gameContext, CancellationToken.None)
    );

    Assert.Equal(
      $"No appropriate exploration action result found for exploration action {explorationActionId}.", ex.Message
    );
  }

  #endregion
}
