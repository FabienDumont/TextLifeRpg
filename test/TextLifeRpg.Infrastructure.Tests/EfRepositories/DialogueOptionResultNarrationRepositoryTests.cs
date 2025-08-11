using MockQueryable.FakeItEasy;
using TextLifeRpg.Domain;
using TextLifeRpg.Domain.Tests.Helpers;
using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.EfRepositories;

namespace TextLifeRpg.Infrastructure.Tests.EfRepositories;

public class DialogueOptionResultNarrationRepositoryTests
{
  #region Fields

  private readonly Guid _resultId = Guid.NewGuid();

  #endregion

  #region Methods

  [Fact]
  public async Task GetByDialogueOptionResultIdAsync_ShouldReturnNarration_WhenConditionsAreMet()
  {
    // Arrange
    var narrationId = Guid.NewGuid();
    var character = new CharacterBuilder().Build();
    character.Energy = 20;
    var world = World.Create(DateTime.Now, [character]);

    var gameContext = new GameContext
    {
      Actor = character,
      World = world
    };

    var narrations = new List<DialogueOptionResultNarrationDataModel>
    {
      new()
      {
        Id = narrationId,
        DialogueOptionResultId = _resultId,
        Text = "You collapse on the ground."
      }
    };

    var conditions = new List<ConditionDataModel>
    {
      new()
      {
        Id = Guid.NewGuid(),
        ContextType = ContextType.DialogueOptionResultNarration,
        ContextId = narrationId,
        ConditionType = ConditionType.ActorEnergy,
        Operator = "<=",
        OperandRight = "25",
        Negate = false
      }
    };

    var narrationDbSet = narrations.BuildMockDbSet();
    var conditionDbSet = conditions.BuildMockDbSet();

    var context = A.Fake<ApplicationContext>();
    A.CallTo(() => context.DialogueOptionResultNarrations).Returns(narrationDbSet);
    A.CallTo(() => context.Conditions).Returns(conditionDbSet);

    var repo = new DialogueOptionResultNarrationRepository(context);

    // Act
    var result = await repo.GetByDialogueOptionResultIdAsync(_resultId, gameContext, CancellationToken.None);

    // Assert
    Assert.Equal("You collapse on the ground.", result);
  }

  [Fact]
  public async Task GetByDialogueOptionResultIdAsync_ShouldReturnNull_WhenNoNarrationMatches()
  {
    // Arrange
    var narrationId = Guid.NewGuid();
    var character = new CharacterBuilder().Build();
    character.Energy = 80; // Fails condition
    var world = World.Create(DateTime.Now, [character]);

    var gameContext = new GameContext
    {
      Actor = character,
      World = world
    };

    var narrations = new List<DialogueOptionResultNarrationDataModel>
    {
      new()
      {
        Id = narrationId,
        DialogueOptionResultId = _resultId,
        Text = "You hesitate."
      }
    };

    var conditions = new List<ConditionDataModel>
    {
      new()
      {
        Id = Guid.NewGuid(),
        ContextType = ContextType.DialogueOptionResultNarration,
        ContextId = narrationId,
        ConditionType = ConditionType.ActorEnergy,
        Operator = "<=",
        OperandRight = "25", // fails with energy = 80
        Negate = false
      }
    };

    var narrationDbSet = narrations.BuildMockDbSet();
    var conditionDbSet = conditions.BuildMockDbSet();

    var context = A.Fake<ApplicationContext>();
    A.CallTo(() => context.DialogueOptionResultNarrations).Returns(narrationDbSet);
    A.CallTo(() => context.Conditions).Returns(conditionDbSet);

    var repo = new DialogueOptionResultNarrationRepository(context);

    // Act
    var result = await repo.GetByDialogueOptionResultIdAsync(_resultId, gameContext, CancellationToken.None);

    // Assert
    Assert.Null(result);
  }

  #endregion
}
