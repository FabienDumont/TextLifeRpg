using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TextLifeRpg.Infrastructure.EfDataModels;

/// <summary>
/// EF Core data model representing an exploration action result.
/// </summary>
[Table("ExplorationActionResults")]
[PrimaryKey(nameof(Id))]
public class ExplorationActionResultDataModel
{
  #region Properties

  /// <summary>
  /// Unique identifier.
  /// </summary>
  [Column("Id", Order = 1)]
  [Required]
  public Guid Id { get; set; }

  /// <summary>
  /// Identifier of the exploration action this entry belongs to.
  /// </summary>
  [Column("ExplorationActionId", Order = 2)]
  [Required]
  public required Guid ExplorationActionId { get; set; }

  /// <summary>
  /// Label of the exploration action.
  /// </summary>
  [Column("AddMinutes", Order = 3)]
  [Required]
  public bool AddMinutes { get; set; }

  /// <summary>
  /// Energy change when getting this result.
  /// </summary>
  [Column("EnergyChange", Order = 4)]
  public int? EnergyChange { get; set; }

  /// <summary>
  /// Money change when getting this result.
  /// </summary>
  [Column("MoneyChange", Order = 5)]
  public int? MoneyChange { get; set; }

  /// <summary>
  /// Navigation property to the exploration action.
  /// </summary>
  [ForeignKey(nameof(ExplorationActionId))]
  public ExplorationActionDataModel? ExplorationAction { get; set; }

  #endregion
}
