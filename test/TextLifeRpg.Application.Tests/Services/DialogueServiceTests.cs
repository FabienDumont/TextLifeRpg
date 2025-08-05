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

  private readonly IDialogueOptionResultNarrationRepository _dialogueOptionResultNarrationRepository =
    A.Fake<IDialogueOptionResultNarrationRepository>();

  private readonly IDialogueOptionResultSpokenTextRepository _dialogueOptionResultSpokenTextRepository =
    A.Fake<IDialogueOptionResultSpokenTextRepository>();

  private readonly DialogueService _dialogueService;

  #endregion

  #region Ctors

  public DialogueServiceTests()
  {
    _dialogueService = new DialogueService(
      _greetingRepository, _dialogueOptionRepository, _dialogueOptionSpokenTextRepository,
      _dialogueOptionResultRepository, _dialogueOptionResultNarrationRepository,
      _dialogueOptionResultSpokenTextRepository
    );
  }

  #endregion

  #region Methods

  [Fact]
  public async Task ExecuteGreetingAsync_ShouldCallGreetingRepositoryWithCorrectContextAndReturnGreeting()
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
    await _dialogueService.ExecuteGreetingAsync(save);

    // Assert
    A.CallTo(() => _greetingRepository.GetAsync(
        A<GameContext>.That.Matches(ctx => ctx.Actor == npc && ctx.Target == player && ctx.World == world),
        A<CancellationToken>._
      )
    ).MustHaveHappenedOnceExactly();
  }

  [Fact]
  public async Task ExecuteGreetingAsync_ShouldThrowWhenInteractingNpcIsNull()
  {
    // Arrange
    var player = new CharacterBuilder().Build();
    var world = World.Create(DateTime.Now, [player]);
    var save = GameSave.Create(player, world); // InteractingNpc is null by default

    // Act & Assert
    var exception =
      await Assert.ThrowsAsync<InvalidOperationException>(() => _dialogueService.ExecuteGreetingAsync(save));

    Assert.Equal("InteractingNpc shouldn't be null.", exception.Message);
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

  [Fact]
  public async Task ExecuteDialogueOptionAsync_ShouldBuildFullDialogueFlow_WhenDataIsComplete()
  {
    // Arrange
    var player = new CharacterBuilder().WithName("Player").WithSex(BiologicalSex.Male).Build();
    var npc = new CharacterBuilder().WithName("NPC").WithSex(BiologicalSex.Female).Build();
    var world = World.Create(DateTime.Now, [player, npc]);
    var save = GameSave.Create(player, world);
    save.StartDialogue(npc.Id);

    var dialogueOption = DialogueOption.Create("Ask something");
    var result = DialogueOptionResult.Create(Guid.NewGuid(), endDialogue: false);

    const string playerSpokenText = "Where are you from?";
    const string npcSpokenText = "I'm from the old city.";
    const string narrationText = "[TARGETNAME] looks away, uneasy.";

    A.CallTo(() => _dialogueOptionSpokenTextRepository.GetByDialogueOptionIdAsync(
        dialogueOption.Id, A<GameContext>._, A<CancellationToken>._
      )
    ).Returns(playerSpokenText);

    A.CallTo(() => _dialogueOptionResultRepository.GetByDialogueOptionIdAsync(
        dialogueOption.Id, A<GameContext>._, A<CancellationToken>._
      )
    ).Returns(result);

    A.CallTo(() => _dialogueOptionResultSpokenTextRepository.GetByDialogueOptionResultIdAsync(
        result.Id, A<GameContext>._, A<CancellationToken>._
      )
    ).Returns(npcSpokenText);

    A.CallTo(() => _dialogueOptionResultNarrationRepository.GetByDialogueOptionResultIdAsync(
        result.Id, A<GameContext>._, A<CancellationToken>._
      )
    ).Returns(narrationText);

    // Act
    await _dialogueService.ExecuteDialogueOptionAsync(dialogueOption, save, CancellationToken.None);

    // Assert
    Assert.Equal(3, save.TextLines.Count);

    var playerLine = save.TextLines[0];
    Assert.Equal("Player", playerLine.TextParts[0].Text);
    Assert.Equal(CharacterColor.Yellow, playerLine.TextParts[0].Color);
    Assert.Equal(" : Where are you from?", playerLine.TextParts[1].Text);
    Assert.Null(playerLine.TextParts[1].Color);

    var npcLine = save.TextLines[1];
    Assert.Equal("NPC", npcLine.TextParts[0].Text);
    Assert.Equal(CharacterColor.Pink, npcLine.TextParts[0].Color);
    Assert.Equal(" : I'm from the old city.", npcLine.TextParts[1].Text);
    Assert.Null(npcLine.TextParts[1].Color);

    var narrationLine = save.TextLines[2];
    Assert.Equal(
      "[TARGETNAME] looks away, uneasy.".Replace("[TARGETNAME]", npc.Name),
      string.Concat(narrationLine.TextParts.Select(p => p.Text))
    );
  }

  [Fact]
  public async Task ExecuteDialogueOptionAsync_ShouldEndInteraction_WhenResultEndsDialogue()
  {
    // Arrange
    var player = new CharacterBuilder().WithName("Player").Build();
    var npc = new CharacterBuilder().WithName("NPC").Build();
    var world = World.Create(DateTime.Now, [player, npc]);
    var save = GameSave.Create(player, world);
    save.StartDialogue(npc.Id);

    var dialogueOption = DialogueOption.Create("Say goodbye");

    A.CallTo(() => _dialogueOptionSpokenTextRepository.GetByDialogueOptionIdAsync(
        dialogueOption.Id, A<GameContext>._, A<CancellationToken>._
      )
    ).Returns("Bye!");

    var result = DialogueOptionResult.Create(Guid.NewGuid(), endDialogue: true);
    A.CallTo(() => _dialogueOptionResultRepository.GetByDialogueOptionIdAsync(
        dialogueOption.Id, A<GameContext>._, A<CancellationToken>._
      )
    ).Returns(result);

    A.CallTo(() =>
      _dialogueOptionResultSpokenTextRepository.GetByDialogueOptionResultIdAsync(
        result.Id, A<GameContext>._, A<CancellationToken>._
      )
    ).Returns(null as string);

    A.CallTo(() =>
      _dialogueOptionResultNarrationRepository.GetByDialogueOptionResultIdAsync(
        result.Id, A<GameContext>._, A<CancellationToken>._
      )
    ).Returns(null as string);

    // Act
    await _dialogueService.ExecuteDialogueOptionAsync(dialogueOption, save, CancellationToken.None);

    // Assert
    Assert.Null(save.NpcInteractionType);
    Assert.Null(save.InteractingNpc);
  }

  #endregion
}
