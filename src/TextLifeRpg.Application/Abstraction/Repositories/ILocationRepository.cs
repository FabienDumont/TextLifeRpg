using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Abstraction.Repositories;

/// <summary>
///   Repository interface for locations.
/// </summary>
public interface ILocationRepository
{
  #region Methods

  /// <summary>
  ///   Retrieves a location by its identifier.
  /// </summary>
  Task<Location> GetByIdAsync(Guid id, CancellationToken cancellationToken);

  /// <summary>
  ///   Retrieves a location by its name.
  /// </summary>
  Task<Location?> GetByNameAsync(string name, CancellationToken ct);

  #endregion
}
