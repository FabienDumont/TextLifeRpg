using System.ComponentModel.DataAnnotations;

namespace TextLifeRpg.Domain;

/// <summary>
///   Domain class representing a relationship between a source character and a target character.
/// </summary>
public class Relationship
{
  #region Properties

  /// <summary>
  ///   Unique identifier of the relationship.
  /// </summary>
  public Guid Id { get; }

  /// <summary>
  ///   The character who holds the feelings.
  /// </summary>
  public Guid SourceCharacterId { get; }

  /// <summary>
  ///   The character who is the target of the relationship.
  /// </summary>
  public Guid TargetCharacterId { get; }

  /// <summary>
  ///   Relationship intensity, from -100 (hatred) to +100 (deep affection).
  /// </summary>
  public int Value { get; set; }

  /// <summary>
  ///   Optional relationship type (friendship, rivalry, etc.).
  /// </summary>
  public RelationshipType Type { get; set; }

  /// <summary>
  ///   Historical interaction log and metrics.
  /// </summary>
  public RelationshipHistory History { get; }

  #endregion

  #region Ctors

  /// <summary>
  ///   Private constructor for internal use.
  /// </summary>
  private Relationship(
    Guid id, Guid sourceCharacterId, Guid targetCharacterId, int value, RelationshipType type,
    RelationshipHistory history
  )
  {
    Id = id;
    SourceCharacterId = sourceCharacterId;
    TargetCharacterId = targetCharacterId;
    Value = value;
    Type = type;
    History = history;
  }

  #endregion

  #region Methods

  /// <summary>
  ///   Factory method to create a new instance.
  /// </summary>
  public static Relationship Create(
    Guid sourceCharacterId, Guid targetCharacterId, RelationshipType type, DateOnly firstInteraction,
    DateOnly lastInteraction, int value
  )
  {
    var history = RelationshipHistory.Create(firstInteraction, lastInteraction);

    return new Relationship(Guid.NewGuid(), sourceCharacterId, targetCharacterId, value, type, history);
  }

  /// <summary>
  ///   Factory method to load an existing instance from persistence.
  /// </summary>
  public static Relationship Load(
    Guid id, Guid sourceCharacterId, Guid targetCharacterId, int value, RelationshipType type,
    RelationshipHistory history
  )
  {
    return new Relationship(id, sourceCharacterId, targetCharacterId, value, type, history);
  }

  /// <summary>
  ///   Adjusts the relationship intensity by the given delta, clamps the result,
  ///   updates the interaction timestamp, and changes the relationship type if applicable.
  /// </summary>
  public void AdjustValue(int delta, DateOnly interactionDate)
  {
    Value = Math.Clamp(Value + delta, -100, 100);
    History.UpdateLastInteraction(interactionDate);

    // Auto-update type for dynamic categories
    if (Type is RelationshipType.Acquaintance or RelationshipType.Friend or RelationshipType.Enemy)
    {
      Type = Value switch
      {
        >= 50 => RelationshipType.Friend,
        <= -50 => RelationshipType.Enemy,
        _ => RelationshipType.Acquaintance
      };
    }
  }

  #endregion
}

public enum RelationshipType
{
  [Display(Name = "Acquaintance")]
  Acquaintance,

  [Display(Name = "Friend")]
  Friend,

  [Display(Name = "Enemy")]
  Enemy,

  [Display(Name = "Casual romantic partner")]
  CasualRomanticPartner,

  [Display(Name = "Romantic partner")]
  RomanticPartner,

  [Display(Name = "Spouse")]
  Spouse,

  [Display(Name = "Sibling")]
  Sibling,

  [Display(Name = "Parent")]
  Parent,

  [Display(Name = "Grandparent")]
  Grandparent,

  [Display(Name = "Child")]
  Child,

  [Display(Name = "Grandchild")]
  Grandchild
}
