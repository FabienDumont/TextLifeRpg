using Microsoft.EntityFrameworkCore;
using TextLifeRpg.Application.Abstraction.Repositories;
using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.Helper;
using TextLifeRpg.Infrastructure.Mappers;

namespace TextLifeRpg.Infrastructure.EfRepositories;

/// <summary>
/// Repository for narrations.
/// </summary>
public class NarrationRepository(ApplicationContext context) : RepositoryBase(context), INarrationRepository
{
  #region Implementation of INarrationRepository

  /// <inheritdoc />
  public async Task<Narration> GetNarrationByKeyAsync(
    string key, GameContext gameContext, CancellationToken cancellationToken
  )
  {
    var narrations = await Context.Narrations.Where(n => n.Key.Equals(key)).ToListAsync(cancellationToken);

    var allConditions = await Context.Conditions.Where(c => c.ContextType == ContextType.Narration)
      .ToListAsync(cancellationToken);

    foreach (var narration in narrations)
    {
      var conditions = allConditions.Where(c => c.ContextId == narration.Id);

      var allSatisfied = conditions.All(condition => ConditionEvaluator.EvaluateCondition(condition, gameContext));

      if (allSatisfied)
      {
        return narration.ToDomain();
      }
    }

    throw new InvalidOperationException($"No narration found for key {key}.");
  }

  #endregion
}
