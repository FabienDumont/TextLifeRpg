using TextLifeRpg.Application.InternalUtilities;
using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.EfDataModels;

namespace TextLifeRpg.Infrastructure.Mappers;

/// <summary>
/// Mapper for converting between <see cref="ExplorationAction" /> domain models and
/// <see cref="ExplorationActionDataModel" /> EF data models.
/// </summary>
public static class ExplorationActionMapper
{
  #region Methods

  /// <summary>
  /// Maps an EF data model to its domain counterpart.
  /// </summary>
  public static ExplorationAction ToDomain(this ExplorationActionDataModel dataModel)
  {
    return dataModel.Map(i => ExplorationAction.Load(i.Id, i.LocationId, i.RoomId, i.Label, i.NeededMinutes));
  }

  /// <summary>
  /// Maps a collection of EF data models to domain models.
  /// </summary>
  public static List<ExplorationAction> ToDomainCollection(this IEnumerable<ExplorationActionDataModel> dataModels)
  {
    return dataModels.MapCollection(ToDomain);
  }

  /// <summary>
  /// Maps a domain model to its EF data model counterpart.
  /// </summary>
  public static ExplorationActionDataModel ToDataModel(this ExplorationAction domain)
  {
    return domain.Map(u => new ExplorationActionDataModel
      {
        Id = u.Id,
        LocationId = u.LocationId,
        RoomId = u.RoomId,
        Label = u.Label,
        NeededMinutes = u.NeededMinutes
      }
    );
  }

  /// <summary>
  /// Maps a collection of domain models to EF data models.
  /// </summary>
  public static List<ExplorationActionDataModel> ToDataModelCollection(this IEnumerable<ExplorationAction> domains)
  {
    return domains.MapCollection(ToDataModel);
  }

  #endregion
}
