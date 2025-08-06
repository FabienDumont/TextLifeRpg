using Microsoft.EntityFrameworkCore;
using TextLifeRpg.Application.Abstraction.Repositories;
using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.Helper;

namespace TextLifeRpg.Infrastructure.EfRepositories;

public class DialogueOptionSpokenTextRepository(ApplicationContext context)
  : RepositoryBase(context), IDialogueOptionSpokenTextRepository
{
  #region Implementation of IDialogueOptionSpokenTextRepository

  public async Task<string> GetByDialogueOptionIdAsync(
    Guid dialogueOptionId, GameContext gameContext, CancellationToken cancellationToken
  )
  {
    var spokenTexts = await Context.DialogueOptionSpokenTexts.Where(n => n.DialogueOptionId == dialogueOptionId)
      .ToListAsync(cancellationToken);

    var spokenTextIds = spokenTexts.Select(n => n.Id).ToList();
    var allConditions = await Context.Conditions
      .Where(c => c.ContextType == ContextType.DialogueOptionSpokenText && spokenTextIds.Contains(c.ContextId))
      .ToListAsync(cancellationToken);

    foreach (var spokenText in spokenTexts)
    {
      var conditions = allConditions.Where(c => c.ContextId == spokenText.Id);

      var allSatisfied = conditions.All(condition => ConditionEvaluator.EvaluateCondition(condition, gameContext));

      if (allSatisfied)
      {
        return spokenText.Text;
      }
    }

    throw new InvalidOperationException(
      $"No appropriate dialogue option spoken text found for dialogue option {dialogueOptionId}."
    );
  }

  #endregion
}
