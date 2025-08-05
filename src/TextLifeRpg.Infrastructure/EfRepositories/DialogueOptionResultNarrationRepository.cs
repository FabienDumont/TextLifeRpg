using Microsoft.EntityFrameworkCore;
using TextLifeRpg.Application.Abstraction.Repositories;
using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.Helper;

namespace TextLifeRpg.Infrastructure.EfRepositories;

public class DialogueOptionResultNarrationRepository(ApplicationContext context)
  : RepositoryBase(context), IDialogueOptionResultNarrationRepository
{
  #region Implementation of IDialogueOptionResultNarrationRepository

  public async Task<string?> GetByDialogueOptionResultIdAsync(
    Guid dialogueOptionResultId, GameContext gameContext, CancellationToken cancellationToken
  )
  {
    var narrations = await Context.DialogueOptionResultNarrations
      .Where(n => n.DialogueOptionResultId == dialogueOptionResultId).ToListAsync(cancellationToken);

    var narrationIds = narrations.Select(n => n.Id).ToList();
    var allConditions = await Context.Conditions
      .Where(c => c.ContextType == ContextType.DialogueOptionResultNarration && narrationIds.Contains(c.ContextId))
      .ToListAsync(cancellationToken);

    foreach (var dialogueOptionResultNarration in narrations)
    {
      var conditions = allConditions.Where(c => c.ContextId == dialogueOptionResultNarration.Id);

      var allSatisfied = conditions.All(condition => ConditionEvaluator.EvaluateCondition(condition, gameContext));

      if (allSatisfied)
      {
        return dialogueOptionResultNarration.Text;
      }
    }

    return null;
  }

  #endregion
}
