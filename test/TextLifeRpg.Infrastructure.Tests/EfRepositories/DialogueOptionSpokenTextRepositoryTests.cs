using MockQueryable.FakeItEasy;
using TextLifeRpg.Domain;
using TextLifeRpg.Domain.Tests.Helpers;
using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.EfRepositories;

namespace TextLifeRpg.Infrastructure.Tests.EfRepositories;

public class DialogueOptionSpokenTextRepositoryTests
{
  #region Fields

  private readonly Guid _dialogueOptionId = Guid.NewGuid();

  #endregion

  #region Methods

  [Fact]
  public async Task GetByDialogueOptionIdAsync_ShouldReturnMatchingText_WhenConditionsAreMet()
  {
    // Arrange
    var matchingId = Guid.NewGuid();
    var character = new CharacterBuilder().Build();
    character.Energy = 50;
    var world = World.Create(DateTime.Now, [character]);

    var gameContext = new GameContext
    {
      Actor = character,
      World = world
    };

    var spokenTexts = new List<DialogueOptionSpokenTextDataModel>
    {
      new()
      {
        Id = matchingId,
        DialogueOptionId = _dialogueOptionId,
        Text = "I don't feel like talking."
      },
      new()
      {
        Id = Guid.NewGuid(),
        DialogueOptionId = _dialogueOptionId,
        Text = "Leave me alone."
      }
    };

    var conditions = new List<ConditionDataModel>
    {
      new()
      {
        Id = Guid.NewGuid(),
        ContextType = ContextType.DialogueOptionSpokenText,
        ContextId = matchingId,
        ConditionType = ConditionType.ActorEnergy,
        Operator = "<=",
        OperandRight = "60",
        Negate = false
      }
    };

    var spokenTextDbSet = spokenTexts.BuildMockDbSet();
    var conditionDbSet = conditions.BuildMockDbSet();

    var context = A.Fake<ApplicationContext>();
    A.CallTo(() => context.DialogueOptionSpokenTexts).Returns(spokenTextDbSet);
    A.CallTo(() => context.Conditions).Returns(conditionDbSet);

    var repo = new DialogueOptionSpokenTextRepository(context);

    // Act
    var result = await repo.GetByDialogueOptionIdAsync(_dialogueOptionId, gameContext, CancellationToken.None);

    // Assert
    Assert.Equal("I don't feel like talking.", result);
  }

  [Fact]
  public async Task GetByDialogueOptionIdAsync_ShouldThrow_WhenNoTextMatches()
  {
    // Arrange
    var character = new CharacterBuilder().Build();
    character.Energy = 90; // Fails condition
    var world = World.Create(DateTime.Now, [character]);

    var gameContext = new GameContext
    {
      Actor = character,
      World = world
    };

    var spokenTextId = Guid.NewGuid();

    var spokenTexts = new List<DialogueOptionSpokenTextDataModel>
    {
      new()
      {
        Id = spokenTextId,
        DialogueOptionId = _dialogueOptionId,
        Text = "You grunt something incomprehensible."
      }
    };

    var conditions = new List<ConditionDataModel>
    {
      new()
      {
        Id = Guid.NewGuid(),
        ContextType = ContextType.DialogueOptionSpokenText,
        ContextId = spokenTextId,
        ConditionType = ConditionType.ActorEnergy,
        Operator = "<=",
        OperandRight = "30", // character has 90
        Negate = false
      }
    };

    var spokenTextDbSet = spokenTexts.BuildMockDbSet();
    var conditionDbSet = conditions.BuildMockDbSet();

    var context = A.Fake<ApplicationContext>();
    A.CallTo(() => context.DialogueOptionSpokenTexts).Returns(spokenTextDbSet);
    A.CallTo(() => context.Conditions).Returns(conditionDbSet);

    var repo = new DialogueOptionSpokenTextRepository(context);

    // Act & Assert
    var result = await repo.GetByDialogueOptionIdAsync(_dialogueOptionId, gameContext, CancellationToken.None);

    Assert.Null(result);
  }

  #endregion
}
