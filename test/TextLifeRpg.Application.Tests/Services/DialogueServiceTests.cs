using TextLifeRpg.Application.Abstraction.Repositories;
using TextLifeRpg.Application.Services;
using TextLifeRpg.Domain;
using TextLifeRpg.Domain.Tests.Helpers;

namespace TextLifeRpg.Application.Tests.Services;

public class DialogueServiceTests
{
  #region Fields

  private readonly IGreetingRepository _greetingRepository = A.Fake<IGreetingRepository>();
  private readonly IDialogueOptionRepository _dialogueOptionRepository = A.Fake<IDialogueOptionRepository>();

  private readonly IDialogueOptionSpokenTextRepository _dialogueOptionSpokenTextRepository =
    A.Fake<IDialogueOptionSpokenTextRepository>();

  private readonly IDialogueOptionResultRepository _dialogueOptionResultRepository =
    A.Fake<IDialogueOptionResultRepository>();

  private readonly DialogueService _dialogueService;

  #endregion

  #region Ctors

  public DialogueServiceTests()
  {
    _dialogueService = new DialogueService(
      _greetingRepository, _dialogueOptionRepository, _dialogueOptionSpokenTextRepository,
      _dialogueOptionResultRepository
    );
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
    save.StartDialogue(npc.Id);

    var expectedGreeting = Greeting.Create("Hello");
    A.CallTo(() => _greetingRepository.GetAsync(A<GameContext>._, A<CancellationToken>._)).Returns(expectedGreeting);

    // Act
    var result = await _dialogueService.GetGreetingAsync(save);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(expectedGreeting.Id, result.Id);
    A.CallTo(() => _greetingRepository.GetAsync(
        A<GameContext>.That.Matches(ctx => ctx.Actor == npc && ctx.Target == player && ctx.World == world),
        A<CancellationToken>._
      )
    ).MustHaveHappenedOnceExactly();
  }

  [Fact]
  public async Task GetPossibleDialogueOptionsAsync_ShouldCallRepositoryWithCorrectContextAndReturnOptions()
  {
    // Arrange
    var player = new CharacterBuilder().Build();
    var npc = new CharacterBuilder().Build();
    var world = World.Create(DateTime.Now, [player, npc]);
    var save = GameSave.Create(player, world);
    save.StartDialogue(npc.Id);

    var expectedOptions = new[]
    {
      DialogueOption.Create("Ask about the weather"),
      DialogueOption.Create("Say goodbye")
    };

    A.CallTo(() => _dialogueOptionRepository.GetPossibleDialogueOptionsAsync(A<GameContext>._, A<CancellationToken>._))
      .Returns(expectedOptions);

    // Act
    var result = await _dialogueService.GetPossibleDialogueOptionsAsync(save);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(expectedOptions.Length, result.Count);
    Assert.All(expectedOptions, option => Assert.Contains(result, r => r.Id == option.Id));

    A.CallTo(() => _dialogueOptionRepository.GetPossibleDialogueOptionsAsync(
        A<GameContext>.That.Matches(ctx => ctx.Actor == player && ctx.Target == npc && ctx.World == world),
        A<CancellationToken>._
      )
    ).MustHaveHappenedOnceExactly();
  }

  [Fact]
  public async Task GetGreetingAsync_ShouldThrowWhenInteractingNpcIsNull()
  {
    // Arrange
    var player = new CharacterBuilder().Build();
    var world = World.Create(DateTime.Now, [player]);
    var save = GameSave.Create(player, world); // InteractingNpc is null by default

    // Act & Assert
    var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _dialogueService.GetGreetingAsync(save));

    Assert.Equal("InteractingNpc shouldn't be null.", exception.Message);
  }

  [Fact]
  public async Task GetPossibleDialogueOptionsAsync_ShouldThrowWhenInteractingNpcIsNull()
  {
    // Arrange
    var player = new CharacterBuilder().Build();
    var world = World.Create(DateTime.Now, [player]);
    var save = GameSave.Create(player, world); // InteractingNpc is null by default

    // Act & Assert
    var exception =
      await Assert.ThrowsAsync<InvalidOperationException>(() => _dialogueService.GetPossibleDialogueOptionsAsync(save));

    Assert.Equal("InteractingNpc shouldn't be null.", exception.Message);
  }

  [Fact]
  public async Task ExecuteDialogueOptionAsync_ShouldThrowWhenInteractingNpcIsNull()
  {
    // Arrange
    var player = new CharacterBuilder().Build();
    var world = World.Create(DateTime.Now, [player]);
    var save = GameSave.Create(player, world); // InteractingNpc is null by default
    var dialogueOption = DialogueOption.Create("Ask something");

    // Act & Assert
    var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
      _dialogueService.ExecuteDialogueOptionAsync(dialogueOption, save, CancellationToken.None)
    );

    Assert.Equal("InteractingNpc shouldn't be null.", exception.Message);
  }

  #endregion
}
