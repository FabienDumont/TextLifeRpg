using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TextLifeRpg.Infrastructure.EfDataModels;

/// <summary>
/// EF Core data model representing a narration text.
/// </summary>
[Table("Narrations")]
[PrimaryKey(nameof(Id))]
public class NarrationDataModel
{
  #region Properties

  /// <summary>
  /// Unique identifier.
  /// </summary>
  [Column("Id", Order = 1)]
  [Required]
  public Guid Id { get; set; }

  /// <summary>
  /// Key describing the narration.
  /// </summary>
  [Column("Key", Order = 2)]
  [Required]
  [MaxLength(100)]
  public required string Key { get; set; }

  /// <summary>
  /// Narration text.
  /// </summary>
  [Column("Text", Order = 3)]
  [Required]
  [MaxLength(500)]
  public required string Text { get; set; }

  #endregion
}
