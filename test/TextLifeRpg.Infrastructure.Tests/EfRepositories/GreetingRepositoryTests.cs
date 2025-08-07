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
    var greetings = await _repository.GetAsync(gameContext, CancellationToken.None);

    // Assert
    Assert.NotEmpty(greetings);
  }

  #endregion
}
