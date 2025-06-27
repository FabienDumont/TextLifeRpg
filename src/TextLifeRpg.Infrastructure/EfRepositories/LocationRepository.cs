using Microsoft.EntityFrameworkCore;
using TextLifeRpg.Application.Abstraction.Repositories;
using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.Mappers;

namespace TextLifeRpg.Infrastructure.EfRepositories;

/// <summary>
/// Repository for locations.
/// </summary>
public class LocationRepository(ApplicationContext context) : RepositoryBase(context), ILocationRepository
{
  #region Implementation of ILocationRepository

  /// <inheritdoc />
  public async Task<Location> GetByIdAsync(Guid id, CancellationToken cancellationToken)
  {
    var dataModel = await Context.Locations.FindAsync([id], cancellationToken).ConfigureAwait(false);

    if (dataModel is null)
    {
      throw new InvalidOperationException($"Location with ID {id} was not found.");
    }

    return dataModel.ToDomain();
  }

  /// <inheritdoc />
  public async Task<Location?> GetByNameAsync(string name, CancellationToken ct)
  {
    var dataModel = await Context.Locations.FirstOrDefaultAsync(l => l.Name == name, ct);

    return dataModel?.ToDomain();
  }

  #endregion
}
