using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Abstraction.Repositories;

/// <summary>
/// Repository interface for traits.
/// </summary>
public interface ITraitRepository
{
  #region Methods

  /// <summary>
  /// Retrieve a trait from a given identifier.
  /// </summary>
  /// <param name="id">The trait identifier.</param>
  /// <param name="cancellationToken">A cancellation token.</param>
  /// <returns>The corresponding trait.</returns>
  Task<Trait?> GetById(Guid id, CancellationToken cancellationToken);

  /// <summary>
  /// Retrieves all available traits.
  /// </summary>
  Task<IReadOnlyCollection<Trait>> GetAllAsync(CancellationToken cancellationToken);

  /// <summary>
  /// Retrieves incompatible traits.
  /// </summary>
  /// <param name="cancellationToken">A cancellation token.</param>
  /// <returns>A read-only collection of tuple of incompatible traits.</returns>
  Task<IReadOnlyCollection<(Trait, Trait)>> GetIncompatibleTraitsAsync(CancellationToken cancellationToken);

  /// <summary>
  /// Retrieves traits compatible with the selected trait identifiers.
  /// </summary>
  Task<IReadOnlyCollection<Trait>> GetCompatibleTraitsAsync(
    IEnumerable<Guid> selectedTraitsIdsEnumerable, CancellationToken cancellationToken
  );

  #endregion
}
