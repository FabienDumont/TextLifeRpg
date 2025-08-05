using TextLifeRpg.Application.Abstraction;
using TextLifeRpg.Application.Abstraction.Repositories;
using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Services;

public class DialogueService(
  IGreetingRepository greetingRepository, IDialogueOptionRepository dialogueOptionRepository,
  IDialogueOptionSpokenTextRepository dialogueOptionSpokenTextRepositor,
  IDialogueOptionResultRepository dialogueOptionResultRepository
) : IDialogueService
{
  #region Implementation of IDialogueService

  /// <inheritdoc />
  public async Task<Greeting> GetGreetingAsync(GameSave gameSave, CancellationToken cancellationToken = default)
  {
    var context = new GameContext
    {
      Actor = gameSave.InteractingNpc ??
              throw new InvalidOperationException($"{nameof(gameSave.InteractingNpc)} shouldn't be null."),
      World = gameSave.World,
      Target = gameSave.PlayerCharacter
    };

    return await greetingRepository.GetAsync(context, cancellationToken);
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
    var context = new GameContext
    {
      Actor = gameSave.PlayerCharacter,
      World = gameSave.World,
      Target = gameSave.InteractingNpc ??
               throw new InvalidOperationException($"{nameof(gameSave.InteractingNpc)} shouldn't be null.")
    };

    var spokenText = await dialogueOptionSpokenTextRepositor.GetByDialogueOptionIdAsync(
      dialogueOption.Id, context, cancellationToken
    );

    var line = TextLineBuilder.BuildNarrationLine(spokenText, gameSave.PlayerCharacter, gameSave.PlayerCharacterId);

    gameSave.AddText(line.TextParts);

    var result = await dialogueOptionResultRepository.GetByDialogueOptionIdAsync(
      dialogueOption.Id, context, cancellationToken
    );

    if (result.EndDialogue)
    {
      gameSave.EndInteraction();
    }
  }

  #endregion
}
