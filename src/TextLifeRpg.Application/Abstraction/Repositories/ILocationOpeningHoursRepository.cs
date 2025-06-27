using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Abstraction.Repositories;

/// <summary>
///   Repository interface for locations' opening hours.
/// </summary>
public interface ILocationOpeningHoursRepository
{
  #region Methods

  /// <summary>
  ///   Retrieves a location's opening hours.
  /// </summary>
  Task<List<LocationOpeningHours>> GetByLocationIdAsync(Guid locationId, CancellationToken cancellationToken);

  #endregion
}
