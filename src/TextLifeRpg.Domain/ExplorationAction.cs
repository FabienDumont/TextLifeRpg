namespace TextLifeRpg.Domain;

/// <summary>
/// Domain class representing an exploration action.
/// </summary>
public class ExplorationAction
{
  #region Properties

  /// <summary>
  /// Unique identifier.
  /// </summary>
  public Guid Id { get; }

  /// <summary>
  /// Identifier of the location where the action can be done.
  /// </summary>
  public Guid LocationId { get; }

  /// <summary>
  /// Identifier of the room where the action can be done.
  /// </summary>
  public Guid? RoomId { get; }

  /// <summary>
  /// The action's label.
  /// </summary>
  public string Label { get; }

  /// <summary>
  /// The needed minutes to do the action.
  /// </summary>
  public int NeededMinutes { get; }

  #endregion

  #region Ctors

  /// <summary>
  /// Private constructor used internally.
  /// </summary>
  private ExplorationAction(Guid id, Guid locationId, Guid? roomId, string label, int neededMinutes)
  {
    Id = id;
    LocationId = locationId;
    RoomId = roomId;
    Label = label;
    NeededMinutes = neededMinutes;
  }

  #endregion

  #region Methods

  /// <summary>
  /// Factory method to create a new instance.
  /// </summary>
  public static ExplorationAction Create(Guid locationId, Guid? roomId, string label, int neededMinutes)
  {
    return new ExplorationAction(Guid.NewGuid(), locationId, roomId, label, neededMinutes);
  }

  /// <summary>
  /// Factory method to load an existing instance from persistence.
  /// </summary>
  public static ExplorationAction Load(Guid id, Guid locationId, Guid? roomId, string label, int neededMinutes)
  {
    return new ExplorationAction(id, locationId, roomId, label, neededMinutes);
  }

  #endregion
}
