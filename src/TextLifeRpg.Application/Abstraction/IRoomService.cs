using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Abstraction;

/// <summary>
/// Service interface for managing rooms.
/// </summary>
public interface IRoomService
{
  #region Methods

  /// <summary>
  /// Retrieves a room by its identifier.
  /// </summary>
  /// <param name="id">The unique identifier of the room.</param>
  /// <param name="cancellationToken">A cancellation token.</param>
  /// <returns>The room matching the specified identifier.</returns>
  Task<Room> GetByIdAsync(Guid id, CancellationToken cancellationToken);

  /// <summary>
  /// Retrieves the player spawn room.
  /// </summary>
  /// <param name="cancellationToken">A cancellation token.</param>
  /// <returns>The player spawn room.</returns>
  Task<Room?> GetPlayerSpawnAsync(CancellationToken cancellationToken);

  #endregion
}
