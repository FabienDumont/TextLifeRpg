using MockQueryable.FakeItEasy;
using TextLifeRpg.Domain;
using TextLifeRpg.Domain.Tests.Helpers;
using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.EfRepositories;

namespace TextLifeRpg.Infrastructure.Tests.EfRepositories;

public class DialogueOptionRepositoryTests
{
  #region Fields

  private readonly List<ConditionDataModel> _conditionData = [];

  private readonly List<DialogueOptionDataModel> _dialogueOptionData = [];
  private readonly List<DialogueOptionResultNextDialogueOption> _dialogueOptionResultNextDialogueOptionData = [];
  private readonly DialogueOptionRepository _repository;

  #endregion

  #region Ctors

  public DialogueOptionRepositoryTests()
  {
    var context = A.Fake<ApplicationContext>();

    var dialogueOptionDbSet = _dialogueOptionData.BuildMockDbSet();
    A.CallTo(() => context.DialogueOptions).Returns(dialogueOptionDbSet);

    var dialogueOptionResultNextDialogueOptionDbSet = _dialogueOptionResultNextDialogueOptionData.BuildMockDbSet();
    A.CallTo(() => context.DialogueOptionResultNextDialogueOptions)
      .Returns(dialogueOptionResultNextDialogueOptionDbSet);

    var conditionDbSet = _conditionData.BuildMockDbSet();
    A.CallTo(() => context.Conditions).Returns(conditionDbSet);

    A.CallTo(() => context.SaveChangesAsync(A<CancellationToken>._)).Returns(Task.FromResult(1));

    _repository = new DialogueOptionRepository(context);
  }

  #endregion

  #region Methods

  [Fact]
  public async Task GetPossibleInitialDialogueOptionsAsync_ShouldReturnDialogueOptions_WhenMatchExists()
  {
    // Arrange
    var character = new CharacterBuilder().Build();
    var world = World.Create(DateTime.Now, [character]);

    var gameContext = new GameContext
    {
      Actor = character,
      World = world
    };
    var dialogueOption = new DialogueOptionDataModel
    {
      Id = Guid.NewGuid(),
      Label = "Say goodbye", NeededMinutes = 0
    };

    _dialogueOptionData.Clear();
    _dialogueOptionData.Add(dialogueOption);

    _conditionData.Clear();

    // Act
    var result = await _repository.GetPossibleInitialDialogueOptionsAsync(gameContext, CancellationToken.None);

    // Assert
    Assert.NotNull(result);
    Assert.Single(result);
  }

  [Fact]
  public async Task GetPossibleInitialDialogueOptionsAsync_ShouldThrow_WhenConditionsNotMet()
  {
    // Arrange
    var character = new CharacterBuilder().Build();
    var world = World.Create(DateTime.Now, [character]);

    var gameContext = new GameContext
    {
      Actor = character,
      World = world
    };
    var dialogueOptionId = Guid.NewGuid();

    var dialogueOption = new DialogueOptionDataModel
    {
      Id = dialogueOptionId,
      Label = "Say goodbye", NeededMinutes = 0
    };

    _dialogueOptionData.Clear();
    _dialogueOptionData.Add(dialogueOption);

    _conditionData.Clear();
    _conditionData.Add(
      new ConditionDataModel
      {
        ConditionType = ConditionType.ActorHasTrait,
        OperandLeft = Guid.NewGuid().ToString(),
        Operator = "==",
        OperandRight = "true",
        Negate = false,
        ContextType = ContextType.DialogueOption,
        ContextId = dialogueOption.Id
      }
    );

    // Act & Assert
    var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
      await _repository.GetPossibleInitialDialogueOptionsAsync(gameContext, CancellationToken.None)
    );

    Assert.Equal("No appropriate dialogue option found.", exception.Message);
  }

  [Fact]
  public async Task GetPossibleFollowUpsAsync_ShouldReturnOrderedFollowUps_FilteringByConditions()
  {
    // Arrange
    var character = new CharacterBuilder().Build();
    var world = World.Create(DateTime.Now, [character]);

    var gameContext = new GameContext
    {
      Actor = character,
      World = world
    };

    var resultId = Guid.NewGuid();
    var next1Id = Guid.NewGuid(); // should PASS
    var next2Id = Guid.NewGuid(); // should FAIL due to condition

    // dialogue options that can be followed up
    _dialogueOptionData.Clear();
    _dialogueOptionData.AddRange(
      [
        new DialogueOptionDataModel {Id = next1Id, Label = "Ask about job", NeededMinutes = 0},
        new DialogueOptionDataModel {Id = next2Id, Label = "Nevermind", NeededMinutes = 0}
      ]
    );

    // links from result -> next options (order 0 then 1)
    _dialogueOptionResultNextDialogueOptionData.Clear();
    _dialogueOptionResultNextDialogueOptionData.AddRange(
      [
        new DialogueOptionResultNextDialogueOption
        {
          Id = Guid.NewGuid(),
          DialogueOptionResultId = resultId,
          NextDialogueOptionId = next1Id,
          Order = 0
        },
        new DialogueOptionResultNextDialogueOption
        {
          Id = Guid.NewGuid(),
          DialogueOptionResultId = resultId,
          NextDialogueOptionId = next2Id,
          Order = 1
        }
      ]
    );

    _conditionData.Clear();
    _conditionData.Add(
      new ConditionDataModel
      {
        Id = Guid.NewGuid(),
        ContextType = ContextType.DialogueOption,
        ContextId = next2Id,
        ConditionType = ConditionType.ActorHasTrait,
        OperandLeft = Guid.NewGuid().ToString(), // random trait not present
        Operator = "==",
        OperandRight = "true",
        Negate = false
      }
    );

    // Act
    var result = await _repository.GetPossibleFollowUpsAsync(gameContext, resultId, CancellationToken.None);

    // Assert
    Assert.NotNull(result);
    var list = result.ToList();
    Assert.Single(list);
    Assert.Equal("Ask about job", list[0].Label); // order preserved; next2 filtered out
  }

  [Fact]
  public async Task GetPossibleFollowUpsAsync_ShouldReturnEmpty_WhenNoLinksForResult()
  {
    // Arrange
    var character = new CharacterBuilder().Build();
    var world = World.Create(DateTime.Now, [character]);

    var gameContext = new GameContext
    {
      Actor = character,
      World = world
    };

    var resultId = Guid.NewGuid();

    _dialogueOptionData.Clear();
    _dialogueOptionResultNextDialogueOptionData.Clear();
    _conditionData.Clear();

    // Act
    var result = await _repository.GetPossibleFollowUpsAsync(gameContext, resultId, CancellationToken.None);

    // Assert
    Assert.NotNull(result);
    Assert.Empty(result);
  }

  #endregion
}
