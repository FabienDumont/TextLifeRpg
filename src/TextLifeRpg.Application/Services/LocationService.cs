using TextLifeRpg.Application.Abstraction;
using TextLifeRpg.Application.Abstraction.Repositories;
using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Services;

/// <summary>
///   Service for managing locations.
/// </summary>
public class LocationService(ILocationRepository locationRepository) : ILocationService
{
  #region Implementation of ILocationService

  /// <inheritdoc />
  public async Task<Location> GetByIdAsync(Guid id, CancellationToken cancellationToken)
  {
    return await locationRepository.GetByIdAsync(id, cancellationToken);
  }

  /// <inheritdoc />
  public async Task<Location?> GetByNameAsync(string name, CancellationToken cancellationToken)
  {
    return await locationRepository.GetByNameAsync(name, cancellationToken);
  }

  /// <inheritdoc />
  public async Task<bool> IsLocationOpenAsync(
    Guid locationId, DayOfWeek day, TimeSpan time, CancellationToken cancellationToken
  )
  {
    var location = await GetByIdAsync(locationId, cancellationToken);
    return location.IsOpenAt(day, time);
  }

  #endregion
}
