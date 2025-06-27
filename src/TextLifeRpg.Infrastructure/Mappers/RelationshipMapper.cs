using TextLifeRpg.Application.InternalUtilities;
using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.JsonDataModels;

namespace TextLifeRpg.Infrastructure.Mappers;

/// <summary>
///   Mapper for converting between <see cref="Relationship" /> domain models
///   and <see cref="RelationshipDataModel" /> JSON data models.
/// </summary>
public static class RelationshipMapper
{
  #region Methods

  /// <summary>
  ///   Maps a JSON data model to its domain counterpart.
  /// </summary>
  public static Relationship ToDomain(this RelationshipDataModel dataModel)
  {
    return dataModel.Map(i => Relationship.Load(
        i.Id, i.SourceCharacterId, i.TargetCharacterId, i.Value, i.Type, i.History.ToDomain()
      )
    );
  }

  /// <summary>
  ///   Maps a collection of JSON data models to a collection of domain models.
  /// </summary>
  public static List<Relationship> ToDomainCollection(this IEnumerable<RelationshipDataModel> dataModels)
  {
    return dataModels.MapCollection(ToDomain);
  }

  /// <summary>
  ///   Maps a domain model to its JSON data model counterpart.
  /// </summary>
  public static RelationshipDataModel ToDataModel(this Relationship domain)
  {
    return domain.Map(u => new RelationshipDataModel
      {
        Id = u.Id,
        SourceCharacterId = u.SourceCharacterId,
        TargetCharacterId = u.TargetCharacterId,
        Value = u.Value,
        Type = u.Type,
        History = u.History.ToDataModel()
      }
    );
  }

  /// <summary>
  ///   Maps a collection of domain models to a collection of JSON data models.
  /// </summary>
  public static List<RelationshipDataModel> ToDataModelCollection(this IEnumerable<Relationship> domains)
  {
    return domains.MapCollection(ToDataModel);
  }

  #endregion
}
