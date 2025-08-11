using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TextLifeRpg.Infrastructure.EfDataModels;

/// <summary>
/// EF Core data model representing a link between a dialogue option result and a follow up dialogue option.
/// </summary>
[Table("DialogueOptionResultNextDialogues")]
[PrimaryKey(nameof(Id))]
public class DialogueOptionResultNextDialogueOption
{
  #region Properties

  /// <summary>
  /// Unique identifier.
  /// </summary>
  [Column("Id", Order = 1)]
  [Required]
  public Guid Id { get; set; }

  /// <summary>
  /// The unique identifier for the related dialogue option result.
  /// </summary>
  [Column("DialogueOptionResultId", Order = 2)]
  [Required]
  public required Guid DialogueOptionResultId { get; set; }

  /// <summary>
  /// Identifier of the next dialogue option associated with the result of the current dialogue option.
  /// </summary>
  [Column("NextDialogueOptionId", Order = 3)]
  [Required]
  public required Guid NextDialogueOptionId { get; set; }

  /// <summary>
  /// Represents the order in which follow-up dialogue options are presented for a given dialogue option result.
  /// </summary>
  [Column("Order", Order = 4)]
  [Required]
  public required int Order { get; set; }

  /// <summary>
  /// Navigation property to the dialogue option result.
  /// </summary>
  [ForeignKey(nameof(DialogueOptionResultId))]
  [Required]
  public DialogueOptionResultDataModel? DialogueOptionResult { get; set; }

  /// <summary>
  /// Navigation property to the next dialogue option.
  /// </summary>
  [ForeignKey(nameof(NextDialogueOptionId))]
  [Required]
  public DialogueOptionDataModel? NextDialogueOption { get; set; }

  #endregion
}
