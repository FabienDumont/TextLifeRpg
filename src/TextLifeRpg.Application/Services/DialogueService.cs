using TextLifeRpg.Application.Abstraction;
using TextLifeRpg.Application.Abstraction.Repositories;
using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Services;

public class DialogueService(
  IGreetingRepository greetingRepository, IDialogueOptionRepository dialogueOptionRepository,
  IDialogueOptionSpokenTextRepository dialogueOptionSpokenTextRepository,
  IDialogueOptionResultRepository dialogueOptionResultRepository,
  IDialogueOptionResultNarrationRepository dialogueOptionResultNarrationRepository,
  IDialogueOptionResultSpokenTextRepository dialogueOptionResultSpokenTextRepository
) : IDialogueService
{
  #region Implementation of IDialogueService

  /// <inheritdoc />
  public async Task ExecuteGreetingAsync(GameSave gameSave, CancellationToken cancellationToken = default)
  {
    var context = new GameContext
    {
      Actor = gameSave.InteractingNpc ??
              throw new InvalidOperationException($"{nameof(gameSave.InteractingNpc)} shouldn't be null."),
      World = gameSave.World,
      Target = gameSave.PlayerCharacter
    };

    var greeting = await greetingRepository.GetAsync(context, cancellationToken);

    gameSave.TextLines.Clear();
    var textParts = new List<TextPart>(
      [
        new TextPart(
          CharacterColorHelper.GetColorKey(gameSave.InteractingNpc, gameSave.PlayerCharacterId),
          $"{gameSave.InteractingNpc.Name}"
        ),
        new TextPart(null, $" : {greeting.SpokenText}")
      ]
    );
    gameSave.AddText(textParts);
  }

  /// <inheritdoc />
  public async Task<IReadOnlyCollection<DialogueOption>> GetPossibleDialogueOptionsAsync(
    GameSave gameSave, CancellationToken cancellationToken = default
  )
  {
    var context = new GameContext
    {
      Actor = gameSave.PlayerCharacter,
      World = gameSave.World,
      Target = gameSave.InteractingNpc ??
               throw new InvalidOperationException($"{nameof(gameSave.InteractingNpc)} shouldn't be null.")
    };

    return await dialogueOptionRepository.GetPossibleDialogueOptionsAsync(context, cancellationToken);
  }

  /// <inheritdoc />
  public async Task ExecuteDialogueOptionAsync(
    DialogueOption dialogueOption, GameSave gameSave, CancellationToken cancellationToken
  )
  {
    var player = gameSave.PlayerCharacter;
    var npc = gameSave.InteractingNpc ??
              throw new InvalidOperationException($"{nameof(gameSave.InteractingNpc)} shouldn't be null.");
    var context = new GameContext
    {
      Actor = player,
      World = gameSave.World,
      Target = npc
    };

    var spokenText = await dialogueOptionSpokenTextRepository.GetByDialogueOptionIdAsync(
      dialogueOption.Id, context, cancellationToken
    );

    var textPartsSpokenText = new List<TextPart>(
      [
        new TextPart(CharacterColorHelper.GetColorKey(player, player.Id), $"{player.Name}"),
        new TextPart(null, $" : {spokenText}")
      ]
    );

    gameSave.AddText(textPartsSpokenText);

    var result = await dialogueOptionResultRepository.GetByDialogueOptionIdAsync(
      dialogueOption.Id, context, cancellationToken
    );

    var resultSpokenText = await dialogueOptionResultSpokenTextRepository.GetByDialogueOptionResultIdAsync(
      result.Id, context, cancellationToken
    );

    if (resultSpokenText is not null)
    {
      var textPartsResultSpokenText = new List<TextPart>(
        [
          new TextPart(CharacterColorHelper.GetColorKey(npc, player.Id), $"{npc.Name}"),
          new TextPart(null, $" : {resultSpokenText}")
        ]
      );

      gameSave.AddText(textPartsResultSpokenText);
    }

    var resultNarration = await dialogueOptionResultNarrationRepository.GetByDialogueOptionResultIdAsync(
      result.Id, context, cancellationToken
    );

    if (resultNarration is not null)
    {
      TextLineBuilder.BuildNarrationLine(resultNarration, player, npc, gameSave);
    }

    if (result.EndDialogue)
    {
      gameSave.EndInteraction();
    }
  }

  #endregion
}
