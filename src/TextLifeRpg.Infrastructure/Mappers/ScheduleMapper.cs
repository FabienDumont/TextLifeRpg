using TextLifeRpg.Application.InternalUtilities;
using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.JsonDataModels;

namespace TextLifeRpg.Infrastructure.Mappers;

/// <summary>
/// Mapper for converting between <see cref="Schedule" /> domain models
/// and <see cref="ScheduleDataModel" /> JSON data models.
/// </summary>
public static class ScheduleMapper
{
  #region Methods

  /// <summary>
  /// Maps a JSON data model to its domain counterpart.
  /// </summary>
  public static Schedule ToDomain(this ScheduleDataModel dataModel)
  {
    return dataModel.Map(i => Schedule.Create(i.CharacterId, i.Entries.ToDomainCollection()));
  }

  /// <summary>
  /// Maps a collection of JSON data models to a collection of domain models.
  /// </summary>
  public static List<Schedule> ToDomainCollection(this IEnumerable<ScheduleDataModel> dataModels)
  {
    return dataModels.MapCollection(ToDomain);
  }

  /// <summary>
  /// Maps a domain model to its JSON data model counterpart.
  /// </summary>
  public static ScheduleDataModel ToDataModel(this Schedule domain)
  {
    return domain.Map(u => new ScheduleDataModel
      {
        CharacterId = u.CharacterId,
        Entries = u.Entries.ToDataModelCollection()
      }
    );
  }

  /// <summary>
  /// Maps a collection of domain models to a collection of JSON data models.
  /// </summary>
  public static List<ScheduleDataModel> ToDataModelCollection(this IEnumerable<Schedule> domains)
  {
    return domains.MapCollection(ToDataModel);
  }

  #endregion
}
