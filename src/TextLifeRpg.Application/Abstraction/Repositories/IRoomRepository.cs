using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Abstraction.Repositories;

/// <summary>
/// Repository interface for rooms.
/// </summary>
public interface IRoomRepository
{
  #region Methods

  /// <summary>
  /// Retrieves a room by its identifier.
  /// </summary>
  Task<Room> GetByIdAsync(Guid id, CancellationToken cancellationToken);

  /// <summary>
  /// Retrieves the player spawn.
  /// </summary>
  Task<Room?> GetPlayerSpawnAsync(CancellationToken cancellationToken);

  #endregion
}
