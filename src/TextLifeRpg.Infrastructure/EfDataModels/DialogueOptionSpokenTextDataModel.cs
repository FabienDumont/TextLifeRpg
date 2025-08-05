using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TextLifeRpg.Infrastructure.EfDataModels;

/// <summary>
/// EF Core data model representing a dialogue option spoken text.
/// </summary>
[Table("DialogueOptionSpokenTexts")]
[PrimaryKey(nameof(Id))]
public class DialogueOptionSpokenTextDataModel
{
  #region Properties

  [Column("Id")]
  [Required]
  public Guid Id { get; set; }

  [Column("DialogueOptionId")]
  [Required]
  public Guid DialogueOptionId { get; set; }

  [Column("Text")]
  [Required]
  [MaxLength(500)]
  public required string Text { get; set; }

  [ForeignKey(nameof(DialogueOptionId))]
  public DialogueOptionDataModel? DialogueOption { get; set; }

  #endregion
}
