using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Abstraction;

/// <summary>
/// Service interface for exploration actions.
/// </summary>
public interface IExplorationActionService
{
  #region Methods

  /// <summary>
  /// Retrieves a list of exploration actions available at a specified location and room based on the current day and time.
  /// </summary>
  /// <param name="locationId">The unique identifier for the location where the actions are being queried.</param>
  /// <param name="roomId">The unique identifier for the room within the location, or null if no specific room is specified.</param>
  /// <param name="currentDay">The current day of the week determining contextual action availability.</param>
  /// <param name="currentTime">The current time of day determining contextual action availability.</param>
  /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
  /// <returns>A task that represents the asynchronous operation, containing a list of available exploration actions.</returns>
  Task<List<ExplorationAction>> GetExplorationActionsAsync(
    Guid locationId, Guid? roomId, DayOfWeek currentDay, TimeSpan currentTime, CancellationToken cancellationToken
  );

  /// <summary>
  /// Executes the specified exploration action within the context of the provided game save.
  /// </summary>
  /// <param name="action">The exploration action to be executed.</param>
  /// <param name="save">The current game save containing game state and context.</param>
  /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
  /// <returns>A task that represents the asynchronous operation.</returns>
  public Task ExecuteAsync(ExplorationAction action, GameSave save, CancellationToken cancellationToken);

  #endregion
}
