using Microsoft.EntityFrameworkCore;
using TextLifeRpg.Application.Abstraction.Repositories;
using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.Mappers;

namespace TextLifeRpg.Infrastructure.EfRepositories;

/// <summary>
/// Repository for items.
/// </summary>
public class ItemRepository(ApplicationContext context) : RepositoryBase(context), IItemRepository
{
  #region Implementation of IItemRepository

  /// <inheritdoc />
  public async Task<Item?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
  {
    var dataModel = await Context.Items.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

    return dataModel?.ToDomain();
  }

  /// <inheritdoc />
  public async Task<Item?> GetByNameAsync(string name, CancellationToken cancellationToken)
  {
    var dataModel = await Context.Items.FirstOrDefaultAsync(t => t.Name == name, cancellationToken);

    return dataModel?.ToDomain();
  }

  /// <inheritdoc />
  public async Task<IReadOnlyCollection<Item>> GetAllAsync(CancellationToken cancellationToken)
  {
    var dataModels = await Context.Items.ToListAsync(cancellationToken).ConfigureAwait(false);

    return dataModels.ToDomainCollection();
  }

  #endregion
}
