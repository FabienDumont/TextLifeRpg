using TextLifeRpg.Application.InternalUtilities;
using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.EfDataModels;

namespace TextLifeRpg.Infrastructure.Mappers;

/// <summary>
///   Mapper for converting between <see cref="ExplorationActionResultNarration" /> domain models and
///   <see cref="ExplorationActionResultNarrationDataModel" /> EF data models.
/// </summary>
public static class ExplorationActionResultNarrationMapper
{
  #region Methods

  /// <summary>
  ///   Maps an EF data model to its domain counterpart.
  /// </summary>
  public static ExplorationActionResultNarration ToDomain(this ExplorationActionResultNarrationDataModel dataModel)
  {
    return dataModel.Map(model => ExplorationActionResultNarration.Load(
        model.Id, model.ExplorationActionResultId, model.Text
      )
    );
  }

  /// <summary>
  ///   Maps a collection of EF data models to domain models.
  /// </summary>
  public static List<ExplorationActionResultNarration> ToDomainCollection(
    this IEnumerable<ExplorationActionResultNarrationDataModel> dataModels
  )
  {
    return dataModels.MapCollection(ToDomain);
  }

  /// <summary>
  ///   Maps a domain model to its EF data model counterpart.
  /// </summary>
  public static ExplorationActionResultNarrationDataModel ToDataModel(this ExplorationActionResultNarration domain)
  {
    return domain.Map(model => new ExplorationActionResultNarrationDataModel
      {
        Id = model.Id,
        ExplorationActionResultId = model.ExplorationActionResultId,
        Text = model.Text
      }
    );
  }

  /// <summary>
  ///   Maps a collection of domain models to EF data models.
  /// </summary>
  public static List<ExplorationActionResultNarrationDataModel> ToDataModelCollection(
    this IEnumerable<ExplorationActionResultNarration> domains
  )
  {
    return domains.MapCollection(ToDataModel);
  }

  #endregion
}
