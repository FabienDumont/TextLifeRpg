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

  public async Task<IReadOnlyList<string>> GetByDialogueOptionResultIdAsync(
    Guid dialogueOptionResultId, GameContext gameContext, CancellationToken cancellationToken
  )
  {
    var resultSpokenTexts = await Context.DialogueOptionResultSpokenTexts
      .Where(n => n.DialogueOptionResultId == dialogueOptionResultId).ToListAsync(cancellationToken);

    var spokenTextIds = resultSpokenTexts.Select(n => n.Id).ToList();

    var allConditions = await Context.Conditions
      .Where(c => c.ContextType == ContextType.DialogueOptionResultSpokenText && spokenTextIds.Contains(c.ContextId))
      .ToListAsync(cancellationToken);

    var validTexts = new List<string>();

    foreach (var resultSpokenText in resultSpokenTexts)
    {
      var conditions = allConditions.Where(c => c.ContextId == resultSpokenText.Id);
      var allSatisfied = conditions.All(condition => ConditionEvaluator.EvaluateCondition(condition, gameContext));
      if (allSatisfied)
      {
        validTexts.Add(resultSpokenText.Text);
      }
    }

    return validTexts;
  }

  #endregion
}
