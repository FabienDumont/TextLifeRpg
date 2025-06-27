using TextLifeRpg.Application.InternalUtilities;
using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.JsonDataModels;

namespace TextLifeRpg.Infrastructure.Mappers;

/// <summary>
///   Mapper for converting between <see cref="World" /> domain models and <see cref="WorldDataModel" /> JSON data models.
/// </summary>
public static class WorldMapper
{
  #region Methods

  /// <summary>
  ///   Maps a JSON data model to its domain counterpart.
  /// </summary>
  public static World ToDomain(this WorldDataModel dataModel)
  {
    return dataModel.Map(i => World.Load(
        i.Id, i.CurrentDate, i.Characters.ToDomainCollection(), i.Relationships.ToDomainCollection(),
        i.Schedules.ToDomainCollection()
      )
    );
  }

  /// <summary>
  ///   Maps a domain model to its JSON data model counterpart.
  /// </summary>
  public static WorldDataModel ToDataModel(this World domain)
  {
    return domain.Map(u => new WorldDataModel
      {
        Id = u.Id,
        CurrentDate = u.CurrentDate,
        Characters = u.Characters.ToDataModelCollection(),
        Relationships = u.Relationships.ToDataModelCollection(),
        Schedules = u.Schedules.ToDataModelCollection()
      }
    );
  }

  #endregion
}
