using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Abstraction;

/// <summary>
///   Service interface for exploration action result narrations.
/// </summary>
public interface IExplorationActionResultNarrationService
{
  #region Methods

  /// <summary>
  ///   Retrieves an exploration action result narration based on the exploration action result identifier,
  ///   the actor and the world.
  /// </summary>
  Task<ExplorationActionResultNarration> GetExplorationActionResultNarrationAsync(
    Guid explorationActionResultId, Character actor, World world, CancellationToken cancellationToken
  );

  #endregion
}
