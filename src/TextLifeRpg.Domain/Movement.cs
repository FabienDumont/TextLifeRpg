namespace TextLifeRpg.Domain;

/// <summary>
/// Domain class representing a movement between locations or rooms.
/// </summary>
public class Movement
{
  #region Properties

  /// <summary>
  /// Unique identifier.
  /// </summary>
  public Guid Id { get; }

  /// <summary>
  /// Identifier of the location the movement starts from.
  /// </summary>
  public Guid FromLocationId { get; }

  /// <summary>
  /// Identifier of the room the movement starts from (optional).
  /// </summary>
  public Guid? FromRoomId { get; }

  /// <summary>
  /// Identifier of the destination location.
  /// </summary>
  public Guid ToLocationId { get; }

  /// <summary>
  /// Identifier of the destination room (optional).
  /// </summary>
  public Guid? ToRoomId { get; }

  /// <summary>
  /// Identifier of the item required to make the movement (optional).
  /// </summary>
  public Guid? RequiredItemId { get; }

  #endregion

  #region Ctors

  /// <summary>
  /// Private constructor used internally.
  /// </summary>
  private Movement(
    Guid id, Guid fromLocationId, Guid? fromRoomId, Guid toLocationId, Guid? toRoomId, Guid? requiredItemId
  )
  {
    Id = id;
    FromLocationId = fromLocationId;
    FromRoomId = fromRoomId;
    ToLocationId = toLocationId;
    ToRoomId = toRoomId;
    RequiredItemId = requiredItemId;
  }

  #endregion

  #region Methods

  /// <summary>
  /// Factory method to create a new instance.
  /// </summary>
  public static Movement Create(
    Guid fromLocationId, Guid? fromRoomId, Guid toLocationId, Guid? toRoomId, Guid? requiredItemId
  )
  {
    return new Movement(Guid.NewGuid(), fromLocationId, fromRoomId, toLocationId, toRoomId, requiredItemId);
  }

  /// <summary>
  /// Factory method to load an existing instance from persistence.
  /// </summary>
  public static Movement Load(
    Guid id, Guid fromLocationId, Guid? fromRoomId, Guid toLocationId, Guid? toRoomId, Guid? requiredItemId
  )
  {
    return new Movement(id, fromLocationId, fromRoomId, toLocationId, toRoomId, requiredItemId);
  }

  #endregion
}
