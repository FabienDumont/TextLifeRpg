using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TextLifeRpg.Infrastructure.EfDataModels;

/// <summary>
/// EF Core data model representing a movement between locations or rooms.
/// </summary>
[Table("Movements")]
[PrimaryKey(nameof(Id))]
public class MovementDataModel
{
  #region Properties

  /// <summary>
  /// Unique identifier.
  /// </summary>
  [Column("Id", Order = 1)]
  [Required]
  public Guid Id { get; set; }

  /// <summary>
  /// Identifier of the starting room (optional).
  /// If null, movement starts directly from the location.
  /// </summary>
  [Column("FromRoomId", Order = 2)]
  public Guid? FromRoomId { get; set; }

  /// <summary>
  /// Identifier of the starting location.
  /// </summary>
  [Column("FromLocationId", Order = 3)]
  [Required]
  public Guid FromLocationId { get; set; }

  /// <summary>
  /// Identifier of the destination room (optional).
  /// </summary>
  [Column("ToRoomId", Order = 4)]
  public Guid? ToRoomId { get; set; }

  /// <summary>
  /// Identifier of the destination location.
  /// </summary>
  [Column("ToLocationId", Order = 5)]
  [Required]
  public Guid ToLocationId { get; set; }

  /// <summary>
  /// Identifier of the item required to complete the movement (optional).
  /// </summary>
  [Column("RequiredItemId", Order = 6)]
  public Guid? RequiredItemId { get; set; }

  /// <summary>
  /// Navigation to the source room.
  /// </summary>
  [ForeignKey(nameof(FromRoomId))]
  public RoomDataModel? FromRoom { get; set; }

  /// <summary>
  /// Navigation to the source location.
  /// </summary>
  [ForeignKey(nameof(FromLocationId))]
  public LocationDataModel? FromLocation { get; set; }

  /// <summary>
  /// Navigation to the target room.
  /// </summary>
  [ForeignKey(nameof(ToRoomId))]
  public RoomDataModel? ToRoom { get; set; }

  /// <summary>
  /// Navigation to the source location.
  /// </summary>
  [ForeignKey(nameof(ToLocationId))]
  public LocationDataModel? ToLocation { get; set; }

  #endregion
}
