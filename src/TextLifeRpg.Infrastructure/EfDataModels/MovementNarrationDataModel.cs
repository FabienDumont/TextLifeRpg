using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TextLifeRpg.Infrastructure.EfDataModels;

/// <summary>
///   EF Core data model representing the narration text associated with a movement.
/// </summary>
[Table("MovementNarrations")]
[PrimaryKey(nameof(Id))]
public class MovementNarrationDataModel
{
  #region Properties

  /// <summary>
  ///   Unique identifier.
  /// </summary>
  [Column("Id", Order = 1)]
  [Required]
  public Guid Id { get; set; }

  /// <summary>
  ///   Identifier of the associated movement.
  /// </summary>
  [Column("MovementId", Order = 2)]
  public Guid MovementId { get; set; }

  /// <summary>
  ///   Narration text describing the movement.
  /// </summary>
  [Column("Text", Order = 3)]
  [Required]
  [MaxLength(500)]
  public required string Text { get; set; }

  [ForeignKey(nameof(MovementId))]
  public MovementDataModel? Movement { get; set; }

  #endregion
}
