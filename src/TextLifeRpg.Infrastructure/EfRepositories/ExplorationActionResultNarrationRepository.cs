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
public class ExplorationActionResultNarrationRepository(ApplicationContext context)
  : RepositoryBase(context), IExplorationActionResultNarrationRepository
{
  #region Implementation of IExplorationActionResultNarrationRepository

  /// <inheritdoc />
  public async Task<ExplorationActionResultNarration> GetByExplorationActionResultIdAsync(
    Guid explorationActionResultId, GameContext gameContext, CancellationToken cancellationToken
  )
  {
    var narrations = await Context.ExplorationActionResultNarrations
      .Where(n => n.ExplorationActionResultId == explorationActionResultId).ToListAsync(cancellationToken);

    var narrationIds = narrations.Select(n => n.Id).ToList();
    var allConditions = await Context.Conditions
      .Where(c => c.ContextType == ContextType.ExplorationActionResultNarration && narrationIds.Contains(c.ContextId))
      .ToListAsync(cancellationToken);

    foreach (var explorationActionResultNarration in narrations)
    {
      var conditions = allConditions.Where(c => c.ContextId == explorationActionResultNarration.Id);

      var allSatisfied = conditions.All(condition => ConditionEvaluator.EvaluateCondition(condition, gameContext));

      if (allSatisfied)
      {
        return explorationActionResultNarration.ToDomain();
      }
    }

    throw new InvalidOperationException(
      $"No appropriate exploration action result narration found for exploration action result {explorationActionResultId}."
    );
  }

  #endregion
}
