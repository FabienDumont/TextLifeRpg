using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Abstraction;

/// <summary>
/// Service interface for managing movements.
/// </summary>
public interface IMovementService
{
  #region Methods

  /// <summary>
  /// Retrieves a list of available movements based on the current location and room.
  /// </summary>
  /// <param name="currentLocationId">Identifier of the current location.</param>
  /// <param name="currentRoomId">Optional identifier of the current room.</param>
  /// <param name="day">The day.</param>
  /// <param name="time">The time.</param>
  /// <param name="cancellationToken">A cancellation token.</param>
  /// <returns>A list of valid movements the player can perform.</returns>
  Task<List<Movement>> GetAvailableMovementsAsync(
    Guid currentLocationId, Guid? currentRoomId, DayOfWeek day, TimeSpan time, CancellationToken cancellationToken
  );

  /// <summary>
  /// Executes a movement operation for the specified game save.
  /// </summary>
  /// <param name="movement">The movement to be executed.</param>
  /// <param name="save">The current game save state.</param>
  /// <param name="cancellationToken">A cancellation token to signal the operation should be canceled.</param>
  /// <returns>A task that represents the asynchronous execution of the movement.</returns>
  public Task ExecuteAsync(Movement movement, GameSave save, CancellationToken cancellationToken);

  #endregion
}
