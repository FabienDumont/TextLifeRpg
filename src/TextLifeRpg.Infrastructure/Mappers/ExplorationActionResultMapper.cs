using TextLifeRpg.Application.InternalUtilities;
using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.EfDataModels;

namespace TextLifeRpg.Infrastructure.Mappers;

/// <summary>
///   Mapper for converting between <see cref="ExplorationActionResult" /> domain models and
///   <see cref="ExplorationActionResultDataModel" /> EF data models.
/// </summary>
public static class ExplorationActionResultMapper
{
  #region Methods

  /// <summary>
  ///   Maps an EF data model to its domain counterpart.
  /// </summary>
  public static ExplorationActionResult ToDomain(this ExplorationActionResultDataModel dataModel)
  {
    return dataModel.Map(model => ExplorationActionResult.Load(
        model.Id, model.ExplorationActionId, model.AddMinutes, model.EnergyChange, model.MoneyChange
      )
    );
  }

  /// <summary>
  ///   Maps a collection of EF data models to domain models.
  /// </summary>
  public static List<ExplorationActionResult> ToDomainCollection(
    this IEnumerable<ExplorationActionResultDataModel> dataModels
  )
  {
    return dataModels.MapCollection(ToDomain);
  }

  /// <summary>
  ///   Maps a domain model to its EF data model counterpart.
  /// </summary>
  public static ExplorationActionResultDataModel ToDataModel(this ExplorationActionResult domain)
  {
    return domain.Map(result => new ExplorationActionResultDataModel
      {
        Id = result.Id,
        ExplorationActionId = result.ExplorationActionId,
        AddMinutes = result.AddMinutes,
        EnergyChange = result.EnergyChange,
        MoneyChange = result.MoneyChange
      }
    );
  }

  /// <summary>
  ///   Maps a collection of domain models to EF data models.
  /// </summary>
  public static List<ExplorationActionResultDataModel> ToDataModelCollection(
    this IEnumerable<ExplorationActionResult> domains
  )
  {
    return domains.MapCollection(ToDataModel);
  }

  #endregion
}
