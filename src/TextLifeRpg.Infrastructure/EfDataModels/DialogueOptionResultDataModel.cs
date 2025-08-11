using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TextLifeRpg.Infrastructure.EfDataModels;

/// <summary>
/// EF Core data model representing a dialogue option result.
/// </summary>
[Table("DialogueOptionResults")]
[PrimaryKey(nameof(Id))]
public class DialogueOptionResultDataModel
{
  #region Properties

  /// <summary>
  /// Unique identifier.
  /// </summary>
  [Column("Id", Order = 1)]
  [Required]
  public Guid Id { get; set; }

  /// <summary>
  /// Identifier of the dialogue option this entry belongs to.
  /// </summary>
  [Column("DialogueOptionId", Order = 2)]
  [Required]
  public required Guid DialogueOptionId { get; set; }

  /// <summary>
  /// Relationship change when getting this result.
  /// </summary>
  [Column("TargetRelationshipValueChange", Order = 3)]
  public int? TargetRelationshipValueChange { get; set; }

  /// <summary>
  /// Indicates whether the dialogue ends after selecting this option.
  /// </summary>
  [Column("EndDialogue", Order = 4)]
  public bool EndDialogue { get; set; }

  /// <summary>
  /// Navigation property to the dialogue option.
  /// </summary>
  [ForeignKey(nameof(DialogueOptionId))]
  [Required]
  public DialogueOptionDataModel? DialogueOption { get; set; }

  #endregion
}
