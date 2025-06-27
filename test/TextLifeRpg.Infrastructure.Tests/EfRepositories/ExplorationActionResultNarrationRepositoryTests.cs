using MockQueryable.FakeItEasy;
using TextLifeRpg.Domain;
using TextLifeRpg.Domain.Tests.Helpers;
using TextLifeRpg.Infrastructure;
using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.EfRepositories;

namespace TextLifeRpg.Infrastructure.Tests.EfRepositories;

public class ExplorationActionResultNarrationRepositoryTests
{
  #region Fields

  private readonly Guid _resultId = Guid.NewGuid();

  #endregion

  #region Methods

  [Fact]
  public async Task GetByExplorationActionResultIdAsync_ShouldReturnMatchingNarration_WhenCharacterMeetsConditions()
  {
    // Arrange
    var matchingId = Guid.NewGuid();
    var character = new CharacterBuilder().Build();
    character.Energy = 45;
    var world = World.Create(DateTime.Now, [character]);

    var gameContext = new GameContext
    {
      Actor = character,
      World = world
    };

    var narrations = new List<ExplorationActionResultNarrationDataModel>
    {
      new()
      {
        Id = matchingId,
        ExplorationActionResultId = _resultId,
        Text = "You lie down with a heavy sigh."
      },
      new()
      {
        Id = Guid.NewGuid(),
        ExplorationActionResultId = _resultId,
        Text = "You crash into bed instantly."
      }
    };

    var conditions = new List<ConditionDataModel>
    {
      new()
      {
        Id = Guid.NewGuid(),
        ContextType = ContextType.ExplorationActionResultNarration,
        ContextId = matchingId,
        ConditionType = ConditionType.ActorEnergy,
        Operator = "<=",
        OperandRight = "50",
        Negate = false
      }
    };

    var narrationDbSet = narrations.AsQueryable().BuildMockDbSet();
    var conditionDbSet = conditions.AsQueryable().BuildMockDbSet();

    var context = A.Fake<ApplicationContext>();
    A.CallTo(() => context.ExplorationActionResultNarrations).Returns(narrationDbSet);
    A.CallTo(() => context.Conditions).Returns(conditionDbSet);

    var repo = new ExplorationActionResultNarrationRepository(context);

    // Act
    var result = await repo.GetByExplorationActionResultIdAsync(_resultId, gameContext, CancellationToken.None);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(matchingId, result.Id);
    Assert.Equal("You lie down with a heavy sigh.", result.Text);
  }

  [Fact]
  public async Task GetByExplorationActionResultIdAsync_ShouldThrow_WhenNoNarrationMatches()
  {
    // Arrange
    var character = new CharacterBuilder().Build();
    character.Energy = 80;
    var world = World.Create(DateTime.Now, [character]);

    var gameContext = new GameContext
    {
      Actor = character,
      World = world
    };

    var narrationId = Guid.NewGuid();

    var narrations = new List<ExplorationActionResultNarrationDataModel>
    {
      new()
      {
        Id = narrationId,
        ExplorationActionResultId = _resultId,
        Text = "You're completely drained."
      }
    };

    var conditions = new List<ConditionDataModel>
    {
      new()
      {
        Id = Guid.NewGuid(),
        ContextType = ContextType.ExplorationActionResultNarration,
        ContextId = narrationId,
        ConditionType = ConditionType.ActorEnergy,
        Operator = "<=",
        OperandRight = "25", // character has 80, this fails
        Negate = false
      }
    };

    var narrationDbSet = narrations.AsQueryable().BuildMockDbSet();
    var conditionDbSet = conditions.AsQueryable().BuildMockDbSet();

    var context = A.Fake<ApplicationContext>();
    A.CallTo(() => context.ExplorationActionResultNarrations).Returns(narrationDbSet);
    A.CallTo(() => context.Conditions).Returns(conditionDbSet);

    var repo = new ExplorationActionResultNarrationRepository(context);

    // Act & Assert
    var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
      repo.GetByExplorationActionResultIdAsync(_resultId, gameContext, CancellationToken.None)
    );

    Assert.Equal(
      $"No appropriate exploration action result narration found for exploration action result {_resultId}.", ex.Message
    );
  }

  #endregion
}
