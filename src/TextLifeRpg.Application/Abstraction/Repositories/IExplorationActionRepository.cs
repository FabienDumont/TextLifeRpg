using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Abstraction.Repositories;

/// <summary>
/// Repository interface for exploration actions.
/// </summary>
public interface IExplorationActionRepository
{
  #region Methods

  /// <summary>
  /// Retrieves exploration actions by for given location and room identifiers.
  /// </summary>
  Task<List<ExplorationAction>> GetByLocationAndRoomIdAsync(
    Guid locationId, Guid? roomId, CancellationToken cancellationToken
  );

  #endregion
}
