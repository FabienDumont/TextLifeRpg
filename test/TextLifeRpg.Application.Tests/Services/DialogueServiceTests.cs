using TextLifeRpg.Application.Abstraction;
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

  private readonly IRandomProvider _randomProvider = A.Fake<IRandomProvider>();

  private readonly DialogueService _dialogueService;

  #endregion

  #region Ctors

  public DialogueServiceTests()
  {
    _dialogueService = new DialogueService(
      _greetingRepository, _dialogueOptionRepository, _dialogueOptionSpokenTextRepository,
      _dialogueOptionResultRepository, _dialogueOptionResultNarrationRepository,
      _dialogueOptionResultSpokenTextRepository, _randomProvider
    );
  }

  #endregion

  #region Methods

  [Fact]
  public async Task ExecuteGreetingAsync_ShouldCallGreetingRepositoryWithCorrectContext()
  {
    // Arrange
    var player = new CharacterBuilder().Build();
    var npc = new CharacterBuilder().Build();
    var world = World.Create(DateTime.Now, [player, npc]);
    var save = GameSave.Create(player, world);
    save.StartDialogue(npc.Id);

    A.CallTo(() => _greetingRepository.GetAsync(A<GameContext>._, A<CancellationToken>._))
      .Returns(new List<string> {"Hello"});

    // Act
    await _dialogueService.ExecuteGreetingAsync(save);

    // Assert
    Assert.Equal(2, world.Relationships.Count);
    A.CallTo(() => _greetingRepository.GetAsync(
        A<GameContext>.That.Matches(ctx => ctx.Actor == npc && ctx.Target == player && ctx.World == world),
        A<CancellationToken>._
      )
    ).MustHaveHappenedOnceExactly();
  }

  [Fact]
  public async Task ExecuteGreetingAsync_WithExistingRelationship_ShouldCallGreetingRepositoryWithCorrectContext()
  {
    // Arrange
    var player = new CharacterBuilder().Build();
    var npc = new CharacterBuilder().Build();
    var world = World.Create(DateTime.Now, [player, npc]);

    var date = DateOnly.FromDateTime(world.CurrentDate);

    world.AddRelationships(
      [
        Relationship.Create(player.Id, npc.Id, RelationshipType.Acquaintance, date, date, 0),
        Relationship.Create(npc.Id, player.Id, RelationshipType.Acquaintance, date, date, 0)
      ]
    );

    var save = GameSave.Create(player, world);
    save.StartDialogue(npc.Id);

    A.CallTo(() => _greetingRepository.GetAsync(A<GameContext>._, A<CancellationToken>._))
      .Returns(new List<string> {"Hello"});

    // Act
    await _dialogueService.ExecuteGreetingAsync(save);

    // Assert
    Assert.Equal(2, world.Relationships.Count);
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
  public async Task ExecuteGreetingAsync_ShouldThrowWhenNoGreetingsExist()
  {
    // Arrange
    var player = new CharacterBuilder().Build();
    var npc = new CharacterBuilder().Build();
    var world = World.Create(DateTime.Now, [player, npc]);
    var save = GameSave.Create(player, world);
    save.StartDialogue(npc.Id);

    A.CallTo(() => _greetingRepository.GetAsync(A<GameContext>._, A<CancellationToken>._)).Returns(new List<string>());

    // Act & Assert
    var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
      _dialogueService.ExecuteGreetingAsync(save)
    );

    Assert.Equal("No greetings found.", exception.Message);
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

    A.CallTo(() =>
      _dialogueOptionRepository.GetPossibleInitialDialogueOptionsAsync(A<GameContext>._, A<CancellationToken>._)
    ).Returns(expectedOptions);

    // Act
    var result = await _dialogueService.GetPossibleDialogueOptionsAsync(save);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(expectedOptions.Length, result.Count);
    Assert.All(expectedOptions, option => Assert.Contains(result, r => r.Id == option.Id));

    A.CallTo(() => _dialogueOptionRepository.GetPossibleInitialDialogueOptionsAsync(
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
      _dialogueService.BuildDialogueOptionStepsAsync(dialogueOption, save, CancellationToken.None)
    );

    Assert.Equal("InteractingNpc shouldn't be null.", exception.Message);
  }

  [Fact]
  public async Task BuildDialogueOptionStepsAsync_ShouldBuildFullFlow_WhenAllPartsArePresent()
  {
    // Arrange
    var player = new CharacterBuilder().WithName("Player").WithSex(BiologicalSex.Male).Build();
    var npc = new CharacterBuilder().WithName("NPC").WithSex(BiologicalSex.Female).Build();
    var world = World.Create(DateTime.Now, [player, npc]);

    var date = DateOnly.FromDateTime(world.CurrentDate);

    var relationshipPlayer = Relationship.Create(player.Id, npc.Id, RelationshipType.Acquaintance, date, date, 0);
    var relationshipNpc = Relationship.Create(npc.Id, player.Id, RelationshipType.Acquaintance, date, date, 0);

    world.AddRelationships(
      [
        relationshipPlayer,
        relationshipNpc
      ]
    );

    var save = GameSave.Create(player, world);
    save.StartDialogue(npc.Id);

    var dialogueOption = DialogueOption.Create("Ask something");
    var result = DialogueOptionResult.Create(Guid.NewGuid(), 5, endDialogue: true);

    const string playerSpokenText = "What happened here?";
    const string npcSpokenText = "I don't want to talk about it.";
    const string narrationText = "[TARGETNAME] avoids your gaze.";

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
    ).Returns(new List<string> {npcSpokenText});

    A.CallTo(() => _dialogueOptionResultNarrationRepository.GetByDialogueOptionResultIdAsync(
        result.Id, A<GameContext>._, A<CancellationToken>._
      )
    ).Returns(narrationText);

    // Act
    var steps = await _dialogueService.BuildDialogueOptionStepsAsync(dialogueOption, save, CancellationToken.None);

    foreach (var step in steps)
    {
      await step.ExecuteAsync(save);
    }

    // Assert
    Assert.Equal(3, save.TextLines.Count);
    Assert.Equal(3, steps.Count(s => s.Delay));

    var playerLine = save.TextLines[0];
    Assert.Equal("Player", playerLine.TextParts[0].Text);
    Assert.Equal(" : What happened here?", playerLine.TextParts[1].Text);

    var npcLine = save.TextLines[1];
    Assert.Equal("NPC", npcLine.TextParts[0].Text);
    Assert.Equal(" : I don't want to talk about it.", npcLine.TextParts[1].Text);

    var narrationLine = save.TextLines[2];
    var expectedNarration = $"{npc.Name} avoids your gaze.";
    var actualNarration = string.Concat(narrationLine.TextParts.Select(p => p.Text));
    Assert.Equal(expectedNarration, actualNarration);

    Assert.Null(save.InteractingNpc);
    Assert.Null(save.NpcInteractionType);

    Assert.Equal(5, relationshipNpc.Value);
  }

  [Fact]
  public async Task BuildDialogueOptionStepsAsync_ShouldEndInteraction_WhenResultEndsDialogue()
  {
    // Arrange
    var player = new CharacterBuilder().Build();
    var npc = new CharacterBuilder().Build();
    var world = World.Create(DateTime.Now, [player, npc]);
    var save = GameSave.Create(player, world);
    save.StartDialogue(npc.Id);

    var dialogueOption = DialogueOption.Create("Say goodbye");
    var result = DialogueOptionResult.Create(Guid.NewGuid(), null, endDialogue: true);

    A.CallTo(() => _dialogueOptionSpokenTextRepository.GetByDialogueOptionIdAsync(
        dialogueOption.Id, A<GameContext>._, A<CancellationToken>._
      )
    ).Returns(Task.FromResult<string?>("Goodbye."));

    A.CallTo(() => _dialogueOptionResultRepository.GetByDialogueOptionIdAsync(
        dialogueOption.Id, A<GameContext>._, A<CancellationToken>._
      )
    ).Returns(result);

    A.CallTo(() => _dialogueOptionResultSpokenTextRepository.GetByDialogueOptionResultIdAsync(
        result.Id, A<GameContext>._, A<CancellationToken>._
      )
    ).Returns(new List<string>());

    A.CallTo(() => _dialogueOptionResultNarrationRepository.GetByDialogueOptionResultIdAsync(
        result.Id, A<GameContext>._, A<CancellationToken>._
      )
    ).Returns(null as string);

    // Act
    var steps = await _dialogueService.BuildDialogueOptionStepsAsync(dialogueOption, save, CancellationToken.None);

    foreach (var step in steps)
    {
      await step.ExecuteAsync(save);
    }

    // Assert
    Assert.Null(save.InteractingNpc);
    Assert.Null(save.NpcInteractionType);
  }

  [Fact]
  public async Task BuildDialogueOptionStepsAsync_ShouldPopulatePendingFollowUps_WhenFollowUpsExist()
  {
    // Arrange
    var player = new CharacterBuilder().WithName("Player").Build();
    var npc = new CharacterBuilder().WithName("NPC").Build();
    var world = World.Create(DateTime.Now, [player, npc]);
    var save = GameSave.Create(player, world);
    save.StartDialogue(npc.Id);

    // Add stale pending options to ensure the service clears them first
    save.PendingDialogueOptions.Add(DialogueOption.Create("stale"));

    var dialogueOption = DialogueOption.Create("Ask something");
    var result = DialogueOptionResult.Create(Guid.NewGuid(), null, endDialogue: false);

    // No player spoken text / npc reply / narration needed for this test
    A.CallTo(() => _dialogueOptionSpokenTextRepository.GetByDialogueOptionIdAsync(
        dialogueOption.Id, A<GameContext>._, A<CancellationToken>._
      )
    ).Returns(null as string);

    A.CallTo(() => _dialogueOptionResultRepository.GetByDialogueOptionIdAsync(
        dialogueOption.Id, A<GameContext>._, A<CancellationToken>._
      )
    ).Returns(result);

    A.CallTo(() => _dialogueOptionResultSpokenTextRepository.GetByDialogueOptionResultIdAsync(
        result.Id, A<GameContext>._, A<CancellationToken>._
      )
    ).Returns(new List<string>());

    A.CallTo(() => _dialogueOptionResultNarrationRepository.GetByDialogueOptionResultIdAsync(
        result.Id, A<GameContext>._, A<CancellationToken>._
      )
    ).Returns(null as string);

    var followUps = new[]
    {
      DialogueOption.Create("Ask about job"),
      DialogueOption.Create("Nevermind")
    };

    A.CallTo(() => _dialogueOptionRepository.GetPossibleFollowUpsAsync(
        A<GameContext>._, result.Id, A<CancellationToken>._
      )
    ).Returns(followUps);

    // Act
    var steps = await _dialogueService.BuildDialogueOptionStepsAsync(dialogueOption, save, CancellationToken.None);
    foreach (var step in steps)
    {
      await step.ExecuteAsync(save);
    }

    // Assert: follow-ups were set, old ones cleared, interaction stays open
    Assert.Equal(2, save.PendingDialogueOptions.Count);
    Assert.Contains(save.PendingDialogueOptions, o => o.Label == "Ask about job");
    Assert.Contains(save.PendingDialogueOptions, o => o.Label == "Nevermind");
    Assert.NotNull(save.InteractingNpc);
    Assert.Equal(NpcInteractionType.Dialogue, save.NpcInteractionType);

    // Also verify correct context was used to fetch follow-ups
    A.CallTo(() => _dialogueOptionRepository.GetPossibleFollowUpsAsync(
        A<GameContext>.That.Matches(ctx => ctx.Actor == player && ctx.Target == npc && ctx.World == world), result.Id,
        A<CancellationToken>._
      )
    ).MustHaveHappenedOnceExactly();
  }

  #endregion
}
