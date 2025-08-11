using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TextLifeRpg.Infrastructure.EfDataModels;

/// <summary>
/// EF Core data model representing a dialogue option result narration.
/// </summary>
[Table("DialogueOptionResultNarrations")]
[PrimaryKey(nameof(Id))]
public class DialogueOptionResultNarrationDataModel
{
  #region Properties

  [Column("Id")]
  [Required]
  public Guid Id { get; set; }

  [Column("DialogueOptionResultId")]
  [Required]
  public Guid DialogueOptionResultId { get; set; }

  [Column("Text")]
  [Required]
  [MaxLength(500)]
  public required string Text { get; set; }

  [ForeignKey(nameof(DialogueOptionResultId))]
  public DialogueOptionResultDataModel? DialogueOptionResult { get; set; }

  #endregion
}
