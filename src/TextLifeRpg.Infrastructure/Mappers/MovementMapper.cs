using TextLifeRpg.Application.InternalUtilities;
using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.EfDataModels;

namespace TextLifeRpg.Infrastructure.Mappers;

/// <summary>
///   Mapper for converting between <see cref="Movement" /> domain models and <see cref="MovementDataModel" /> EF data
///   models.
/// </summary>
public static class MovementMapper
{
  #region Methods

  /// <summary>
  ///   Maps an EF data model to its domain counterpart.
  /// </summary>
  public static Movement ToDomain(this MovementDataModel dataModel)
  {
    return dataModel.Map(i => Movement.Load(
        i.Id, i.FromLocationId, i.FromRoomId, i.ToLocationId, i.ToRoomId, i.RequiredItemId
      )
    );
  }

  /// <summary>
  ///   Maps a collection of EF data models to domain models.
  /// </summary>
  public static List<Movement> ToDomainCollection(this IEnumerable<MovementDataModel> dataModels)
  {
    return dataModels.MapCollection(ToDomain);
  }

  /// <summary>
  ///   Maps a domain model to its EF data model counterpart.
  /// </summary>
  public static MovementDataModel ToDataModel(this Movement domain)
  {
    return domain.Map(u => new MovementDataModel
      {
        Id = u.Id,
        FromLocationId = u.FromLocationId,
        FromRoomId = u.FromRoomId,
        ToLocationId = u.ToLocationId,
        ToRoomId = u.ToRoomId,
        RequiredItemId = u.RequiredItemId
      }
    );
  }

  #endregion
}
