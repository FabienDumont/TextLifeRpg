using TextLifeRpg.Application.InternalUtilities;
using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.EfDataModels;

namespace TextLifeRpg.Infrastructure.Mappers;

/// <summary>
///   Mapper for converting between <see cref="Narration" /> domain models and
///   <see cref="NarrationDataModel" /> EF data models.
/// </summary>
public static class NarrationMapper
{
  #region Methods

  /// <summary>
  ///   Maps an EF data model to its domain counterpart.
  /// </summary>
  public static Narration ToDomain(this NarrationDataModel dataModel)
  {
    return dataModel.Map(i => Narration.Load(i.Id, i.Key, i.Text));
  }

  /// <summary>
  ///   Maps a collection of EF data models to domain models.
  /// </summary>
  public static List<Narration> ToDomainCollection(this IEnumerable<NarrationDataModel> dataModels)
  {
    return dataModels.MapCollection(ToDomain);
  }

  /// <summary>
  ///   Maps a domain model to its EF data model counterpart.
  /// </summary>
  public static NarrationDataModel ToDataModel(this Narration domain)
  {
    return domain.Map(u => new NarrationDataModel
      {
        Id = u.Id,
        Key = u.Key,
        Text = u.Text
      }
    );
  }

  #endregion
}
