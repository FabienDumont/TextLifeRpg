using Microsoft.EntityFrameworkCore;
using TextLifeRpg.Application.Abstraction.Repositories;
using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.Mappers;

namespace TextLifeRpg.Infrastructure.EfRepositories;

/// <summary>
/// Repository for rooms.
/// </summary>
public class RoomRepository(ApplicationContext context) : RepositoryBase(context), IRoomRepository
{
  #region Implementation of IRoomRepository

  /// <inheritdoc />
  public async Task<Room> GetByIdAsync(Guid id, CancellationToken cancellationToken)
  {
    var dataModel = await Context.Rooms.FindAsync([id], cancellationToken).ConfigureAwait(false);

    if (dataModel is null)
    {
      throw new InvalidOperationException($"Room with ID {id} was not found.");
    }

    return dataModel.ToDomain();
  }

  /// <inheritdoc />
  public async Task<Room?> GetPlayerSpawnAsync(CancellationToken cancellationToken)
  {
    var rooms = await Context.Rooms.ToListAsync(cancellationToken).ConfigureAwait(false);
    var spawn = rooms.FirstOrDefault(r => r.IsPlayerSpawn);

    return spawn?.ToDomain();
  }

  #endregion
}
