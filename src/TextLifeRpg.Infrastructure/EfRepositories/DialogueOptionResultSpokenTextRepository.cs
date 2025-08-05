using Microsoft.EntityFrameworkCore;
using TextLifeRpg.Application.Abstraction.Repositories;
using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.Helper;

namespace TextLifeRpg.Infrastructure.EfRepositories;

public class DialogueOptionResultSpokenTextRepository(ApplicationContext context)
  : RepositoryBase(context), IDialogueOptionResultSpokenTextRepository
{
  #region Implementation of IDialogueOptionResultSpokenTextRepository

  public async Task<string?> GetByDialogueOptionResultIdAsync(
    Guid dialogueOptionResultId, GameContext gameContext, CancellationToken cancellationToken
  )
  {
    var spokenTexts = await Context.DialogueOptionResultSpokenTexts
      .Where(n => n.DialogueOptionResultId == dialogueOptionResultId).ToListAsync(cancellationToken);

    var spokenTextIds = spokenTexts.Select(n => n.Id).ToList();
    var allConditions = await Context.Conditions
      .Where(c => c.ContextType == ContextType.DialogueOptionResultSpokenText && spokenTextIds.Contains(c.ContextId))
      .ToListAsync(cancellationToken);

    foreach (var dialogueOptionResultSpokenText in spokenTexts)
    {
      var conditions = allConditions.Where(c => c.ContextId == dialogueOptionResultSpokenText.Id);

      var allSatisfied = conditions.All(condition => ConditionEvaluator.EvaluateCondition(condition, gameContext));

      if (allSatisfied)
      {
        return dialogueOptionResultSpokenText.Text;
      }
    }

    return null;
  }

  #endregion
}
