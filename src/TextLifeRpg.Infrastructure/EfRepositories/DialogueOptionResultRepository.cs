using Microsoft.EntityFrameworkCore;
using TextLifeRpg.Application.Abstraction.Repositories;
using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.Helper;
using TextLifeRpg.Infrastructure.Mappers;

namespace TextLifeRpg.Infrastructure.EfRepositories;

/// <summary>
/// Repository for dialogue option results.
/// </summary>
public class DialogueOptionResultRepository(ApplicationContext context)
  : RepositoryBase(context), IDialogueOptionResultRepository
{
  #region Implementation of IDialogueOptionResultRepository

  /// <inheritdoc />
  public async Task<DialogueOptionResult> GetByDialogueOptionIdAsync(
    Guid dialogueOptionId, GameContext gameContext, CancellationToken cancellationToken
  )
  {
    var results = await Context.DialogueOptionResults.Where(n => n.DialogueOptionId == dialogueOptionId)
      .ToListAsync(cancellationToken);

    var narrationIds = results.Select(n => n.Id).ToList();
    var allConditions = await Context.Conditions
      .Where(c => c.ContextType == ContextType.DialogueOptionResult && narrationIds.Contains(c.ContextId))
      .ToListAsync(cancellationToken);

    foreach (var dialogueOptionResult in results)
    {
      var conditions = allConditions.Where(c => c.ContextId == dialogueOptionResult.Id);

      var allSatisfied = conditions.All(condition => ConditionEvaluator.EvaluateCondition(condition, gameContext));

      if (allSatisfied)
      {
        return dialogueOptionResult.ToDomain();
      }
    }

    throw new InvalidOperationException(
      $"No appropriate dialogue option result found for dialogue option {dialogueOptionId}."
    );
  }

  #endregion
}
