using MockQueryable.FakeItEasy;
using TextLifeRpg.Domain;
using TextLifeRpg.Domain.Tests.Helpers;
using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.EfRepositories;

namespace TextLifeRpg.Infrastructure.Tests.EfRepositories;

public class NarrationRepositoryTests
{
  #region Fields

  private const string _expectedKey = "intro_scene";
  private const string _expectedText = "You step into the dark forest.";
  private readonly NarrationRepository _repository;

  #endregion

  #region Ctors

  public NarrationRepositoryTests()
  {
    var narrationDataModels = new List<NarrationDataModel>
    {
      new()
      {
        Id = Guid.NewGuid(),
        Key = _expectedKey,
        Text = _expectedText
      }
    };

    var context = A.Fake<ApplicationContext>();

    var narrationDbSet = narrationDataModels.BuildMockDbSet();
    A.CallTo(() => context.Narrations).Returns(narrationDbSet);

    var conditionDbSet = new List<ConditionDataModel>().BuildMockDbSet();
    A.CallTo(() => context.Conditions).Returns(conditionDbSet);

    A.CallTo(() => context.SaveChangesAsync(A<CancellationToken>._)).Returns(Task.FromResult(1));

    _repository = new NarrationRepository(context);
  }

  #endregion

  #region Tests

  [Fact]
  public async Task GetNarrationByKeyAsync_ShouldReturnNarration_WhenExists()
  {
    // Arrange
    var character = new CharacterBuilder().Build();
    var world = World.Create(DateTime.Now, [character]);

    var gameContext = new GameContext
    {
      Actor = character,
      World = world
    };

    // Act
    var result = await _repository.GetNarrationByKeyAsync(_expectedKey, gameContext, CancellationToken.None);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(_expectedKey, result.Key);
    Assert.Equal(_expectedText, result.Text);
  }

  [Fact]
  public async Task GetNarrationByKeyAsync_ShouldThrow_WhenNarrationDoesNotExist()
  {
    // Arrange
    const string nonExistentKey = "non_existent_scene";
    var character = new CharacterBuilder().Build();
    var world = World.Create(DateTime.Now, [character]);

    var gameContext = new GameContext
    {
      Actor = character,
      World = world
    };

    // Act & Assert
    var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
      await _repository.GetNarrationByKeyAsync(nonExistentKey, gameContext, CancellationToken.None)
    );

    Assert.Equal($"No narration found for key {nonExistentKey}.", exception.Message);
  }

  [Fact]
  public async Task GetNarrationByKeyAsync_ShouldThrow_WhenConditionsAreNotMet()
  {
    // Arrange
    var narrationId = Guid.NewGuid();
    var narration = new NarrationDataModel
    {
      Id = narrationId,
      Key = _expectedKey,
      Text = _expectedText
    };

    var failingCondition = new ConditionDataModel
    {
      Id = Guid.NewGuid(),
      ContextType = ContextType.Narration,
      ContextId = narrationId,
      ConditionType = ConditionType.ActorEnergy,
      Operator = ">",
      OperandRight = "999",
      Negate = false
    };

    var character = new CharacterBuilder().Build();
    character.Energy = 50;
    var world = World.Create(DateTime.Now, [character]);

    var context = A.Fake<ApplicationContext>();
    var narrationDbSet = new List<NarrationDataModel> {narration}.BuildMockDbSet();
    var conditionDbSet = new List<ConditionDataModel> {failingCondition}.BuildMockDbSet();

    A.CallTo(() => context.Narrations).Returns(narrationDbSet);
    A.CallTo(() => context.Conditions).Returns(conditionDbSet);
    A.CallTo(() => context.SaveChangesAsync(A<CancellationToken>._)).Returns(Task.FromResult(1));

    var repository = new NarrationRepository(context);
    var gameContext = new GameContext {Actor = character, World = world};

    // Act & Assert
    var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
      repository.GetNarrationByKeyAsync(_expectedKey, gameContext, CancellationToken.None)
    );

    Assert.Equal($"No narration found for key {_expectedKey}.", exception.Message);
  }

  #endregion
}
