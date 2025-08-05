using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using TextLifeRpg.Application.Abstraction.Repositories;
using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.Helper;
using TextLifeRpg.Infrastructure.Mappers;

namespace TextLifeRpg.Infrastructure.EfRepositories;

/// <summary>
/// EF Repository for dialogue options.
/// </summary>
public class DialogueOptionRepository(ApplicationContext context) : RepositoryBase(context), IDialogueOptionRepository
{
  #region Implementation of IDialogueOptionRepository

  /// <inheritdoc />
  public async Task<IReadOnlyCollection<DialogueOption>> GetPossibleDialogueOptionsAsync(
    GameContext gameContext, CancellationToken cancellationToken
  )
  {
    var allDialogueOptions = await Context.DialogueOptions.ToListAsync(cancellationToken);

    var allConditions = await Context.Conditions.Where(c => c.ContextType == ContextType.DialogueOption)
      .ToListAsync(cancellationToken);

    List<DialogueOption> list = [];

    foreach (var dialogueOption in allDialogueOptions)
    {
      var conditions = allConditions.Where(c => c.ContextId == dialogueOption.Id);

      var allSatisfied = conditions.All(condition => ConditionEvaluator.EvaluateCondition(condition, gameContext));

      if (allSatisfied)
      {
        list.Add(dialogueOption.ToDomain());
      }
    }

    if (list.Count == 0)
    {
      throw new InvalidOperationException("No appropriate dialogue option found.");
    }

    return new ReadOnlyCollection<DialogueOption>(list);
  }

  #endregion
}
