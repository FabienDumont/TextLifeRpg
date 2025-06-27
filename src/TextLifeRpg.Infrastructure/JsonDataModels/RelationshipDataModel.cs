using TextLifeRpg.Domain;

namespace TextLifeRpg.Infrastructure.JsonDataModels;

/// <summary>
///   JSON data model representing the relationship between a source character and a target character.
/// </summary>
public class RelationshipDataModel
{
  #region Properties

  /// <summary>
  ///   Unique identifier.
  /// </summary>
  public Guid Id { get; init; }

  /// <summary>
  ///   The character who holds the feelings.
  /// </summary>
  public Guid SourceCharacterId { get; init; }

  /// <summary>
  ///   The character who is the target of the relationship.
  /// </summary>
  public Guid TargetCharacterId { get; init; }

  /// <summary>
  ///   Relationship intensity, from -100 (hatred) to +100 (deep affection).
  /// </summary>
  public int Value { get; init; }

  /// <summary>
  ///   Optional relationship type (friendship, rivalry, etc.).
  /// </summary>
  public RelationshipType Type { get; init; }

  /// <summary>
  ///   Historical interaction log and metrics.
  /// </summary>
  public required RelationshipHistoryDataModel History { get; init; }

  #endregion
}
