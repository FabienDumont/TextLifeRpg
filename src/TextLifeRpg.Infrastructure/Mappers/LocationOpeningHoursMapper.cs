using TextLifeRpg.Application.InternalUtilities;
using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.EfDataModels;

namespace TextLifeRpg.Infrastructure.Mappers;

/// <summary>
///   Mapper for converting between <see cref="LocationOpeningHours" /> domain models and
///   <see cref="LocationOpeningHoursDataModel" /> EF data models.
/// </summary>
public static class LocationOpeningHoursMapper
{
  #region Methods

  /// <summary>
  ///   Maps an EF data model to its domain counterpart.
  /// </summary>
  public static LocationOpeningHours ToDomain(this LocationOpeningHoursDataModel dataModel)
  {
    return dataModel.Map(i => LocationOpeningHours.Load(i.Id, i.LocationId, i.DayOfWeek, i.OpensAt, i.ClosesAt));
  }

  /// <summary>
  ///   Maps a collection of EF data models to domain models.
  /// </summary>
  public static List<LocationOpeningHours> ToDomainCollection(
    this IEnumerable<LocationOpeningHoursDataModel> dataModels
  )
  {
    return dataModels.MapCollection(ToDomain);
  }

  /// <summary>
  ///   Maps a domain model to its EF data model counterpart.
  /// </summary>
  public static LocationOpeningHoursDataModel ToDataModel(this LocationOpeningHours domain)
  {
    return domain.Map(u => new LocationOpeningHoursDataModel
      {
        Id = u.Id,
        LocationId = u.LocationId,
        DayOfWeek = u.DayOfWeek,
        OpensAt = u.OpensAt,
        ClosesAt = u.ClosesAt
      }
    );
  }

  /// <summary>
  ///   Maps a collection of domain models to EF data models.
  /// </summary>
  public static List<LocationOpeningHoursDataModel> ToDataModelCollection(
    this IEnumerable<LocationOpeningHours> domains
  )
  {
    return domains.MapCollection(ToDataModel);
  }

  #endregion
}
