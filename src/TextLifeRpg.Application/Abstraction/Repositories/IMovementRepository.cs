using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Abstraction.Repositories;

/// <summary>
///   Repository interface for movements.
/// </summary>
public interface IMovementRepository
{
  #region Methods

  /// <summary>
  ///   Retrieves movements from the specified location and room.
  /// </summary>
  Task<List<Movement>> GetMovementsAsync(
    Guid currentLocationId, Guid? currentRoomId, CancellationToken cancellationToken
  );

  #endregion
}
