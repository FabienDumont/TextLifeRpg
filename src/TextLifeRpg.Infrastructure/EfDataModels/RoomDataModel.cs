using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TextLifeRpg.Infrastructure.EfDataModels;

/// <summary>
/// EF Core data model representing a room within a location.
/// </summary>
[Table("Rooms")]
[PrimaryKey(nameof(Id))]
public class RoomDataModel
{
  #region Properties

  /// <summary>
  /// Unique identifier.
  /// </summary>
  [Column("Id", Order = 1)]
  [Required]
  public Guid Id { get; set; }

  /// <summary>
  /// Identifier of the location this room belongs to.
  /// </summary>
  [Column("LocationId", Order = 2)]
  [Required]
  public Guid LocationId { get; set; }

  /// <summary>
  /// Name of the room.
  /// </summary>
  [Column("Name", Order = 3)]
  [Required]
  [MaxLength(100)]
  public required string Name { get; set; }

  /// <summary>
  /// Indicates whether this room is the default entry point for the location.
  /// </summary>
  [Column("IsPlayerSpawn", Order = 4)]
  [Required]
  public bool IsPlayerSpawn { get; set; }

  /// <summary>
  /// Navigation property to the parent location.
  /// </summary>
  [ForeignKey(nameof(LocationId))]
  public LocationDataModel? Location { get; set; }

  #endregion
}
