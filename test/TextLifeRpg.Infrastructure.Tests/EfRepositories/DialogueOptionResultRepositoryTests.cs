using MockQueryable.FakeItEasy;
using TextLifeRpg.Domain;
using TextLifeRpg.Domain.Tests.Helpers;
using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.EfRepositories;

namespace TextLifeRpg.Infrastructure.Tests.EfRepositories;

public class DialogueOptionResultRepositoryTests
{
  #region Methods

  [Fact]
  public async Task GetByDialogueOptionIdAsync_ShouldReturnMatchingResult_WhenConditionsAreMet()
  {
    // Arrange
    var dialogueOptionId = Guid.NewGuid();
    var matchingResultId = Guid.NewGuid();

    var player = new CharacterBuilder().Build();
    player.Energy = 75;
    var npc = new CharacterBuilder().Build();

    var world = World.Create(DateTime.Now, [player, npc]);
    var gameContext = new GameContext
    {
      Actor = player,
      Target = npc,
      World = world
    };

    var results = new List<DialogueOptionResultDataModel>
    {
      new()
      {
        Id = matchingResultId,
        DialogueOptionId = dialogueOptionId,
        EndDialogue = true
      },
      new()
      {
        Id = Guid.NewGuid(),
        DialogueOptionId = dialogueOptionId,
        EndDialogue = false
      }
    };

    var conditions = new List<ConditionDataModel>
    {
      new()
      {
        Id = Guid.NewGuid(),
        ContextType = ContextType.DialogueOptionResult,
        ContextId = matchingResultId,
        ConditionType = ConditionType.ActorEnergy,
        Operator = ">=",
        OperandRight = "50",
        Negate = false
      }
    };

    var resultDbSet = results.BuildMockDbSet();
    var conditionDbSet = conditions.BuildMockDbSet();

    var context = A.Fake<ApplicationContext>();
    A.CallTo(() => context.DialogueOptionResults).Returns(resultDbSet);
    A.CallTo(() => context.Conditions).Returns(conditionDbSet);

    var repo = new DialogueOptionResultRepository(context);

    // Act
    var result = await repo.GetByDialogueOptionIdAsync(dialogueOptionId, gameContext, CancellationToken.None);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(matchingResultId, result.Id);
  }

  [Fact]
  public async Task GetByDialogueOptionIdAsync_ShouldThrow_WhenNoMatchingResultExists()
  {
    // Arrange
    var dialogueOptionId = Guid.NewGuid();
    var resultId = Guid.NewGuid();

    var player = new CharacterBuilder().Build();
    player.Energy = 10; // too low for condition
    var npc = new CharacterBuilder().Build();

    var world = World.Create(DateTime.Now, [player, npc]);
    var gameContext = new GameContext
    {
      Actor = player,
      Target = npc,
      World = world
    };

    var results = new List<DialogueOptionResultDataModel>
    {
      new()
      {
        Id = resultId,
        DialogueOptionId = dialogueOptionId,
        EndDialogue = true
      }
    };

    var conditions = new List<ConditionDataModel>
    {
      new()
      {
        Id = Guid.NewGuid(),
        ContextType = ContextType.DialogueOptionResult,
        ContextId = resultId,
        ConditionType = ConditionType.ActorEnergy,
        Operator = ">=",
        OperandRight = "50",
        Negate = false
      }
    };

    var resultDbSet = results.BuildMockDbSet();
    var conditionDbSet = conditions.BuildMockDbSet();

    var context = A.Fake<ApplicationContext>();
    A.CallTo(() => context.DialogueOptionResults).Returns(resultDbSet);
    A.CallTo(() => context.Conditions).Returns(conditionDbSet);

    var repo = new DialogueOptionResultRepository(context);

    // Act & Assert
    var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
      repo.GetByDialogueOptionIdAsync(dialogueOptionId, gameContext, CancellationToken.None)
    );

    Assert.Equal($"No appropriate dialogue option result found for dialogue option {dialogueOptionId}.", ex.Message);
  }

  #endregion
}
