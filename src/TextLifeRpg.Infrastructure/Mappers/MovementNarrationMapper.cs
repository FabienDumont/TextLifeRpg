using TextLifeRpg.Application.InternalUtilities;
using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.EfDataModels;

namespace TextLifeRpg.Infrastructure.Mappers;

/// <summary>
/// Mapper for converting between <see cref="MovementNarration" /> domain models and
/// <see cref="MovementNarrationDataModel" /> EF data models.
/// </summary>
public static class MovementNarrationMapper
{
  #region Methods

  /// <summary>
  /// Maps an EF data model to its domain counterpart.
  /// </summary>
  public static MovementNarration ToDomain(this MovementNarrationDataModel dataModel)
  {
    return dataModel.Map(i => MovementNarration.Load(i.Id, i.MovementId, i.Text));
  }

  /// <summary>
  /// Maps a collection of EF data models to domain models.
  /// </summary>
  public static List<MovementNarration> ToDomainCollection(this IEnumerable<MovementNarrationDataModel> dataModels)
  {
    return dataModels.MapCollection(ToDomain);
  }

  /// <summary>
  /// Maps a domain model to its EF data model counterpart.
  /// </summary>
  public static MovementNarrationDataModel ToDataModel(this MovementNarration domain)
  {
    return domain.Map(u => new MovementNarrationDataModel
      {
        Id = u.Id,
        MovementId = u.MovementId,
        Text = u.Text
      }
    );
  }

  #endregion
}
