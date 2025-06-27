using TextLifeRpg.Application.InternalUtilities;
using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.EfDataModels;

namespace TextLifeRpg.Infrastructure.Mappers;

/// <summary>
/// Mapper for converting between <see cref="Room" /> domain models and <see cref="RoomDataModel" /> EF data models.
/// </summary>
public static class RoomMapper
{
  #region Methods

  /// <summary>
  /// Maps an EF data model to its domain counterpart.
  /// </summary>
  public static Room ToDomain(this RoomDataModel dataModel)
  {
    return dataModel.Map(i => Room.Load(i.Id, i.LocationId, i.Name, i.IsPlayerSpawn));
  }

  /// <summary>
  /// Maps a collection of EF data models to domain models.
  /// </summary>
  public static List<Room> ToDomainCollection(this IEnumerable<RoomDataModel> dataModels)
  {
    return dataModels.MapCollection(ToDomain);
  }

  /// <summary>
  /// Maps a domain model to its EF data model counterpart.
  /// </summary>
  public static RoomDataModel ToDataModel(this Room domain)
  {
    return domain.Map(u => new RoomDataModel
      {
        Id = u.Id,
        LocationId = u.LocationId,
        Name = u.Name,
        IsPlayerSpawn = u.IsPlayerSpawn
      }
    );
  }

  #endregion
}
