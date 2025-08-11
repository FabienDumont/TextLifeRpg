using MockQueryable.FakeItEasy;
using TextLifeRpg.Domain;
using TextLifeRpg.Domain.Tests.Helpers;
using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.EfRepositories;

namespace TextLifeRpg.Infrastructure.Tests.EfRepositories;

public class DialogueOptionResultSpokenTextRepositoryTests
{
  #region Fields

  private readonly Guid _resultId = Guid.NewGuid();

  #endregion

  #region Methods

  [Fact]
  public async Task GetByDialogueOptionResultIdAsync_ShouldReturnText_WhenConditionsAreMet()
  {
    // Arrange
    var spokenTextId = Guid.NewGuid();
    var character = new CharacterBuilder().Build();
    character.Energy = 35;
    var world = World.Create(DateTime.Now, [character]);

    var gameContext = new GameContext
    {
      Actor = character,
      World = world
    };

    var spokenTexts = new List<DialogueOptionResultSpokenTextDataModel>
    {
      new()
      {
        Id = spokenTextId,
        DialogueOptionResultId = _resultId,
        Text = "I can't go on like this."
      }
    };

    var conditions = new List<ConditionDataModel>
    {
      new()
      {
        Id = Guid.NewGuid(),
        ContextType = ContextType.DialogueOptionResultSpokenText,
        ContextId = spokenTextId,
        ConditionType = ConditionType.ActorEnergy,
        Operator = "<=",
        OperandRight = "40",
        Negate = false
      }
    };

    var spokenTextDbSet = spokenTexts.BuildMockDbSet();
    var conditionDbSet = conditions.BuildMockDbSet();

    var context = A.Fake<ApplicationContext>();
    A.CallTo(() => context.DialogueOptionResultSpokenTexts).Returns(spokenTextDbSet);
    A.CallTo(() => context.Conditions).Returns(conditionDbSet);

    var repo = new DialogueOptionResultSpokenTextRepository(context);

    // Act
    var result = await repo.GetByDialogueOptionResultIdAsync(_resultId, gameContext, CancellationToken.None);

    // Assert
    Assert.Contains("I can't go on like this.", result);
  }

  [Fact]
  public async Task GetByDialogueOptionResultIdAsync_ShouldReturnNull_WhenNoTextMatches()
  {
    // Arrange
    var spokenTextId = Guid.NewGuid();
    var character = new CharacterBuilder().Build();
    character.Energy = 75; // Does not satisfy condition
    var world = World.Create(DateTime.Now, [character]);

    var gameContext = new GameContext
    {
      Actor = character,
      World = world
    };

    var spokenTexts = new List<DialogueOptionResultSpokenTextDataModel>
    {
      new()
      {
        Id = spokenTextId,
        DialogueOptionResultId = _resultId,
        Text = "You scream in frustration."
      }
    };

    var conditions = new List<ConditionDataModel>
    {
      new()
      {
        Id = Guid.NewGuid(),
        ContextType = ContextType.DialogueOptionResultSpokenText,
        ContextId = spokenTextId,
        ConditionType = ConditionType.ActorEnergy,
        Operator = "<=",
        OperandRight = "30",
        Negate = false
      }
    };

    var spokenTextDbSet = spokenTexts.BuildMockDbSet();
    var conditionDbSet = conditions.BuildMockDbSet();

    var context = A.Fake<ApplicationContext>();
    A.CallTo(() => context.DialogueOptionResultSpokenTexts).Returns(spokenTextDbSet);
    A.CallTo(() => context.Conditions).Returns(conditionDbSet);

    var repo = new DialogueOptionResultSpokenTextRepository(context);

    // Act
    var result = await repo.GetByDialogueOptionResultIdAsync(_resultId, gameContext, CancellationToken.None);

    // Assert
    Assert.Empty(result);
  }

  #endregion
}
