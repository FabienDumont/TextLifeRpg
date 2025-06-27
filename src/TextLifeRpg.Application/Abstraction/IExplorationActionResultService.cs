using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Abstraction;

/// <summary>
/// Service interface for exploration action results.
/// </summary>
public interface IExplorationActionResultService
{
  #region Methods

  /// <summary>
  /// Retrieves an exploration action result based on the exploration action identifier, the actor and the world.
  /// </summary>
  Task<ExplorationActionResult> GetExplorationActionResultAsync(
    Guid explorationActionId, Character actor, World world, CancellationToken cancellationToken
  );

  #endregion
}
