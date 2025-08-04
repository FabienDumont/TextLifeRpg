using MockQueryable.FakeItEasy;
using TextLifeRpg.Domain;
using TextLifeRpg.Domain.Tests.Helpers;
using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.EfRepositories;

namespace TextLifeRpg.Infrastructure.Tests.EfRepositories;

public class GreetingRepositoryTests
{
  #region Fields

  private readonly List<ConditionDataModel> _conditionData = [];

  private readonly List<GreetingDataModel> _greetingData = [];
  private readonly GreetingRepository _repository;

  #endregion

  #region Ctors

  public GreetingRepositoryTests()
  {
    var context = A.Fake<ApplicationContext>();

    var greetingDbSet = _greetingData.BuildMockDbSet();
    A.CallTo(() => context.Greetings).Returns(greetingDbSet);

    var conditionDbSet = _conditionData.BuildMockDbSet();
    A.CallTo(() => context.Conditions).Returns(conditionDbSet);

    A.CallTo(() => context.SaveChangesAsync(A<CancellationToken>._)).Returns(Task.FromResult(1));

    _repository = new GreetingRepository(context);
  }

  #endregion

  #region Methods

  [Fact]
  public async Task GetAsync_ShouldReturnGreeting_WhenMatchExists()
  {
    // Arrange
    var character = new CharacterBuilder().Build();
    var world = World.Create(DateTime.Now, [character]);

    var gameContext = new GameContext
    {
      Actor = character,
      World = world
    };
    var greeting = new GreetingDataModel
    {
      Id = Guid.NewGuid(),
      SpokenText = "Yo!"
    };

    _greetingData.Clear();
    _greetingData.Add(greeting);

    _conditionData.Clear();

    // Act
    var result = await _repository.GetAsync(gameContext, CancellationToken.None);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(greeting.Id, result.Id);
    Assert.Equal(greeting.SpokenText, result.SpokenText);
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
    var greetingId = Guid.NewGuid();

    var greeting = new GreetingDataModel
    {
      Id = greetingId,
      SpokenText = "Leave me alone!"
    };

    _greetingData.Clear();
    _greetingData.Add(greeting);

    _conditionData.Clear();
    _conditionData.Add(
      new ConditionDataModel
      {
        ConditionType = ConditionType.ActorHasTrait,
        OperandLeft = Guid.NewGuid().ToString(),
        Operator = "=",
        OperandRight = "true",
        Negate = false,
        ContextType = ContextType.Greeting,
        ContextId = greeting.Id
      }
    );

    // Act & Assert
    var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
      await _repository.GetAsync(gameContext, CancellationToken.None)
    );

    Assert.Equal("No appropriate greeting found.", exception.Message);
  }

  #endregion
}
