using Microsoft.EntityFrameworkCore;
using TextLifeRpg.Application.Abstraction.Repositories;
using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.Mappers;

namespace TextLifeRpg.Infrastructure.EfRepositories;

/// <summary>
/// Repository for movements.
/// </summary>
public class MovementRepository(ApplicationContext context) : RepositoryBase(context), IMovementRepository
{
  #region Implementation of IMovementRepository

  /// <inheritdoc />
  public async Task<List<Movement>> GetMovementsAsync(
    Guid currentLocationId, Guid? currentRoomId, CancellationToken cancellationToken
  )
  {
    var movements = await Context.Movements
      .Where(m => m.FromLocationId == currentLocationId && m.FromRoomId == currentRoomId).ToListAsync(cancellationToken)
      .ConfigureAwait(false);

    // Fetch names for sorting
    var locationNames = await Context.Locations.ToDictionaryAsync(l => l.Id, l => l.Name, cancellationToken);

    var roomNames = await Context.Rooms.ToDictionaryAsync(r => r.Id, r => r.Name, cancellationToken);

    // Sort in memory
    var sortedMovements = movements.OrderBy(m => m.ToLocationId == currentLocationId ? 0 : 1)
      .ThenBy(m => locationNames.TryGetValue(m.ToLocationId, out var locName) ? locName : "").ThenBy(m =>
        m.ToRoomId.HasValue && roomNames.TryGetValue(m.ToRoomId.Value, out var roomName) ? roomName : ""
      ).ToList();

    return sortedMovements.ToDomainCollection();
  }

  #endregion
}
