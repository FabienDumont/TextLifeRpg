using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Abstraction;

/// <summary>
/// Service interface for managing locations.
/// </summary>
public interface ILocationService
{
  #region Methods

  /// <summary>
  /// Retrieves a location by its unique identifier.
  /// </summary>
  /// <param name="id">The unique identifier of the location.</param>
  /// <param name="cancellationToken">A cancellation token.</param>
  /// <returns>The matching location.</returns>
  Task<Location> GetByIdAsync(Guid id, CancellationToken cancellationToken);

  /// <summary>
  /// Retrieves a location by its name.
  /// </summary>
  /// <param name="name">The name of the location.</param>
  /// <param name="cancellationToken">A cancellation token.</param>
  /// <returns>The matching location.</returns>
  Task<Location?> GetByNameAsync(string name, CancellationToken cancellationToken);

  /// <summary>
  /// Checks whether a location is open at a given day and time.
  /// </summary>
  /// <param name="locationId">Identifier of the current location.</param>
  /// <param name="day">The day.</param>
  /// <param name="time">The time of the day.</param>
  /// <param name="cancellationToken">A cancellation token.</param>
  /// <returns>A list of valid movements the player can perform.</returns>
  Task<bool> IsLocationOpenAsync(Guid locationId, DayOfWeek day, TimeSpan time, CancellationToken cancellationToken);

  #endregion
}
