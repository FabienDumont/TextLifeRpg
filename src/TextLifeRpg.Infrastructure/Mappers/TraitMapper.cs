using TextLifeRpg.Application.InternalUtilities;
using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.EfDataModels;

namespace TextLifeRpg.Infrastructure.Mappers;

/// <summary>
///   Mapper for converting between <see cref="Trait" /> domain models and <see cref="TraitDataModel" /> EF data models.
/// </summary>
public static class TraitMapper
{
  #region Methods

  /// <summary>
  ///   Maps an EF data model to its domain counterpart.
  /// </summary>
  public static Trait ToDomain(this TraitDataModel dataModel)
  {
    return dataModel.Map(i => Trait.Load(i.Id, i.Name));
  }

  /// <summary>
  ///   Maps a collection of EF data models to domain models.
  /// </summary>
  public static List<Trait> ToDomainCollection(this IEnumerable<TraitDataModel> dataModels)
  {
    return dataModels.MapCollection(ToDomain);
  }

  /// <summary>
  ///   Maps a domain model to its EF data model counterpart.
  /// </summary>
  public static TraitDataModel ToDataModel(this Trait domain)
  {
    return domain.Map(u => new TraitDataModel
      {
        Id = u.Id,
        Name = u.Name
      }
    );
  }

  #endregion
}
