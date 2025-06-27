using TextLifeRpg.Application.InternalUtilities;
using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.EfDataModels;

namespace TextLifeRpg.Infrastructure.Mappers;

/// <summary>
///   Mapper for converting between <see cref="Greeting" /> domain models and <see cref="GreetingDataModel" /> EF data
///   models.
/// </summary>
public static class GreetingMapper
{
  #region Methods

  /// <summary>
  ///   Maps an EF data model to its domain counterpart.
  /// </summary>
  public static Greeting ToDomain(this GreetingDataModel dataModel)
  {
    return dataModel.Map(i => Greeting.Load(i.Id, i.SpokenText));
  }

  /// <summary>
  ///   Maps a collection of EF data models to domain models.
  /// </summary>
  public static List<Greeting> ToDomainCollection(this IEnumerable<GreetingDataModel> dataModels)
  {
    return dataModels.MapCollection(ToDomain);
  }

  /// <summary>
  ///   Maps a domain model to its EF data model counterpart.
  /// </summary>
  public static GreetingDataModel ToDataModel(this Greeting domain)
  {
    return domain.Map(u => new GreetingDataModel
      {
        Id = u.Id,
        SpokenText = u.SpokenText
      }
    );
  }

  #endregion
}
