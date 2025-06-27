using TextLifeRpg.Application.InternalUtilities;
using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.JsonDataModels;

namespace TextLifeRpg.Infrastructure.Mappers;

/// <summary>
///   Mapper for converting between <see cref="ScheduleEntry" /> domain models
///   and <see cref="ScheduleEntryDataModel" /> JSON data models.
/// </summary>
public static class ScheduleEntryMapper
{
  #region Methods

  /// <summary>
  ///   Maps a JSON data model to its domain counterpart.
  /// </summary>
  public static ScheduleEntry ToDomain(this ScheduleEntryDataModel dataModel)
  {
    return dataModel.Map(i => new ScheduleEntry(i.Day, i.StartHour, i.EndHour, i.LocationId, i.RoomId));
  }

  /// <summary>
  ///   Maps a collection of JSON data models to a collection of domain models.
  /// </summary>
  public static List<ScheduleEntry> ToDomainCollection(this IEnumerable<ScheduleEntryDataModel> dataModels)
  {
    return dataModels.MapCollection(ToDomain);
  }

  /// <summary>
  ///   Maps a domain model to its JSON data model counterpart.
  /// </summary>
  public static ScheduleEntryDataModel ToDataModel(this ScheduleEntry domain)
  {
    return domain.Map(u => new ScheduleEntryDataModel
      {
        Day = u.Day,
        StartHour = u.StartHour,
        EndHour = u.EndHour,
        LocationId = u.LocationId,
        RoomId = u.RoomId
      }
    );
  }

  /// <summary>
  ///   Maps a collection of domain models to a collection of JSON data models.
  /// </summary>
  public static List<ScheduleEntryDataModel> ToDataModelCollection(this IEnumerable<ScheduleEntry> domains) =>
    domains.MapCollection(ToDataModel);

  #endregion
}
