using TextLifeRpg.Application.Abstraction;
using TextLifeRpg.Application.Abstraction.Repositories;
using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Services;

public class DialogueService(
  IGreetingRepository greetingRepository, IDialogueOptionRepository dialogueOptionRepository,
  IDialogueOptionSpokenTextRepository dialogueOptionSpokenTextRepository,
  IDialogueOptionResultRepository dialogueOptionResultRepository,
  IDialogueOptionResultNarrationRepository dialogueOptionResultNarrationRepository,
  IDialogueOptionResultSpokenTextRepository dialogueOptionResultSpokenTextRepository, IRandomProvider randomProvider
) : IDialogueService
{
  #region Implementation of IDialogueService

  /// <inheritdoc />
  public async Task ExecuteGreetingAsync(GameSave gameSave, CancellationToken cancellationToken = default)
  {
    var player = gameSave.PlayerCharacter;
    var npc = gameSave.InteractingNpc ??
              throw new InvalidOperationException($"{nameof(gameSave.InteractingNpc)} shouldn't be null.");

    var context = new GameContext
    {
      Actor = npc,
      World = gameSave.World,
      Target = player
    };

    var possibleGreetings = await greetingRepository.GetAsync(context, cancellationToken);

    if (possibleGreetings.Count == 0)
    {
      throw new InvalidOperationException("No greetings found.");
    }

    gameSave.TextLines.Clear();
    TextLineBuilder.BuildSpokenText(
      possibleGreetings[randomProvider.Next(0, possibleGreetings.Count)], npc, player, gameSave
    );

    var hasBothRelationships =
      gameSave.World.Relationships.Any(r => r.SourceCharacterId == player.Id && r.TargetCharacterId == npc.Id) &&
      gameSave.World.Relationships.Any(r => r.SourceCharacterId == npc.Id && r.TargetCharacterId == player.Id);

    if (!hasBothRelationships)
    {
      var date = DateOnly.FromDateTime(gameSave.World.CurrentDate);
      gameSave.World.AddRelationships(
        [
          Relationship.Create(player.Id, npc.Id, RelationshipType.Acquaintance, date, date, 0),
          Relationship.Create(npc.Id, player.Id, RelationshipType.Acquaintance, date, date, 0)
        ]
      );
    }
  }

  /// <inheritdoc />
  public async Task<IReadOnlyCollection<DialogueOption>> GetPossibleDialogueOptionsAsync(
    GameSave gameSave, CancellationToken cancellationToken = default
  )
  {
    var player = gameSave.PlayerCharacter;
    var npc = gameSave.InteractingNpc ?? throw new InvalidOperationException("InteractingNpc shouldn't be null.");

    var context = new GameContext
    {
      Actor = player,
      World = gameSave.World,
      Target = npc
    };

    return await dialogueOptionRepository.GetPossibleInitialDialogueOptionsAsync(context, cancellationToken);
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

    gameSave.ClearPendingDialogueOptions();

    var spokenText =
      await dialogueOptionSpokenTextRepository.GetByDialogueOptionIdAsync(
        dialogueOption.Id, context, cancellationToken
      );

    if (spokenText is not null)
    {
      steps.Add(
        new GameFlowStep
        {
          Delay = true,
          ExecuteAsync = async save =>
          {
            TextLineBuilder.BuildSpokenText(spokenText, player, npc, save);
            await Task.CompletedTask;
          }
        }
      );
    }

    var result =
      await dialogueOptionResultRepository.GetByDialogueOptionIdAsync(dialogueOption.Id, context, cancellationToken);

    var dialogueResultSpokenTextContext = new GameContext
    {
      Actor = npc,
      World = gameSave.World,
      Target = player
    };

    var possibleSpokenTexts = await dialogueOptionResultSpokenTextRepository.GetByDialogueOptionResultIdAsync(
      result.Id, dialogueResultSpokenTextContext, cancellationToken
    );

    if (possibleSpokenTexts.Count > 0)
    {
      steps.Add(
        new GameFlowStep
        {
          Delay = true,
          ExecuteAsync = async save =>
          {
            TextLineBuilder.BuildSpokenText(
              possibleSpokenTexts[randomProvider.Next(0, possibleSpokenTexts.Count)], npc, player, save
            );
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
          Delay = true,
          ExecuteAsync = async save =>
          {
            TextLineBuilder.BuildNarrationLine(resultNarration, player, npc, save);
            await Task.CompletedTask;
          }
        }
      );
    }

    if (result.TargetRelationshipValueChange is not null)
    {
      steps.Add(
        new GameFlowStep
        {
          ExecuteAsync = async save =>
          {
            var relationship = save.World.Relationships.FirstOrDefault(r =>
              r.SourceCharacterId == npc.Id && r.TargetCharacterId == player.Id
            );

            relationship?.AdjustValue(
              result.TargetRelationshipValueChange.Value, DateOnly.FromDateTime(save.World.CurrentDate)
            );

            await Task.CompletedTask;
          }
        }
      );
    }

    if (result.ActorLearnFact is not null)
    {
      steps.Add(
        new GameFlowStep
        {
          ExecuteAsync = async save =>
          {
            var relationship = save.World.Relationships.FirstOrDefault(r =>
              r.SourceCharacterId == player.Id && r.TargetCharacterId == npc.Id
            );

            relationship?.History.LearnFact(result.ActorLearnFact);

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
            save.ClearPendingDialogueOptions();
            save.EndInteraction();
            await Task.CompletedTask;
          }
        }
      );
    }
    else
    {
      var followUps = await dialogueOptionRepository.GetPossibleFollowUpsAsync(context, result.Id, cancellationToken);

      if (followUps.Count != 0)
      {
        steps.Add(
          new GameFlowStep
          {
            ExecuteAsync = async save =>
            {
              save.PendingDialogueOptions.AddRange(followUps);
              await Task.CompletedTask;
            }
          }
        );
      }
    }

    return steps;
  }

  #endregion
}
