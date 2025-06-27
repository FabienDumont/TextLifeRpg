using Microsoft.EntityFrameworkCore;
using TextLifeRpg.Application.Abstraction.Repositories;
using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.Helper;
using TextLifeRpg.Infrastructure.Mappers;

namespace TextLifeRpg.Infrastructure.EfRepositories;

/// <summary>
/// Repository for exploration action results.
/// </summary>
public class ExplorationActionResultRepository(ApplicationContext context)
  : RepositoryBase(context), IExplorationActionResultRepository
{
  #region Implementation of IExplorationActionResultRepository

  /// <inheritdoc />
  public async Task<ExplorationActionResult> GetByExplorationActionIdAsync(
    Guid explorationActionId, GameContext gameContext, CancellationToken cancellationToken
  )
  {
    var results = await Context.ExplorationActionResults.Where(n => n.ExplorationActionId == explorationActionId)
      .ToListAsync(cancellationToken);

    var narrationIds = results.Select(n => n.Id).ToList();
    var allConditions = await Context.Conditions
      .Where(c => c.ContextType == ContextType.ExplorationActionResult && narrationIds.Contains(c.ContextId))
      .ToListAsync(cancellationToken);

    foreach (var explorationActionResult in results)
    {
      var conditions = allConditions.Where(c => c.ContextId == explorationActionResult.Id);

      var allSatisfied = conditions.All(condition => ConditionEvaluator.EvaluateCondition(condition, gameContext));

      if (allSatisfied)
      {
        return explorationActionResult.ToDomain();
      }
    }

    throw new InvalidOperationException(
      $"No appropriate exploration action result found for exploration action {explorationActionId}."
    );
  }

  #endregion
}
