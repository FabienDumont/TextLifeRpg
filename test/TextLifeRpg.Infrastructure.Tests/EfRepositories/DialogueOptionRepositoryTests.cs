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

  private readonly List<DialogueOptionDataModel> _DialogueOptionData = [];
  private readonly DialogueOptionRepository _repository;

  #endregion

  #region Ctors

  public DialogueOptionRepositoryTests()
  {
    var context = A.Fake<ApplicationContext>();

    var DialogueOptionDbSet = _DialogueOptionData.BuildMockDbSet();
    A.CallTo(() => context.DialogueOptions).Returns(DialogueOptionDbSet);

    var conditionDbSet = _conditionData.BuildMockDbSet();
    A.CallTo(() => context.Conditions).Returns(conditionDbSet);

    A.CallTo(() => context.SaveChangesAsync(A<CancellationToken>._)).Returns(Task.FromResult(1));

    _repository = new DialogueOptionRepository(context);
  }

  #endregion

  #region Methods

  [Fact]
  public async Task GetPossibleDialogueOptionsAsync_ShouldReturnDialogueOptions_WhenMatchExists()
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
      Label = "Say goodbye"
    };

    _DialogueOptionData.Clear();
    _DialogueOptionData.Add(dialogueOption);

    _conditionData.Clear();

    // Act
    var result = await _repository.GetPossibleDialogueOptionsAsync(gameContext, CancellationToken.None);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(1, result.Count);
  }

  [Fact]
  public async Task GetAsync_ShouldThrow_WhenConditionsNotMet()
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
      Label = "Say goodbye"
    };

    _DialogueOptionData.Clear();
    _DialogueOptionData.Add(dialogueOption);

    _conditionData.Clear();
    _conditionData.Add(
      new ConditionDataModel
      {
        ConditionType = ConditionType.ActorHasTrait,
        OperandLeft = Guid.NewGuid().ToString(),
        Operator = "=",
        OperandRight = "true",
        Negate = false,
        ContextType = ContextType.DialogueOption,
        ContextId = dialogueOption.Id
      }
    );

    // Act & Assert
    var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
      await _repository.GetPossibleDialogueOptionsAsync(gameContext, CancellationToken.None)
    );

    Assert.Equal("No appropriate dialogue option found.", exception.Message);
  }

  #endregion
}
