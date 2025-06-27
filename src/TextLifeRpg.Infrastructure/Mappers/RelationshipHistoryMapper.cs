using TextLifeRpg.Application.InternalUtilities;
using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.JsonDataModels;

namespace TextLifeRpg.Infrastructure.Mappers;

/// <summary>
/// Mapper for converting between <see cref="RelationshipHistory" /> domain models
/// and <see cref="RelationshipHistoryDataModel" /> JSON data models.
/// </summary>
public static class RelationshipHistoryMapper
{
  #region Methods

  /// <summary>
  /// Maps a JSON data model to its domain counterpart.
  /// </summary>
  public static RelationshipHistory ToDomain(this RelationshipHistoryDataModel dataModel)
  {
    return dataModel.Map(i => RelationshipHistory.Load(i.FirstInteraction, i.LastInteraction));
  }

  /// <summary>
  /// Maps a domain model to its JSON data model counterpart.
  /// </summary>
  public static RelationshipHistoryDataModel ToDataModel(this RelationshipHistory domain)
  {
    return domain.Map(u => new RelationshipHistoryDataModel
      {
        FirstInteraction = u.FirstInteraction,
        LastInteraction = u.LastInteraction
      }
    );
  }

  #endregion
}
