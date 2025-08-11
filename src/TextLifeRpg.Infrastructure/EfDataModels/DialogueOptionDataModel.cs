using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TextLifeRpg.Infrastructure.EfDataModels;

/// <summary>
/// EF Core data model representing a dialogue option entry.
/// </summary>
[Table("DialogueOptions")]
[PrimaryKey(nameof(Id))]
public class DialogueOptionDataModel
{
  #region Properties

  /// <summary>
  /// Unique identifier.
  /// </summary>
  [Column("Id", Order = 1)]
  [Required]
  public Guid Id { get; set; }

  /// <summary>
  /// The spoken text of the greeting.
  /// </summary>
  [Column("Label", Order = 2)]
  [Required]
  [MaxLength(500)]
  public required string Label { get; set; }

  #endregion
}
