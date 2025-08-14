using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using TextLifeRpg.Domain;

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

  [Column("AddMinutes", Order = 3)]
  public bool AddMinutes { get; set; }

  /// <summary>
  /// Relationship change when getting this result.
  /// </summary>
  [Column("TargetRelationshipValueChange", Order = 4)]
  public int? TargetRelationshipValueChange { get; set; }

  /// <summary>
  /// Represents a fact that can will be learned as a result of the dialogue option.
  /// </summary>
  [Column("LearnFact", Order = 5)]
  public Fact? ActorLearnFact { get; set; }

  /// <summary>
  /// Represents a special action that can will happen as a result of the dialogue option.
  /// </summary>
  [Column("ActorTargetSpecialAction", Order = 6)]
  public ActorTargetSpecialAction? ActorTargetSpecialAction { get; set; }

  /// <summary>
  /// Indicates whether the dialogue ends after selecting this option.
  /// </summary>
  [Column("EndDialogue", Order = 7)]
  public bool EndDialogue { get; set; }

  /// <summary>
  /// Navigation property to the dialogue option.
  /// </summary>
  [ForeignKey(nameof(DialogueOptionId))]
  [Required]
  public DialogueOptionDataModel? DialogueOption { get; set; }

  #endregion
}
