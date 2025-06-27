using Microsoft.EntityFrameworkCore;
using TextLifeRpg.Application.Abstraction.Repositories;
using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.Mappers;

namespace TextLifeRpg.Infrastructure.EfRepositories;

/// <summary>
/// Repository for locations' opening hours.
/// </summary>
public class LocationOpeningHoursRepository(ApplicationContext context)
  : RepositoryBase(context), ILocationOpeningHoursRepository
{
  #region Implementation of ILocationOpeningHoursRepository

  public async Task<List<LocationOpeningHours>> GetByLocationIdAsync(
    Guid locationId, CancellationToken cancellationToken
  )
  {
    var openingHours = await Context.LocationOpeningHours.Where(oh => oh.LocationId == locationId)
      .ToListAsync(cancellationToken).ConfigureAwait(false);

    return openingHours.ToDomainCollection();
  }

  #endregion
}
