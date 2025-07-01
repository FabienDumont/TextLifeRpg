using TextLifeRpg.Application.Abstraction;
using TextLifeRpg.Application.Abstraction.Repositories;
using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Services;

/// <summary>
/// Service for managing items.
/// </summary>
public class ItemService(IItemRepository itemRepository) : IItemService
{
  #region Implmentation of IItemService

  /// <inheritdoc />
  public async Task<Item?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
  {
    return await itemRepository.GetByIdAsync(id, cancellationToken);
  }

  /// <inheritdoc />
  public async Task<Item?> GetByNameAsync(string name, CancellationToken cancellationToken)
  {
    return await itemRepository.GetByNameAsync(name, cancellationToken);
  }

  /// <inheritdoc />
  public async Task<IReadOnlyCollection<Item>> GetAllAsync(CancellationToken cancellationToken)
  {
    return await itemRepository.GetAllAsync(cancellationToken);
  }

  #endregion
}
