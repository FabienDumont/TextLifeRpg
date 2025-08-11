using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Abstraction.Repositories;

/// <summary>
/// Repository interface for exploration action result narrations.
/// </summary>
public interface IExplorationActionResultNarrationRepository
{
  #region Methods

  /// <summary>
  /// Retrieves exploration action result narration for given exploration action result identifier and game context.
  /// </summary>
  Task<string> GetByExplorationActionResultIdAsync(
    Guid explorationActionResultId, GameContext gameContext, CancellationToken cancellationToken
  );

  #endregion
}
