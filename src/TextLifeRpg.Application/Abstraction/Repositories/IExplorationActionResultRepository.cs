using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Abstraction.Repositories;

/// <summary>
/// Repository interface for exploration action results.
/// </summary>
public interface IExplorationActionResultRepository
{
  #region Methods

  /// <summary>
  /// Retrieves exploration action result for given exploration action identifier and game context.
  /// </summary>
  Task<ExplorationActionResult> GetByExplorationActionIdAsync(
    Guid explorationActionId, GameContext gameContext, CancellationToken cancellationToken
  );

  #endregion
}
