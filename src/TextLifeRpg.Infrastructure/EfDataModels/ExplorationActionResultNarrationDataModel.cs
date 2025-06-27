using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TextLifeRpg.Infrastructure.EfDataModels;

/// <summary>
/// EF Core data model representing an exploration action result narration.
/// </summary>
[Table("ExplorationActionResultNarrations")]
[PrimaryKey(nameof(Id))]
public class ExplorationActionResultNarrationDataModel
{
  #region Properties

  [Column("Id")]
  [Required]
  public Guid Id { get; set; }

  [Column("ExplorationActionResultId")]
  [Required]
  public Guid ExplorationActionResultId { get; set; }

  [Column("Text")]
  [Required]
  [MaxLength(500)]
  public required string Text { get; set; }

  [ForeignKey(nameof(ExplorationActionResultId))]
  public ExplorationActionResultDataModel? ExplorationActionResult { get; set; }

  #endregion
}
