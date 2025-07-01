using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Abstraction.Repositories;

/// <summary>
/// Repository interface for items.
/// </summary>
public interface IItemRepository
{
  #region Methods

  /// <summary>
  /// Retrieve an item from a given identifier.
  /// </summary>
  /// <param name="id">The item identifier.</param>
  /// <param name="cancellationToken">A cancellation token.</param>
  /// <returns>The corresponding item.</returns>
  Task<Item?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

  /// <summary>
  /// Retrieve an item from a given name.
  /// </summary>
  /// <param name="name">The item name.</param>
  /// <param name="cancellationToken">A cancellation token.</param>
  /// <returns>The corresponding item.</returns>
  Task<Item?> GetByNameAsync(string name, CancellationToken cancellationToken);

  /// <summary>
  /// Retrieves all available items.
  /// </summary>
  Task<IReadOnlyCollection<Item>> GetAllAsync(CancellationToken cancellationToken);

  #endregion
}
