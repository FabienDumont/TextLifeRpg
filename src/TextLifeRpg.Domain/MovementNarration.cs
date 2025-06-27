namespace TextLifeRpg.Domain;

/// <summary>
///   Domain class representing a movement narration.
/// </summary>
public class MovementNarration
{
  #region Properties

  /// <summary>
  ///   Unique identifier.
  /// </summary>
  public Guid Id { get; }

  /// <summary>
  ///   Identifier of the associated movement.
  /// </summary>
  public Guid MovementId { get; }

  /// <summary>
  ///   Narration text template.
  /// </summary>
  public string Text { get; }

  #endregion

  #region Ctors

  /// <summary>
  ///   Private constructor used internally.
  /// </summary>
  private MovementNarration(Guid id, Guid movementId, string text)
  {
    Id = id;
    MovementId = movementId;
    Text = text;
  }

  #endregion

  #region Methods

  /// <summary>
  ///   Factory method to create a new instance.
  /// </summary>
  public static MovementNarration Create(Guid movementId, string text)
  {
    return new MovementNarration(Guid.NewGuid(), movementId, text);
  }

  /// <summary>
  ///   Factory method to load an existing instance from persistence.
  /// </summary>
  public static MovementNarration Load(Guid id, Guid movementId, string text)
  {
    return new MovementNarration(id, movementId, text);
  }

  #endregion
}
