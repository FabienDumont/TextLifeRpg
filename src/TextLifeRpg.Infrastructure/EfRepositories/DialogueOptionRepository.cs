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
  public async Task<IReadOnlyCollection<DialogueOption>> GetPossibleInitialDialogueOptionsAsync(
    GameContext gameContext, CancellationToken cancellationToken
  )
  {
    var allDialogueOptions = await Context.DialogueOptions
      .Where(o => !Context.DialogueOptionResultNextDialogueOptions.Any(l => l.NextDialogueOptionId == o.Id))
      .ToListAsync(cancellationToken);

    var allConditions = await Context.Conditions
      .Where(c => c.ContextType == ContextType.DialogueOption &&
                  allDialogueOptions.Select(o => o.Id).ToHashSet().Contains(c.ContextId)
      ).ToListAsync(cancellationToken);

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

  /// <inheritdoc />
  public async Task<IReadOnlyCollection<DialogueOption>> GetPossibleFollowUpsAsync(
    GameContext gameContext, Guid dialogueOptionResultId, CancellationToken ct
  )
  {
    var candidateIds = await Context.DialogueOptionResultNextDialogueOptions
      .Where(l => l.DialogueOptionResultId == dialogueOptionResultId).OrderBy(l => l.Order)
      .Select(l => l.NextDialogueOptionId).ToListAsync(ct);

    if (candidateIds.Count == 0) return [];

    var options = await Context.DialogueOptions.AsNoTracking().Where(o => candidateIds.Contains(o.Id)).ToListAsync(ct);

    var conditions = await Context.Conditions.AsNoTracking()
      .Where(c => c.ContextType == ContextType.DialogueOption && candidateIds.Contains(c.ContextId)).ToListAsync(ct);

    var conds = conditions.ToLookup(c => c.ContextId);

    var list = new List<DialogueOption>(options.Count);
    foreach (var o in options.OrderBy(o => candidateIds.IndexOf(o.Id)))
    {
      var ok = conds[o.Id].All(c => ConditionEvaluator.EvaluateCondition(c, gameContext));

      if (ok) list.Add(o.ToDomain());
    }

    return new ReadOnlyCollection<DialogueOption>(list);
  }

  #endregion
}
