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
    TextLineBuilder.BuildSpokenText(greeting.SpokenText, gameSave.InteractingNpc, gameSave.PlayerCharacter, gameSave);
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
  public async Task<IReadOnlyList<GameFlowStep>> BuildDialogueOptionStepsAsync(
    DialogueOption dialogueOption, GameSave gameSave, CancellationToken cancellationToken
  )
  {
    var steps = new List<GameFlowStep>();

    var player = gameSave.PlayerCharacter;
    var npc = gameSave.InteractingNpc ?? throw new InvalidOperationException("InteractingNpc shouldn't be null.");

    var context = new GameContext
    {
      Actor = player,
      World = gameSave.World,
      Target = npc
    };

    var spokenText =
      await dialogueOptionSpokenTextRepository.GetByDialogueOptionIdAsync(
        dialogueOption.Id, context, cancellationToken
      );
    steps.Add(
      new GameFlowStep
      {
        ExecuteAsync = async save =>
        {
          TextLineBuilder.BuildSpokenText(spokenText, player, npc, save);
          await Task.CompletedTask;
        }
      }
    );

    var result =
      await dialogueOptionResultRepository.GetByDialogueOptionIdAsync(dialogueOption.Id, context, cancellationToken);

    var resultSpokenText =
      await dialogueOptionResultSpokenTextRepository.GetByDialogueOptionResultIdAsync(
        result.Id, context, cancellationToken
      );
    if (resultSpokenText is not null)
    {
      steps.Add(
        new GameFlowStep
        {
          ExecuteAsync = async save =>
          {
            TextLineBuilder.BuildSpokenText(resultSpokenText, npc, player, save);
            await Task.CompletedTask;
          }
        }
      );
    }

    var resultNarration =
      await dialogueOptionResultNarrationRepository.GetByDialogueOptionResultIdAsync(
        result.Id, context, cancellationToken
      );
    if (resultNarration is not null)
    {
      steps.Add(
        new GameFlowStep
        {
          ExecuteAsync = async save =>
          {
            TextLineBuilder.BuildNarrationLine(resultNarration, player, npc, save);
            await Task.CompletedTask;
          }
        }
      );
    }

    if (result.EndDialogue)
    {
      steps.Add(
        new GameFlowStep
        {
          ExecuteAsync = async save =>
          {
            save.EndInteraction();
            await Task.CompletedTask;
          }
        }
      );
    }

    return steps;
  }

  #endregion
}
