using TextLifeRpg.Application.Abstraction.Repositories;
using TextLifeRpg.Application.Services;
using TextLifeRpg.Domain;
using TextLifeRpg.Domain.Tests.Helpers;

namespace TextLifeRpg.Application.Tests.Services;

public class DialogueServiceTests
{
  #region Fields

  private readonly IGreetingRepository _greetingRepository = A.Fake<IGreetingRepository>();
  private readonly DialogueService _dialogueService;

  #endregion

  #region Ctors

  public DialogueServiceTests()
  {
    _dialogueService = new DialogueService(_greetingRepository);
  }

  #endregion

  #region Methods

  [Fact]
  public async Task GetGreetingAsync_ShouldCallGreetingRepositoryWithCorrectContextAndReturnGreeting()
  {
    // Arrange
    var player = new CharacterBuilder().Build();
    var npc = new CharacterBuilder().Build();
    var world = World.Create(DateTime.Now, [player, npc]);
    var save = GameSave.Create(player, world);

    var expectedGreeting = Greeting.Create("Hello");
    A.CallTo(() => _greetingRepository.GetAsync(A<GameContext>._, A<CancellationToken>._)).Returns(expectedGreeting);

    // Act
    var result = await _dialogueService.GetGreetingAsync(save, npc);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(expectedGreeting.Id, result.Id);
    A.CallTo(() => _greetingRepository.GetAsync(
        A<GameContext>.That.Matches(ctx => ctx.Actor == player && ctx.Target == npc && ctx.World == world),
        A<CancellationToken>._
      )
    ).MustHaveHappenedOnceExactly();
  }

  #endregion
}
