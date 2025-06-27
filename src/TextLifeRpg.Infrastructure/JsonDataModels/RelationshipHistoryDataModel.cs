namespace TextLifeRpg.Infrastructure.JsonDataModels;

/// <summary>
/// JSON data model representing the relationship history between a source character and a target character.
/// </summary>
public class RelationshipHistoryDataModel
{
  #region Properties

  /// <summary>
  /// The date of the first interaction between the source and target characters.
  /// </summary>
  public DateOnly FirstInteraction { get; init; }

  /// <summary>
  /// The date of the most recent interaction between the source and target characters.
  /// </summary>
  public DateOnly LastInteraction { get; init; }

  #endregion
}
